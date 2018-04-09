using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Excel;

namespace GSCPsychTest.Student
{
    public partial class ucStudent : UserControl
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr;

        OpenFileDialog file = new OpenFileDialog();

        public ucStudent(string sGetServer, string dbName)
        {
            InitializeComponent();

            con = new SqlConnection(@"Data Source = " + sGetServer + "; Initial Catalog = " + dbName + "; Integrated Security = true");
        }

        private void ucStudent_Load(object sender, EventArgs e)
        {

        }
        public void LoadExcelFile()
        {
            try
            {
                FileStream stream = new FileStream(file.FileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();
                dataGridGetExcelData.DataSource = result.Tables[0];

                excelReader.Close();

                this.btnTransfer.Enabled = true;

                try
                {
                    for (int numberOfRow = 0; numberOfRow < this.dataGridGetExcelData.Rows.Count; numberOfRow++)
                    {
                        for (int numberOfColBlank = 0; numberOfColBlank < this.dataGridGetExcelData.Columns.Count; numberOfColBlank++)
                        {
                            if (this.dataGridGetExcelData.Rows[numberOfRow].Cells[numberOfColBlank].Value.ToString() == "BLANK")
                            {
                                this.dataGridGetExcelData.Rows[numberOfRow].Cells[numberOfColBlank].Value = 3;
                            }
                        }
                    }
                }
                catch
                {

                }

                this.lblNumOfPsychTaken.Text = "Number of Student take Psychological Test:  " + this.dataGridGetExcelData.Rows.Count;
            }
            catch
            {
                MessageBox.Show("File is Open!!! Must Close.");
            }

        }

        public void InsertData()
        {
            for (int iRow = 0; iRow < this.dataGridGetExcelData.Rows.Count; iRow++ )
            {
                con.Open();

                cmd = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM tblStudent WHERE studIDNum = '"
                    + this.dataGridGetExcelData.Rows[iRow].Cells[1].Value.ToString().Trim().Replace("'", "") + "') BEGIN INSERT INTO tblStudent (studIDNum,studName) VALUES ('"
                    + this.dataGridGetExcelData.Rows[iRow].Cells[1].Value.ToString().Trim().Replace("'", "") + "','"
                    + this.dataGridGetExcelData.Rows[iRow].Cells[109].Value.ToString().Trim().Replace("'", "") + "') END BEGIN UPDATE tblStudent SET studName = '"
                    + this.dataGridGetExcelData.Rows[iRow].Cells[109].Value.ToString().Trim().Replace("'", "") + "' WHERE studIDNum = '"
                    + this.dataGridGetExcelData.Rows[iRow].Cells[1].Value.ToString().Trim().Replace("'", "") + "' END", con);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            file.Title = "Open File Excel";
            file.Filter = "Excel Files |*.xls;*.xlsx;.*.xlsm;";

            if (file.ShowDialog() == DialogResult.OK)
            {
                this.btnTransfer.Enabled = true;
                LoadExcelFile();
            }
            else return;
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            this.pBarTransferData.Visible = true;

            this.pBarTransferData.Maximum = this.dataGridGetExcelData.Rows.Count;
            this.pBarTransferData.Value = pBarTransferData.Minimum;

            MessageBox.Show("Data Successfully Load", "");

            this.pBarTransferData.Visible = false;
        }
    }
}
