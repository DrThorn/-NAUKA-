using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NAUKA_
{
    public partial class Employee_Form : Form
    {
        private int currentStringID = 0;
        private bool EditMode=false;

        public Employee_Form()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string[] nRow = new string[7];
            nRow[0] = tbName.Text;
            nRow[1] = tbFamily.Text;
            nRow[2] = tbPatronymic.Text;
            nRow[3] = dtBirthDate.Text;
            nRow[4] = tbAddress.Text;
            nRow[5] = tbDepartment.Text;
            nRow[6] = tbAbout.Text;
            if (CheckEmptyFillds(nRow))
            {
                if (!EditMode)
                {
                    Program.mainForm.AddEntry(nRow);
                }
                else
                {
                    Program.mainForm.EditEntry(nRow);
                }
                tbName.Text = "";
                tbFamily.Text = "";
                tbPatronymic.Text = "";
                tbAddress.Text = "";
                tbDepartment.Text = "";
                tbAbout.Text = "";
                this.Close();
            }
            
        }

        private bool CheckEmptyFillds(string[] fields)
        {
            for(int i=0; i<fields.Length; i++)
            {
                if (fields[i].Trim() == "") { return false; break; }
            }
            return true;
        }

        public void FillFields(DataGridViewCell[] fields)
        {
            currentStringID = int.Parse(fields[0].Value.ToString());
            tbName.Text = fields[1].Value.ToString();
            tbFamily.Text = fields[2].Value.ToString();
            tbPatronymic.Text = fields[3].Value.ToString();
            dtBirthDate.Text = fields[4].Value.ToString();
            tbAddress.Text = fields[5].Value.ToString();
            tbDepartment.Text = fields[6].Value.ToString();
            tbAbout.Text = fields[7].Value.ToString();
            EditMode = true;   
        }
    }
}
