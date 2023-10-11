using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace _NAUKA_
{
    internal class DB_Controller
    {
       
        private int currentRowID;
        private static DB_Controller _instance=null;
        private Brush myBrush;
        private string nameTable;
        private int entries;
        private string tableColumnNames;
        private string tableColumnWidths;
        private ComboBox status;
        private DataGridView dataGridView;
        private bool shift;

        public DataGridView DataGridView { get => dataGridView; set => dataGridView = value; }
        private SqlConnection connect { get; }
        public string NameTable { get => nameTable; set => nameTable = value; }
        //public string TableColumnNames {set => tableColumnNames = value; }
        //public string TableColumnWidths {set => tableColumnWidths = value; }
        public ComboBox Status { set => status = value; }
        public int CurrentRowID {set => currentRowID = value; }

        public static DB_Controller Instance()
        {
            if (_instance == null)
            {
                _instance = new DB_Controller(8, 
                    "ID|Имя|Фамилия|Отчество|Дата Рождения|Место жительства|Отдел |О себе", 
                    "30|130|130|130|135|340|130|240", 
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Employees.mdf;Integrated Security=True");
                
                return _instance;
            }
            else
            {
                return null;
            }
        }

        

        private void Log(string message)
        {
            
            status.Items.Add(DateTime.Now + " " + message);  
                     
            
        }

       

        protected DB_Controller( int entries, string TableColumnNames,string TableColumnWidths, string connectionString)
        {
            
            //this.status = Program.mainForm.logView;
            tableColumnNames =TableColumnNames;
            tableColumnWidths=TableColumnWidths;
            this.entries = entries;
            connect = new SqlConnection(connectionString);
            
        }

        

        public void Refresh()
        {            
            connect.Open();
            var cmd2 = new SqlCommand($"SELECT * FROM [{nameTable}]", connect);
            var reader = cmd2.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGridView.DataSource = dt;            
            connect.Close();
        }

        public void Format()
        {
            string result = FormatTableHeader(dataGridView, tableColumnWidths, tableColumnNames, 8);
            if (result == "OK")
            {
                Log("Создание заголовка таблицы: " + result);                
            }
            else
            {
                Log("Создание заголовка таблицы: " + result);               
            }
        }
        private string FormatTableHeader(DataGridView dataGridView, string headerWidths, string headerNames, int columnsCount)
        {
            string[] arrayWidths = headerWidths.Split("|"[0]);
            string[] arrayNames = headerNames.Split("|"[0]);
            if (arrayNames.Length == arrayWidths.Length )
            {
                if (columnsCount == arrayWidths.Length && columnsCount == arrayNames.Length)
                {
                    for (int columm = 0; columm < arrayWidths.Length; columm++)
                    {
                        dataGridView.Columns[columm].Width = int.Parse(arrayWidths[columm]);
                        dataGridView.Columns[columm].HeaderText = arrayNames[columm];                            
                    }
                }
                else
                {
                    return "Указано не правильное число колонок";
                }
            }
            else
            {
                return "Несоответствие длин строк форматирования";
            }
            DataGridViewCellStyle style = dataGridView.ColumnHeadersDefaultCellStyle;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.Columns[0].Visible = false;
            return "OK";
        }   

        public void Update(string[] employeeData)
        {            
            try
            {
                connect.Open();
                var command = new SqlCommand($"INSERT INTO [{nameTable}] (Name, Family, Patronymic, BirthDate, Adress, Department, About) VALUES ( @Name, @Family, @Patronymic, @BirthDate, @Adress, @Department, @About)", connect);
                command.Parameters.AddWithValue("@Name", employeeData[0]);
                command.Parameters.AddWithValue("@Family", employeeData[1]);
                command.Parameters.AddWithValue("@Patronymic", employeeData[2]);
                command.Parameters.AddWithValue("@BirthDate", employeeData[3]);
                command.Parameters.AddWithValue("@Adress", employeeData[4]);
                command.Parameters.AddWithValue("@Department", employeeData[5]);
                command.Parameters.AddWithValue("@About", employeeData[6]);
                command.ExecuteNonQuery();
                connect.Close();
                Log("Добавление новой строки: Успешно.");
                Refresh();
            }
            catch (Exception ex) 
            {
                connect.Close();
                Log("Ошибка добавления строки: "+ex.Message);
            }
        }

        public void RemoveEntry(int  id)
        {
            try
            {
                connect.Open();
                var command = new SqlCommand($"DELETE FROM [{nameTable}] WHERE id = {id}", connect);
                
                command.ExecuteNonQuery();    
                
                Log("Удаление строки: Успешно.");
                connect.Close();
                Refresh();
            }
            catch (Exception ex) 
            {
                Log("Удаление строки: "+ex.Message);
                connect.Close();
            }
        }

        public void UpdateEntry(string[] employeeData)
        {
            try
            {
                connect.Open();
                var command = new SqlCommand($"UPDATE {nameTable} " +
                    $"SET Name = @Name , " +
                    $"Family = @Family , " +
                    $"Patronymic = @Patronymic , " +
                    $"BirthDate = @BirthDate , " +
                    $"Adress = @Adress , " +
                    $"Department = @Department , " +
                    $"About = @About " +
                    $"WHERE id = '{currentRowID}'", connect);
                command.Parameters.AddWithValue("@Name", employeeData[0]);
                command.Parameters.AddWithValue("@Family", employeeData[1]);
                command.Parameters.AddWithValue("@Patronymic", employeeData[2]);
                command.Parameters.AddWithValue("@BirthDate", employeeData[3]);
                command.Parameters.AddWithValue("@Adress", employeeData[4]);
                command.Parameters.AddWithValue("@Department", employeeData[5]);
                command.Parameters.AddWithValue("@About", employeeData[6]);                
                command.ExecuteNonQuery();
                connect.Close();
                Log("Редактирование строки: Успешно.");
                Refresh();
            }
            catch(Exception ex)
            {
                connect.Close();
                Log("Ошибка редактирования строки: " + ex.Message);
            }
            
        }
    }
}
