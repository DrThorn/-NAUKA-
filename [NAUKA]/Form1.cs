using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace _NAUKA_
{

    public partial class MainForm : Form
    {
         
        DB_Controller dbController = DB_Controller.Instance();
        

        public MainForm()
        {
            Program.mainForm = this; // теперь mainForm будет ссылкой на форму Form1
            InitializeComponent();
            dbController.DataGridView = dataGridView;
            dbController.Status = logView;
            
            //dbController.TableColumnNames = "ID|Имя|Фамилия|Отчество|Дата Рождения|Место жительства|Отдел |О себе";
            //dbController.TableColumnWidths = "30|130|130|130|130|130|130|130";
        }

        public void AddEntry(string[] employeeData)
        {
            dbController.Update(employeeData);
        }

        public void EditEntry(string[] employeeData)
        {
            dbController.UpdateEntry(employeeData);
        }

       

        

     
        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "employeesDataSet.Table". При необходимости она может быть перемещена или удалена.
            //this.tableTableAdapter.Fill(this.employeesDataSet.Table);
            dbController.NameTable = "Employees";
            dbController.Refresh();             
            dbController.Format();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //tableTableAdapter.Update(this.employeesDataSet.Table);
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                DataGridViewCellCollection cellCollection = dataGridView.CurrentRow.Cells;
                DialogResult dr = MessageBox.Show($"Удалить запись ?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }else if (dr == DialogResult.OK)
                {
                    e.Cancel = true;
                    dbController.RemoveEntry(int.Parse(cellCollection[0].FormattedValue.ToString()));
                }
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Employee_Form form = new Employee_Form();
            form.Owner = this;
            
            form.ShowDialog(this);
        }     
      

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            DataGridViewCell[] nRow = new DataGridViewCell[8];
            if (dataGridView.CurrentRow != null)
            {
                dataGridView.CurrentRow.Cells.CopyTo(nRow, 0);
                Employee_Form form = new Employee_Form();
                form.Owner = this;
                dbController.CurrentRowID = int.Parse(nRow[0].Value.ToString());
                form.FillFields(nRow);
                form.ShowDialog(this);
            }
            else
            {
                Program.mainForm.logView.Items.Add("Ошибка: Нет строк для редактирования.");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
