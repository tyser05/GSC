using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Excel;
using System.Data.SqlClient;

namespace GSCPsychTest.Psych_Test
{
    public partial class ucPsychTest : UserControl
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr;

        OpenFileDialog file = new OpenFileDialog();

        string TScore, Percentile;

        string AnxietyRawScore, DepressionRawScore, SuicidalIdeationRawScore, SubstanceAbuseRawScore, SelfEsteemProblemRawScore,
                InterpersonalProblemRawScore, FamilyProblemRawScore, AcademicProblemRawScore, CareerProblemRawScore;

        string AnxietyTScore, DepressionTScore, SuicidalIdeationTScore, SubstanceAbuseTScore, SelfEsteemProblemTScore,
                InterpersonalProblemTScore, FamilyProblemTScore, AcademicProblemTScore, CareerProblemTScore;

        string IDNumber, StudentIDNumber;

        string sServer, AdmintType, sSemester, sYear;

        public ucPsychTest(string sGetServer, string dbName)
        {
            InitializeComponent();

            con = new SqlConnection(@"Data Source = " + sGetServer + "; Initial Catalog = "+dbName+"; Integrated Security = true");
        }

        private void ucPsychTest_Load(object sender, EventArgs e)
        {

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


        //Insert Data From Excel To DataGrid
        public void LoadPsychTest()
        {
            for (int NumberOfRows = 0; NumberOfRows < this.dataGridGetExcelData.Rows.Count; NumberOfRows++)
            {
                try
                {
                    IDNumber = this.dataGridGetExcelData.Rows[NumberOfRows].Cells[0].Value.ToString();

                   // MessageBox.Show(IDNumber);
                    con.Open();

                    cmd = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM tblStudent WHERE studIDNum = '"
                        + IDNumber + "') BEGIN INSERT INTO tblStudent (studIDNum,studName) VALUES ('"
                        + this.dataGridGetExcelData.Rows[NumberOfRows].Cells[0].Value.ToString().Trim().Replace("'", "") + "','"
                        + this.dataGridGetExcelData.Rows[NumberOfRows].Cells[109].Value.ToString().Trim().Replace("'", "") + "') END BEGIN UPDATE tblStudent SET studName = '"
                        + this.dataGridGetExcelData.Rows[NumberOfRows].Cells[109].Value.ToString().Trim().Replace("'", "") + "' WHERE studIDNum = '"
                        + IDNumber + "' END", con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    //
                    con.Open();

                    cmd = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM PsychologicalTest WHERE StudentIDNumber = '"
                        + IDNumber + "') BEGIN INSERT INTO PsychologicalTest (StudentIDNumber) VALUES ('"
                        + IDNumber + "') END", con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    //
                    con.Open();


                    cmd = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM PsychologicalTestInterpretation WHERE StudentIDNumber = '"
                        + IDNumber + "') BEGIN INSERT INTO PsychologicalTestInterpretation (StudentIDNumber) VALUES ('"
                        + IDNumber + "') END", con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    //

                    //con.Close();
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTest SET StudentIDNumber = '"
                                + this.IDNumber + "', Test1 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[1].Value.ToString() + "',Test2 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[2].Value.ToString() + "',Test3 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[3].Value.ToString() + "',Test4 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[4].Value.ToString() + "',Test5 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[5].Value.ToString() + "',Test6 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[6].Value.ToString() + "',Test7 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[7].Value.ToString() + "',Test8 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[8].Value.ToString() + "',Test9 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[9].Value.ToString() + "',Test10 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[10].Value.ToString() + "', Test11 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[11].Value.ToString() + "',Test12 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[12].Value.ToString() + "',Test13 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[13].Value.ToString() + "',Test14 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[14].Value.ToString() + "',Test15 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[15].Value.ToString() + "',Test16 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[16].Value.ToString() + "',Test17 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[17].Value.ToString() + "',Test18 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[18].Value.ToString() + "',Test19 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[19].Value.ToString() + "',Test20 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[20].Value.ToString() + "', Test21 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[21].Value.ToString() + "',Test22 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[22].Value.ToString() + "',Test23 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[23].Value.ToString() + "',Test24 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[24].Value.ToString() + "',Test25 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[25].Value.ToString() + "',Test26 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[26].Value.ToString() + "',Test27 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[27].Value.ToString() + "',Test28 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[28].Value.ToString() + "',Test29 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[29].Value.ToString() + "',Test30 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[30].Value.ToString() + "', Test31 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[31].Value.ToString() + "',Test32 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[32].Value.ToString() + "',Test33 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[33].Value.ToString() + "',Test34 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[34].Value.ToString() + "',Test35 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[35].Value.ToString() + "',Test36 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[36].Value.ToString() + "',Test37 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[37].Value.ToString() + "',Test38 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[38].Value.ToString() + "',Test39 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[39].Value.ToString() + "',Test40 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[40].Value.ToString() + "', Test41 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[41].Value.ToString() + "',Test42 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[42].Value.ToString() + "',Test43 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[43].Value.ToString() + "',Test44 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[44].Value.ToString() + "',Test45 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[45].Value.ToString() + "',Test46 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[46].Value.ToString() + "',Test47 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[47].Value.ToString() + "',Test48 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[48].Value.ToString() + "',Test49 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[49].Value.ToString() + "',Test50 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[50].Value.ToString() + "', Test51 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[51].Value.ToString() + "',Test52 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[52].Value.ToString() + "',Test53 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[53].Value.ToString() + "',Test54 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[54].Value.ToString() + "',Test55 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[55].Value.ToString() + "',Test56 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[56].Value.ToString() + "',Test57 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[57].Value.ToString() + "',Test58 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[58].Value.ToString() + "',Test59 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[59].Value.ToString() + "',Test60 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[60].Value.ToString() + "', Test61 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[61].Value.ToString() + "',Test62 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[62].Value.ToString() + "',Test63 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[63].Value.ToString() + "',Test64 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[64].Value.ToString() + "',Test65 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[65].Value.ToString() + "',Test66 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[66].Value.ToString() + "',Test67 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[67].Value.ToString() + "',Test68 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[68].Value.ToString() + "',Test69 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[69].Value.ToString() + "',Test70 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[70].Value.ToString() + "', Test71 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[71].Value.ToString() + "',Test72 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[72].Value.ToString() + "',Test73 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[73].Value.ToString() + "',Test74 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[74].Value.ToString() + "',Test75 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[75].Value.ToString() + "',Test76 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[76].Value.ToString() + "',Test77 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[77].Value.ToString() + "',Test78 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[78].Value.ToString() + "',Test79 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[79].Value.ToString() + "',Test80 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[80].Value.ToString() + "', Test81 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[81].Value.ToString() + "',Test82 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[82].Value.ToString() + "',Test83 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[83].Value.ToString() + "',Test84 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[84].Value.ToString() + "',Test85 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[85].Value.ToString() + "',Test86 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[86].Value.ToString() + "',Test87 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[87].Value.ToString() + "',Test88 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[88].Value.ToString() + "',Test89 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[89].Value.ToString() + "',Test90 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[90].Value.ToString() + "', Test91 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[91].Value.ToString() + "',Test92 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[92].Value.ToString() + "',Test93 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[93].Value.ToString() + "',Test94 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[94].Value.ToString() + "',Test95 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[95].Value.ToString() + "',Test96 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[96].Value.ToString() + "',Test97 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[97].Value.ToString() + "',Test98 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[98].Value.ToString() + "',Test99 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[99].Value.ToString() + "',Test100 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[100].Value.ToString() + "', Test101 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[101].Value.ToString() + "',Test102 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[102].Value.ToString() + "',Test103 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[103].Value.ToString() + "',Test104 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[104].Value.ToString() + "',Test105 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[105].Value.ToString() + "',Test106 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[106].Value.ToString() + "',Test107 = '" + dataGridGetExcelData.Rows[NumberOfRows].Cells[107].Value.ToString() + "',Test108 = '"
                                + dataGridGetExcelData.Rows[NumberOfRows].Cells[108].Value.ToString() + "' WHERE StudentIDNumber = '" + this.IDNumber + "'", con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                }
                catch (ArgumentOutOfRangeException aore)
                {
                    //MessageBox.Show(aore.Data.Values.Count.ToString());
                }
                catch (NullReferenceException nre)
                {
                    //MessageBox.Show(nre.Data.ToString());
                }

                //pBarTransferData.Value++;

                int percent = (int)(((double)(pBarTransferData.Value - pBarTransferData.Minimum) /
                (double)(pBarTransferData.Maximum - pBarTransferData.Minimum)) * 100);

                using (Graphics gr = pBarTransferData.CreateGraphics())
                {
                    gr.DrawString(percent.ToString() + "%",
                        SystemFonts.DefaultFont,
                        Brushes.Black,
                        new PointF(pBarTransferData.Width / 2 - (gr.MeasureString(percent.ToString() + "%",
                            SystemFonts.DefaultFont).Width / 2.0F),
                        pBarTransferData.Height / 2 - (gr.MeasureString(percent.ToString() + "%",
                            SystemFonts.DefaultFont).Height / 2.0F)));
                }
            }
        }

        //Load Psychlogical Interpretation
        public void GetIDByPsychologicalInterpretation()
        {
            for (int NumberOfRows = 0; NumberOfRows < this.dataGridGetExcelData.Rows.Count; NumberOfRows++)
            {
                try
                {
                    this.IDNumber = this.dataGridGetExcelData.Rows[NumberOfRows].Cells[0].Value.ToString();
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation SET StudentIDNumber = '"
                                   + this.IDNumber + "' WHERE StudentIDNumber = '"
                                   + this.IDNumber + "'", con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                }
                catch (ArgumentOutOfRangeException aore)
                {
                    //MessageBox.Show(aore.Data.Values.Count.ToString());
                }
                catch (NullReferenceException nre)
                {
                    //MessageBox.Show(nre.Data.ToString());
                }

                int percent = (int)(((double)(pBarTransferData.Value - pBarTransferData.Minimum) /
                (double)(pBarTransferData.Maximum - pBarTransferData.Minimum)) * 100);

                using (Graphics gr = pBarTransferData.CreateGraphics())
                {
                    gr.DrawString(percent.ToString() + "%",
                        SystemFonts.DefaultFont,
                        Brushes.Black,
                        new PointF(pBarTransferData.Width / 2 - (gr.MeasureString(percent.ToString() + "%",
                            SystemFonts.DefaultFont).Width / 2.0F),
                        pBarTransferData.Height / 2 - (gr.MeasureString(percent.ToString() + "%",
                            SystemFonts.DefaultFont).Height / 2.0F)));
                }
            }
        }

        //Anxiety T Score
        public void CalCulateAnxietyTScoreValue()
        {
            for (int NumberOfRows = 0; NumberOfRows < dataGridGetExcelData.Rows.Count; NumberOfRows++)
            {
                try
                {
                    this.IDNumber = this.dataGridGetExcelData.Rows[NumberOfRows].Cells[0].Value.ToString();


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTest.StudentIDNumber, SUM(Test2 + Test11 + Test20 + Test29 + Test38 + Test47 + 
                                Test56 + Test65 + Test74 + Test83 + Test92 + Test101)
                                FROM PsychologicalTest
                                WHERE PsychologicalTest.StudentIDNumber = '"
                                    + IDNumber + "' GROUP BY PsychologicalTest.StudentIDNumber;", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.AnxietyRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    //Update Anxiety Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation SET AnxietyRowScore = '"
                                                + this.AnxietyRawScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                                + IDNumber + "';", con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, AnxietyRowScore
                                FROM PsychologicalTestInterpretation
                                WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);

                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.AnxietyRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    //Anxiety T Score 
                    if (Convert.ToInt32(this.AnxietyRawScore) >= 0 && Convert.ToInt32(this.AnxietyRawScore) <= 11)
                    {
                        TScore = "0";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "12")
                    {
                        TScore = "30";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "13")
                    {
                        TScore = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "14")
                    {
                        TScore = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "15")
                    {
                        TScore = "40";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "16")
                    {
                        TScore = "43";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "17")
                    {
                        TScore = "44";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "18")
                    {
                        TScore = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "19")
                    {
                        TScore = "48";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "20")
                    {
                        TScore = "49";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "21")
                    {
                        TScore = "51";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "22")
                    {
                        TScore = "52";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "23")
                    {
                        TScore = "53";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "24")
                    {
                        TScore = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "25")
                    {
                        TScore = "56";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "26")
                    {
                        TScore = "57";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "27")
                    {
                        TScore = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "28")
                    {
                        TScore = "59";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "29")
                    {
                        TScore = "60";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "30")
                    {
                        TScore = "61";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "31")
                    {
                        TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "32")
                    {
                        TScore = "63";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "33")
                    {
                        TScore = "64";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "34")
                    {
                        TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "35")
                    {
                        TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "36")
                    {
                        TScore = "67";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "37")
                    {
                        TScore = "68";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "38")
                    {
                        TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "39")
                    {
                        TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "40")
                    {
                        TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "41")
                    {
                        TScore = "72";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "42")
                    {
                        TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "43")
                    {
                        TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "44")
                    {
                        TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "45")
                    {
                        TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "46")
                    {
                        TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "47")
                    {
                        TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyRawScore == "48")
                    {
                        TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, AnxietyTScore
                                FROM PsychologicalTestInterpretation
                                WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.AnxietyTScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    //Anxiety % ile

                    if (this.AnxietyTScore == "20")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "21")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "22")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "23")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "24")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "25")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "26")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "27")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "28")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "29")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "30")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "31")
                    {
                        this.Percentile = "3";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "32")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "33")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "34")
                    {
                        this.Percentile = "5";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "35")
                    {
                        this.Percentile = "7";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "36")
                    {
                        this.Percentile = "8";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "37")
                    {
                        this.Percentile = "10";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "38")
                    {
                        this.Percentile = "12";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "39")
                    {
                        this.Percentile = "14";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "40")
                    {
                        this.Percentile = "16";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "41")
                    {
                        this.Percentile = "18";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "42")
                    {
                        this.Percentile = "21";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "43")
                    {
                        this.Percentile = "24";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "44")
                    {
                        this.Percentile = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "45")
                    {
                        this.Percentile = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "46")
                    {
                        this.Percentile = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "47")
                    {
                        this.Percentile = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "48")
                    {
                        this.Percentile = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "49")
                    {
                        this.Percentile = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "50")
                    {
                        this.Percentile = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "51")
                    {
                        this.Percentile = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "52")
                    {
                        this.Percentile = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "53")
                    {
                        this.Percentile = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "54")
                    {
                        this.Percentile = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "55")
                    {
                        this.Percentile = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "56")
                    {
                        this.Percentile = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "57")
                    {
                        this.Percentile = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "58")
                    {
                        this.Percentile = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "59")
                    {
                        this.Percentile = "82";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "60")
                    {
                        this.Percentile = "84";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "61")
                    {
                        this.Percentile = "86";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "62")
                    {
                        this.Percentile = "88";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "63")
                    {
                        this.Percentile = "90";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "64")
                    {
                        this.Percentile = "92";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "65")
                    {
                        this.Percentile = "93";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "66")
                    {
                        this.Percentile = "95";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "67")
                    {
                        this.Percentile = "96";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "68")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "69")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "70")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "71")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "72")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "73")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "74")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AnxietyTScore == "75")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AnxietyTScore == "76")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AnxietyTScore == "77")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AnxietyTScore == "78")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AnxietyTScore == "79")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AnxietyTScore == "80")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AnxietyPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }



                    //======================================================================================================================================================================================
                    //Depression

                    //Depression Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTest.StudentIDNumber, SUM(Test4 +  Test13 +  Test22 +  Test31 + Test40 + Test49 +
                                Test58 + Test67 + Test76 + Test85 + Test94 + Test103)
                                FROM PsychologicalTest
                                WHERE PsychologicalTest.StudentIDNumber = '"
                                    + IDNumber + "' GROUP BY PsychologicalTest.StudentIDNumber;", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.DepressionRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    //Update Depression Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                            SET DepressionRowScore = '" + this.DepressionRawScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                + IDNumber + "';", con);
                    cmd.ExecuteNonQuery();

                    con.Close();


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, DepressionRowScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.DepressionRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    //Depression T Score
                    if (this.DepressionRawScore == "0" || this.DepressionRawScore == "1" || this.DepressionRawScore == "2" || this.DepressionRawScore == "3" || this.DepressionRawScore == "4" ||
                        this.DepressionRawScore == "5" || this.DepressionRawScore == "6" || this.DepressionRawScore == "7" || this.DepressionRawScore == "8" || this.DepressionRawScore == "9" ||
                        this.DepressionRawScore == "10" || this.DepressionRawScore == "11")
                    {
                        this.TScore = "0";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "12")
                    {
                        this.TScore = "27";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    else if (this.DepressionRawScore == "13")
                    {
                        this.TScore = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "14")
                    {
                        this.TScore = "43";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "15")
                    {
                        this.TScore = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "16")
                    {
                        this.TScore = "49";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "17")
                    {
                        this.TScore = "51";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "18")
                    {
                        this.TScore = "53";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "19")
                    {
                        this.TScore = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "20")
                    {
                        this.TScore = "55";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "21")
                    {
                        this.TScore = "57";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "22")
                    {
                        this.TScore = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "23")
                    {
                        this.TScore = "59";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "24")
                    {
                        this.TScore = "60";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "25")
                    {
                        this.TScore = "61";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "26")
                    {
                        this.TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "27")
                    {
                        this.TScore = "63";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "28")
                    {
                        this.TScore = "64";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "29")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "30")
                    {
                        this.TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "31")
                    {
                        this.TScore = "67";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "32")
                    {
                        this.TScore = "68";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "33")
                    {
                        this.TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "34")
                    {
                        this.TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "35")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "36")
                    {
                        this.TScore = "72";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "37")
                    {
                        this.TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "38")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "39")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "40")
                    {
                        this.TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "41")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "42")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "43")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "44")
                    {
                        this.TScore = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "45")
                    {
                        this.TScore = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "46")
                    {
                        this.TScore = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "47")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionRawScore == "48")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else
                    {
                        this.TScore = "0";
                    }


                    //Depression % ile
                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, DepressionTScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.DepressionTScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    if (this.DepressionTScore == "20")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "21")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "22")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "23")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "24")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "25")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "26")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "27")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "28")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "29")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "30")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "31")
                    {
                        this.Percentile = "3";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "32")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "33")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "34")
                    {
                        this.Percentile = "5";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "35")
                    {
                        this.Percentile = "7";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "36")
                    {
                        this.Percentile = "8";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "37")
                    {
                        this.Percentile = "10";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "38")
                    {
                        this.Percentile = "12";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "39")
                    {
                        this.Percentile = "14";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "40")
                    {
                        this.Percentile = "16";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "41")
                    {
                        this.Percentile = "18";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "42")
                    {
                        this.Percentile = "21";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "43")
                    {
                        this.Percentile = "24";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "44")
                    {
                        this.Percentile = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "45")
                    {
                        this.Percentile = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "46")
                    {
                        this.Percentile = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "47")
                    {
                        this.Percentile = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "48")
                    {
                        this.Percentile = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "49")
                    {
                        this.Percentile = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "50")
                    {
                        this.Percentile = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "51")
                    {
                        this.Percentile = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "52")
                    {
                        this.Percentile = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "53")
                    {
                        this.Percentile = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "54")
                    {
                        this.Percentile = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "55")
                    {
                        this.Percentile = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "56")
                    {
                        this.Percentile = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "57")
                    {
                        this.Percentile = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "58")
                    {
                        this.Percentile = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "59")
                    {
                        this.Percentile = "82";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "60")
                    {
                        this.Percentile = "84";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "61")
                    {
                        this.Percentile = "86";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "62")
                    {
                        this.Percentile = "88";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "63")
                    {
                        this.Percentile = "90";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "64")
                    {
                        this.Percentile = "92";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "65")
                    {
                        this.Percentile = "93";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "66")
                    {
                        this.Percentile = "95";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "67")
                    {
                        this.Percentile = "96";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "68")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "69")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "70")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "71")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "72")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "73")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "74")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.DepressionTScore == "75")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.DepressionTScore == "76")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.DepressionTScore == "77")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.DepressionTScore == "78")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.DepressionTScore == "79")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.DepressionTScore == "80")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }




                    //=====================================================================================================================================================================//
                    //Suicidal Ideation

                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTest.StudentIDNumber, SUM(Test6 +  Test15 +  Test24 +  Test33 + Test42 + Test51 +
                                Test60 + Test69 + Test78 + Test87 + Test96 + Test105)
                                FROM PsychologicalTest
                                WHERE PsychologicalTest.StudentIDNumber = '"
                                    + IDNumber + "' GROUP BY PsychologicalTest.StudentIDNumber;", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.SuicidalIdeationRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    //Update Suicidal Ideation Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                            SET SuicidalIDeationRowScore = '" + this.SuicidalIdeationRawScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                + IDNumber + "';", con);
                    cmd.ExecuteNonQuery();

                    con.Close();


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, SuicidalIDeationRowScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.SuicidalIdeationRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    if (this.SuicidalIdeationRawScore == "0" || this.SuicidalIdeationRawScore == "1" || this.SuicidalIdeationRawScore == "2" || this.SuicidalIdeationRawScore == "3" || this.SuicidalIdeationRawScore == "4" ||
                        this.SuicidalIdeationRawScore == "5" || this.SuicidalIdeationRawScore == "6" || this.SuicidalIdeationRawScore == "7" || this.SuicidalIdeationRawScore == "8" || this.SuicidalIdeationRawScore == "9" ||
                        this.SuicidalIdeationRawScore == "10" || this.SuicidalIdeationRawScore == "11")
                    {
                        this.TScore = "0";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "12")
                    {
                        this.TScore = "44";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    else if (this.SuicidalIdeationRawScore == "13")
                    {
                        this.TScore = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "14")
                    {
                        this.TScore = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "15")
                    {
                        this.TScore = "56";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "16")
                    {
                        this.TScore = "59";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "17")
                    {
                        this.TScore = "60";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "18")
                    {
                        this.TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "19")
                    {
                        this.TScore = "63";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "20")
                    {
                        this.TScore = "64";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "21")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "22")
                    {
                        this.TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "23")
                    {
                        this.TScore = "67";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "24")
                    {
                        this.TScore = "68";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "25")
                    {
                        this.TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "26")
                    {
                        this.TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "27")
                    {
                        this.TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "28")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "29")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "30")
                    {
                        this.TScore = "72";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "31")
                    {
                        this.TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "32")
                    {
                        this.TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "33")
                    {
                        this.TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "34")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "35")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "36")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "37")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "38")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "39")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "40")
                    {
                        this.TScore = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "41")
                    {
                        this.TScore = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "42")
                    {
                        this.TScore = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "43")
                    {
                        this.TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "44")
                    {
                        this.TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "45")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "46")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "47")
                    {
                        this.TScore = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationRawScore == "48")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else
                    {
                        this.TScore = "0";
                    }


                    //Get Suicidal Ideation % ile
                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, SuicidalIDeationTScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.SuicidalIdeationTScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    if (this.SuicidalIdeationTScore == "20")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "21")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "22")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "23")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "24")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "25")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "26")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "27")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "28")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "29")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "30")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "31")
                    {
                        this.Percentile = "3";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "32")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "33")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "34")
                    {
                        this.Percentile = "5";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "35")
                    {
                        this.Percentile = "7";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "36")
                    {
                        this.Percentile = "8";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "37")
                    {
                        this.Percentile = "10";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "38")
                    {
                        this.Percentile = "12";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "39")
                    {
                        this.Percentile = "14";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "40")
                    {
                        this.Percentile = "16";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "41")
                    {
                        this.Percentile = "18";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "42")
                    {
                        this.Percentile = "21";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "43")
                    {
                        this.Percentile = "24";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "44")
                    {
                        this.Percentile = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "45")
                    {
                        this.Percentile = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "46")
                    {
                        this.Percentile = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "47")
                    {
                        this.Percentile = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "48")
                    {
                        this.Percentile = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "49")
                    {
                        this.Percentile = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "50")
                    {
                        this.Percentile = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "51")
                    {
                        this.Percentile = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "52")
                    {
                        this.Percentile = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "53")
                    {
                        this.Percentile = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "54")
                    {
                        this.Percentile = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "55")
                    {
                        this.Percentile = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "56")
                    {
                        this.Percentile = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "57")
                    {
                        this.Percentile = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "58")
                    {
                        this.Percentile = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "59")
                    {
                        this.Percentile = "82";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "60")
                    {
                        this.Percentile = "84";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "61")
                    {
                        this.Percentile = "86";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "62")
                    {
                        this.Percentile = "88";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "63")
                    {
                        this.Percentile = "90";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "64")
                    {
                        this.Percentile = "92";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "65")
                    {
                        this.Percentile = "93";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "66")
                    {
                        this.Percentile = "95";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "67")
                    {
                        this.Percentile = "96";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "68")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "69")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "70")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "71")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "72")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "73")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "74")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SuicidalIdeationTScore == "75")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SuicidalIdeationTScore == "76")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SuicidalIdeationTScore == "77")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SuicidalIdeationTScore == "78")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SuicidalIdeationTScore == "79")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SuicidalIdeationTScore == "80")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SuicidalIDeationPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }





                    //============================================================================================================================================================//


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTest.StudentIDNumber, SUM(Test7 +  Test16 +  Test25 +  Test34 + Test43 + Test52 +
                                Test61 + Test70 + Test79 + Test88 + Test97 + Test106)
                                FROM PsychologicalTest
                                WHERE PsychologicalTest.StudentIDNumber = '"
                                    + IDNumber + "' GROUP BY PsychologicalTest.StudentIDNumber;", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.SubstanceAbuseRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    //Update Substance Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                            SET SubstanceAbuseRowScore = '" + this.SubstanceAbuseRawScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                + IDNumber + "';", con);
                    cmd.ExecuteNonQuery();

                    con.Close();


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, SubstanceAbuseRowScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.SubstanceAbuseRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    if (this.SubstanceAbuseRawScore == "0" || this.SubstanceAbuseRawScore == "1" || this.SubstanceAbuseRawScore == "2" || this.SubstanceAbuseRawScore == "3" || this.SubstanceAbuseRawScore == "4" ||
                        this.SubstanceAbuseRawScore == "5" || this.SubstanceAbuseRawScore == "6" || this.SubstanceAbuseRawScore == "7" || this.SubstanceAbuseRawScore == "8" || this.SubstanceAbuseRawScore == "9" ||
                        this.SubstanceAbuseRawScore == "10")
                    {
                        this.TScore = "0";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "11")
                    {
                        this.TScore = "39";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "12")
                    {
                        this.TScore = "39";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    else if (this.SubstanceAbuseRawScore == "13")
                    {
                        this.TScore = "45";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "14")
                    {
                        this.TScore = "48";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "15")
                    {
                        this.TScore = "51";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "16")
                    {
                        this.TScore = "53";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "17")
                    {
                        this.TScore = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "18")
                    {
                        this.TScore = "56";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "19")
                    {
                        this.TScore = "57";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "20")
                    {
                        this.TScore = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "21")
                    {
                        this.TScore = "59";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "22")
                    {
                        this.TScore = "60";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "23")
                    {
                        this.TScore = "61";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "24")
                    {
                        this.TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "25")
                    {
                        this.TScore = "63";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "26")
                    {
                        this.TScore = "64";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "27")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "28")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "29")
                    {
                        this.TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "30")
                    {
                        this.TScore = "67";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "31")
                    {
                        this.TScore = "68";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "32")
                    {
                        this.TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "33")
                    {
                        this.TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "34")
                    {
                        this.TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "35")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "36")
                    {
                        this.TScore = "72";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "37")
                    {
                        this.TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "38")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "39")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "40")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "41")
                    {
                        this.TScore = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "42")
                    {
                        this.TScore = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "43")
                    {
                        this.TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "44")
                    {
                        this.TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "45")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "46")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "47")
                    {
                        this.TScore = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseRawScore == "48")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbuseTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else
                    {
                        this.TScore = "0";
                    }


                    //Substance Abuse % ile
                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, SubstanceAbuseTScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.SubstanceAbuseTScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    if (this.SubstanceAbuseTScore == "20")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "21")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "22")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "23")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "24")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "25")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "26")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "27")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "28")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "29")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "30")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "31")
                    {
                        this.Percentile = "3";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "32")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "33")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "34")
                    {
                        this.Percentile = "5";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "35")
                    {
                        this.Percentile = "7";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "36")
                    {
                        this.Percentile = "8";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "37")
                    {
                        this.Percentile = "10";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "38")
                    {
                        this.Percentile = "12";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "39")
                    {
                        this.Percentile = "14";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "40")
                    {
                        this.Percentile = "16";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "41")
                    {
                        this.Percentile = "18";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "42")
                    {
                        this.Percentile = "21";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "43")
                    {
                        this.Percentile = "24";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "44")
                    {
                        this.Percentile = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "45")
                    {
                        this.Percentile = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "46")
                    {
                        this.Percentile = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "47")
                    {
                        this.Percentile = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "48")
                    {
                        this.Percentile = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "49")
                    {
                        this.Percentile = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "50")
                    {
                        this.Percentile = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "51")
                    {
                        this.Percentile = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "52")
                    {
                        this.Percentile = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "53")
                    {
                        this.Percentile = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "54")
                    {
                        this.Percentile = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "55")
                    {
                        this.Percentile = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "56")
                    {
                        this.Percentile = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "57")
                    {
                        this.Percentile = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "58")
                    {
                        this.Percentile = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "59")
                    {
                        this.Percentile = "82";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "60")
                    {
                        this.Percentile = "84";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "61")
                    {
                        this.Percentile = "86";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "62")
                    {
                        this.Percentile = "88";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "63")
                    {
                        this.Percentile = "90";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "64")
                    {
                        this.Percentile = "92";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "65")
                    {
                        this.Percentile = "93";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "66")
                    {
                        this.Percentile = "95";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "67")
                    {
                        this.Percentile = "96";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "68")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "69")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "70")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "71")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "72")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "73")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "74")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SubstanceAbuseTScore == "75")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SubstanceAbuseTScore == "76")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SubstanceAbuseTScore == "77")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SubstanceAbuseTScore == "78")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SubstanceAbuseTScore == "79")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SubstanceAbuseTScore == "80")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET SubstanceAbusePercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }




                    //=======================================================================================================================================================//


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTest.StudentIDNumber, SUM(Test8 +  Test17 +  Test26 +  Test35 + Test44 + Test53 +
                                Test62 + Test71 + Test80 + Test89 + Test98 + Test107)
                                FROM PsychologicalTest
                                WHERE PsychologicalTest.StudentIDNumber = '"
                                    + IDNumber + "' GROUP BY PsychologicalTest.StudentIDNumber;", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.SelfEsteemProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    //Update Self-Esteem Problem Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                            SET Self_EsteemProblemRowScore = '" + this.SelfEsteemProblemRawScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                + IDNumber + "';", con);
                    cmd.ExecuteNonQuery();

                    con.Close();


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, Self_EsteemProblemRowScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.SelfEsteemProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    if (this.SelfEsteemProblemRawScore == "0" || this.SelfEsteemProblemRawScore == "1" || this.SelfEsteemProblemRawScore == "2" || this.SelfEsteemProblemRawScore == "3" || this.SelfEsteemProblemRawScore == "4" ||
                        this.SelfEsteemProblemRawScore == "5" || this.SelfEsteemProblemRawScore == "6" || this.SelfEsteemProblemRawScore == "7" || this.SelfEsteemProblemRawScore == "8" || this.SelfEsteemProblemRawScore == "9" ||
                        this.SelfEsteemProblemRawScore == "10" || this.SelfEsteemProblemRawScore == "11")
                    {
                        this.TScore = "0";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "12")
                    {
                        this.TScore = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    else if (this.SelfEsteemProblemRawScore == "13")
                    {
                        this.TScore = "30";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "14")
                    {
                        this.TScore = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "15")
                    {
                        this.TScore = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "16")
                    {
                        this.TScore = "40";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "17")
                    {
                        this.TScore = "43";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "18")
                    {
                        this.TScore = "45";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "19")
                    {
                        this.TScore = "47";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "20")
                    {
                        this.TScore = "48";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "21")
                    {
                        this.TScore = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "22")
                    {
                        this.TScore = "51";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "23")
                    {
                        this.TScore = "53";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "24")
                    {
                        this.TScore = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "25")
                    {
                        this.TScore = "55";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "26")
                    {
                        this.TScore = "57";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "27")
                    {
                        this.TScore = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "28")
                    {
                        this.TScore = "59";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "29")
                    {
                        this.TScore = "60";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "30")
                    {
                        this.TScore = "61";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "31")
                    {
                        this.TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "32")
                    {
                        this.TScore = "63";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "33")
                    {
                        this.TScore = "64";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "34")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "35")
                    {
                        this.TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "36")
                    {
                        this.TScore = "67";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "37")
                    {
                        this.TScore = "68";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "38")
                    {
                        this.TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "39")
                    {
                        this.TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "40")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "41")
                    {
                        this.TScore = "72";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "42")
                    {
                        this.TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "43")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "44")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "45")
                    {
                        this.TScore = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "46")
                    {
                        this.TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "47")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemRawScore == "48")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else
                    {
                        this.TScore = "0";
                    }


                    //Self-Esteem Problem % ile
                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, Self_EsteemProblemTScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.SelfEsteemProblemTScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    if (this.SelfEsteemProblemTScore == "20")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "21")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "22")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "23")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "24")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "25")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "26")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "27")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "28")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "29")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "30")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "31")
                    {
                        this.Percentile = "3";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "32")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "33")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "34")
                    {
                        this.Percentile = "5";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "35")
                    {
                        this.Percentile = "7";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "36")
                    {
                        this.Percentile = "8";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "37")
                    {
                        this.Percentile = "10";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "38")
                    {
                        this.Percentile = "12";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "39")
                    {
                        this.Percentile = "14";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "40")
                    {
                        this.Percentile = "16";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "41")
                    {
                        this.Percentile = "18";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "42")
                    {
                        this.Percentile = "21";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "43")
                    {
                        this.Percentile = "24";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "44")
                    {
                        this.Percentile = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "45")
                    {
                        this.Percentile = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "46")
                    {
                        this.Percentile = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "47")
                    {
                        this.Percentile = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "48")
                    {
                        this.Percentile = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "49")
                    {
                        this.Percentile = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "50")
                    {
                        this.Percentile = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "51")
                    {
                        this.Percentile = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "52")
                    {
                        this.Percentile = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "53")
                    {
                        this.Percentile = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "54")
                    {
                        this.Percentile = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "55")
                    {
                        this.Percentile = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "56")
                    {
                        this.Percentile = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "57")
                    {
                        this.Percentile = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "58")
                    {
                        this.Percentile = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "59")
                    {
                        this.Percentile = "82";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "60")
                    {
                        this.Percentile = "84";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "61")
                    {
                        this.Percentile = "86";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "62")
                    {
                        this.Percentile = "88";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "63")
                    {
                        this.Percentile = "90";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "64")
                    {
                        this.Percentile = "92";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "65")
                    {
                        this.Percentile = "93";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "66")
                    {
                        this.Percentile = "95";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "67")
                    {
                        this.Percentile = "96";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "68")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "69")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "70")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "71")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "72")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "73")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "74")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.SelfEsteemProblemTScore == "75")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SelfEsteemProblemTScore == "76")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SelfEsteemProblemTScore == "77")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SelfEsteemProblemTScore == "78")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SelfEsteemProblemTScore == "79")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.SelfEsteemProblemTScore == "80")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET Self_EsteemProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }




                    //================================================================================================================================================================================//


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTest.StudentIDNumber, SUM(Test3 +  Test12 +  Test21 +  Test30 + Test39 + Test48 +
                                Test57 + Test66 + Test75 + Test84 + Test93 + Test102)
                                FROM PsychologicalTest
                                WHERE PsychologicalTest.StudentIDNumber = '"
                                    + IDNumber + "' GROUP BY PsychologicalTest.StudentIDNumber;", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.InterpersonalProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    //Update Interpersonal Problem Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                            SET InterpersonalProblemRowScore = '" + this.InterpersonalProblemRawScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                + IDNumber + "';", con);
                    cmd.ExecuteNonQuery();

                    con.Close();


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, InterpersonalProblemRowScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.InterpersonalProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();



                    if (this.InterpersonalProblemRawScore == "0" || this.InterpersonalProblemRawScore == "1" || this.InterpersonalProblemRawScore == "2" || this.InterpersonalProblemRawScore == "3" || this.InterpersonalProblemRawScore == "4" ||
                        this.InterpersonalProblemRawScore == "5" || this.InterpersonalProblemRawScore == "6" || this.InterpersonalProblemRawScore == "7" || this.InterpersonalProblemRawScore == "8" || this.InterpersonalProblemRawScore == "9" ||
                        this.InterpersonalProblemRawScore == "10" || this.InterpersonalProblemRawScore == "11")
                    {
                        this.TScore = "0";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "12")
                    {
                        this.TScore = "29";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    else if (this.InterpersonalProblemRawScore == "13")
                    {
                        this.TScore = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "14")
                    {
                        this.TScore = "37";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "15")
                    {
                        this.TScore = "40";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "16")
                    {
                        this.TScore = "43";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "17")
                    {
                        this.TScore = "45";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "18")
                    {
                        this.TScore = "47";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "19")
                    {
                        this.TScore = "48";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "20")
                    {
                        this.TScore = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "21")
                    {
                        this.TScore = "52";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "22")
                    {
                        this.TScore = "53";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "23")
                    {
                        this.TScore = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "24")
                    {
                        this.TScore = "56";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "25")
                    {
                        this.TScore = "57";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "26")
                    {
                        this.TScore = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "27")
                    {
                        this.TScore = "59";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "28")
                    {
                        this.TScore = "60";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "29")
                    {
                        this.TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "30")
                    {
                        this.TScore = "63";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "31")
                    {
                        this.TScore = "64";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "32")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "33")
                    {
                        this.TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "34")
                    {
                        this.TScore = "67";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "35")
                    {
                        this.TScore = "68";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "36")
                    {
                        this.TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "37")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "38")
                    {
                        this.TScore = "72";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "39")
                    {
                        this.TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "40")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "41")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "42")
                    {
                        this.TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "43")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "44")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "45")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "46")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "47")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemRawScore == "48")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else
                    {
                        this.TScore = "0";
                    }


                    //Interpersonal Problem % ile
                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, InterpersonalProblemTScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.InterpersonalProblemTScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    if (this.InterpersonalProblemTScore == "20")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "21")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "22")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "23")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "24")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "25")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "26")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "27")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "28")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "29")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "30")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "31")
                    {
                        this.Percentile = "3";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "32")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "33")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "34")
                    {
                        this.Percentile = "5";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "35")
                    {
                        this.Percentile = "7";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "36")
                    {
                        this.Percentile = "8";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "37")
                    {
                        this.Percentile = "10";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "38")
                    {
                        this.Percentile = "12";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "39")
                    {
                        this.Percentile = "14";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "40")
                    {
                        this.Percentile = "16";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "41")
                    {
                        this.Percentile = "18";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "42")
                    {
                        this.Percentile = "21";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "43")
                    {
                        this.Percentile = "24";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "44")
                    {
                        this.Percentile = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "45")
                    {
                        this.Percentile = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "46")
                    {
                        this.Percentile = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "47")
                    {
                        this.Percentile = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "48")
                    {
                        this.Percentile = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "49")
                    {
                        this.Percentile = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "50")
                    {
                        this.Percentile = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "51")
                    {
                        this.Percentile = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "52")
                    {
                        this.Percentile = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "53")
                    {
                        this.Percentile = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "54")
                    {
                        this.Percentile = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "55")
                    {
                        this.Percentile = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "56")
                    {
                        this.Percentile = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "57")
                    {
                        this.Percentile = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "58")
                    {
                        this.Percentile = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "59")
                    {
                        this.Percentile = "82";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "60")
                    {
                        this.Percentile = "84";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "61")
                    {
                        this.Percentile = "86";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "62")
                    {
                        this.Percentile = "88";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "63")
                    {
                        this.Percentile = "90";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "64")
                    {
                        this.Percentile = "92";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "65")
                    {
                        this.Percentile = "93";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "66")
                    {
                        this.Percentile = "95";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "67")
                    {
                        this.Percentile = "96";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "68")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "69")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "70")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "71")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "72")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "73")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "74")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.InterpersonalProblemTScore == "75")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.InterpersonalProblemTScore == "76")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.InterpersonalProblemTScore == "77")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.InterpersonalProblemTScore == "78")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.InterpersonalProblemTScore == "79")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.InterpersonalProblemTScore == "80")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET InterpersonalProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }



                    //===============================================================================================================================================================//


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTest.StudentIDNumber, SUM(Test9 +  Test18 +  Test27 +  Test36 + Test45 + Test54 +
                                Test63 + Test72 + Test81 + Test90 + Test99 + Test108)
                                FROM PsychologicalTest
                                WHERE PsychologicalTest.StudentIDNumber = '"
                                    + IDNumber + "' GROUP BY PsychologicalTest.StudentIDNumber;", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.FamilyProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    //Update Family Problem Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                            SET FamilyProblemRowScore = '" + this.FamilyProblemRawScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                + IDNumber + "';", con);
                    cmd.ExecuteNonQuery();

                    con.Close();


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, FamilyProblemRowScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.FamilyProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    if (this.FamilyProblemRawScore == "0" || this.FamilyProblemRawScore == "1" || this.FamilyProblemRawScore == "2" || this.FamilyProblemRawScore == "3" || this.FamilyProblemRawScore == "4" ||
                        this.FamilyProblemRawScore == "5" || this.FamilyProblemRawScore == "6" || this.FamilyProblemRawScore == "7" || this.FamilyProblemRawScore == "8" || this.FamilyProblemRawScore == "9" ||
                        this.FamilyProblemRawScore == "10" || this.FamilyProblemRawScore == "11")
                    {
                        this.TScore = "0";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "12")
                    {
                        this.TScore = "32";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    else if (this.FamilyProblemRawScore == "13")
                    {
                        this.TScore = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "14")
                    {
                        this.TScore = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "15")
                    {
                        this.TScore = "45";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "16")
                    {
                        this.TScore = "47";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "17")
                    {
                        this.TScore = "49";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "18")
                    {
                        this.TScore = "51";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "19")
                    {
                        this.TScore = "52";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "20")
                    {
                        this.TScore = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "21")
                    {
                        this.TScore = "55";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "22")
                    {
                        this.TScore = "56";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "23")
                    {
                        this.TScore = "57";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "24")
                    {
                        this.TScore = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "25")
                    {
                        this.TScore = "59";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "26")
                    {
                        this.TScore = "60";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "27")
                    {
                        this.TScore = "61";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "28")
                    {
                        this.TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "29")
                    {
                        this.TScore = "63";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "30")
                    {
                        this.TScore = "64";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "31")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "32")
                    {
                        this.TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "33")
                    {
                        this.TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "34")
                    {
                        this.TScore = "67";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "35")
                    {
                        this.TScore = "68";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "36")
                    {
                        this.TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "37")
                    {
                        this.TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "38")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "39")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "40")
                    {
                        this.TScore = "72";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "41")
                    {
                        this.TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "42")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "43")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "44")
                    {
                        this.TScore = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "45")
                    {
                        this.TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "46")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "47")
                    {
                        this.TScore = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemRawScore == "48")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else
                    {
                        this.TScore = "0";
                    }


                    //Family Problem % ile
                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, FamilyProblemTScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.FamilyProblemTScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    if (this.FamilyProblemTScore == "20")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "21")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "22")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "23")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "24")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "25")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "26")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "27")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "28")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "29")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "30")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "31")
                    {
                        this.Percentile = "3";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "32")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "33")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "34")
                    {
                        this.Percentile = "5";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "35")
                    {
                        this.Percentile = "7";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "36")
                    {
                        this.Percentile = "8";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "37")
                    {
                        this.Percentile = "10";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "38")
                    {
                        this.Percentile = "12";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "39")
                    {
                        this.Percentile = "14";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "40")
                    {
                        this.Percentile = "16";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "41")
                    {
                        this.Percentile = "18";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "42")
                    {
                        this.Percentile = "21";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "43")
                    {
                        this.Percentile = "24";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "44")
                    {
                        this.Percentile = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "45")
                    {
                        this.Percentile = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "46")
                    {
                        this.Percentile = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "47")
                    {
                        this.Percentile = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "48")
                    {
                        this.Percentile = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "49")
                    {
                        this.Percentile = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "50")
                    {
                        this.Percentile = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "51")
                    {
                        this.Percentile = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "52")
                    {
                        this.Percentile = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "53")
                    {
                        this.Percentile = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "54")
                    {
                        this.Percentile = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "55")
                    {
                        this.Percentile = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "56")
                    {
                        this.Percentile = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "57")
                    {
                        this.Percentile = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "58")
                    {
                        this.Percentile = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                         + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "59")
                    {
                        this.Percentile = "82";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "60")
                    {
                        this.Percentile = "84";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "61")
                    {
                        this.Percentile = "86";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "62")
                    {
                        this.Percentile = "88";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "63")
                    {
                        this.Percentile = "90";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "64")
                    {
                        this.Percentile = "92";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "65")
                    {
                        this.Percentile = "93";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "66")
                    {
                        this.Percentile = "95";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "67")
                    {
                        this.Percentile = "96";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "68")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "69")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "70")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "71")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "72")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "73")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "74")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.FamilyProblemTScore == "75")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.FamilyProblemTScore == "76")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.FamilyProblemTScore == "77")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.FamilyProblemTScore == "78")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.FamilyProblemTScore == "79")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.FamilyProblemTScore == "80")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET FamilyProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }




                    //==============================================================================================================================================================================//


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTest.StudentIDNumber, SUM(Test1 +  Test10 +  Test19 +  Test28 + Test37 + Test46 +
                                Test55 + Test64 + Test73 + Test82 + Test91 + Test100)
                                FROM PsychologicalTest
                                WHERE PsychologicalTest.StudentIDNumber = '"
                                    + IDNumber + "' GROUP BY PsychologicalTest.StudentIDNumber;", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.AcademicProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    //Update Academic Problem Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                            SET AcademicProblemRowScore = '" + this.AcademicProblemRawScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                + IDNumber + "';", con);
                    cmd.ExecuteNonQuery();

                    con.Close();


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, AcademicProblemRowScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.AcademicProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    //Academic Problem T Score
                    if (this.AcademicProblemRawScore == "0" || this.AcademicProblemRawScore == "1" || this.AcademicProblemRawScore == "2" || this.AcademicProblemRawScore == "3" || this.AcademicProblemRawScore == "4" ||
                        this.AcademicProblemRawScore == "5" || this.AcademicProblemRawScore == "6" || this.AcademicProblemRawScore == "7" || this.AcademicProblemRawScore == "8" || this.AcademicProblemRawScore == "9" ||
                        this.AcademicProblemRawScore == "10" || this.AcademicProblemRawScore == "11")
                    {
                        this.TScore = "0";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "12")
                    {
                        this.TScore = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    else if (this.AcademicProblemRawScore == "13")
                    {
                        this.TScore = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "14")
                    {
                        this.TScore = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "15")
                    {
                        this.TScore = "36";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "16")
                    {
                        this.TScore = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "17")
                    {
                        this.TScore = "40";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "18")
                    {
                        this.TScore = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "19")
                    {
                        this.TScore = "44";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "20")
                    {
                        this.TScore = "45";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "21")
                    {
                        this.TScore = "47";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "22")
                    {
                        this.TScore = "48";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "23")
                    {
                        this.TScore = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "24")
                    {
                        this.TScore = "51";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "25")
                    {
                        this.TScore = "52";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "26")
                    {
                        this.TScore = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "27")
                    {
                        this.TScore = "55";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "28")
                    {
                        this.TScore = "56";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "29")
                    {
                        this.TScore = "57";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "30")
                    {
                        this.TScore = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "31")
                    {
                        this.TScore = "60";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "32")
                    {
                        this.TScore = "61";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "33")
                    {
                        this.TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "34")
                    {
                        this.TScore = "63";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "35")
                    {
                        this.TScore = "64";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "36")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "37")
                    {
                        this.TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "38")
                    {
                        this.TScore = "67";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "39")
                    {
                        this.TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "40")
                    {
                        this.TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "41")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "42")
                    {
                        this.TScore = "72";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "43")
                    {
                        this.TScore = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "44")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "45")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "46")
                    {
                        this.TScore = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "47")
                    {
                        this.TScore = "77";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemRawScore == "48")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else
                    {
                        this.TScore = "0";
                    }


                    //Academic Problem % ile
                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, AcademicProblemTScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.AcademicProblemTScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    if (this.AcademicProblemTScore == "20")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "21")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "22")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "23")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "24")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "25")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "26")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "27")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "28")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "29")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "30")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "31")
                    {
                        this.Percentile = "3";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "32")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "33")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "34")
                    {
                        this.Percentile = "5";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "35")
                    {
                        this.Percentile = "7";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "36")
                    {
                        this.Percentile = "8";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "37")
                    {
                        this.Percentile = "10";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "38")
                    {
                        this.Percentile = "12";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "39")
                    {
                        this.Percentile = "14";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "40")
                    {
                        this.Percentile = "16";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "41")
                    {
                        this.Percentile = "18";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "42")
                    {
                        this.Percentile = "21";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "43")
                    {
                        this.Percentile = "24";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "44")
                    {
                        this.Percentile = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "45")
                    {
                        this.Percentile = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "46")
                    {
                        this.Percentile = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "47")
                    {
                        this.Percentile = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "48")
                    {
                        this.Percentile = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "49")
                    {
                        this.Percentile = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "50")
                    {
                        this.Percentile = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "51")
                    {
                        this.Percentile = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "52")
                    {
                        this.Percentile = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "53")
                    {
                        this.Percentile = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "54")
                    {
                        this.Percentile = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "55")
                    {
                        this.Percentile = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "56")
                    {
                        this.Percentile = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "57")
                    {
                        this.Percentile = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "58")
                    {
                        this.Percentile = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "59")
                    {
                        this.Percentile = "82";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "60")
                    {
                        this.Percentile = "84";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "61")
                    {
                        this.Percentile = "86";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "62")
                    {
                        this.Percentile = "88";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "63")
                    {
                        this.Percentile = "90";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "64")
                    {
                        this.Percentile = "92";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "65")
                    {
                        this.Percentile = "93";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "66")
                    {
                        this.Percentile = "95";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "67")
                    {
                        this.Percentile = "96";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "68")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "69")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "70")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "71")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "72")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "73")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "74")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.AcademicProblemTScore == "75")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AcademicProblemTScore == "76")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AcademicProblemTScore == "77")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AcademicProblemTScore == "78")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AcademicProblemTScore == "79")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.AcademicProblemTScore == "80")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET AcademicProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                        + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }


                    //======================================================================================================================================================================//

                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTest.StudentIDNumber, SUM(Test5 +  Test14 +  Test23 +  Test32 + Test41 + Test50 +
                                Test59 + Test68 + Test77 + Test86 + Test95 + Test104)
                                FROM PsychologicalTest
                                WHERE PsychologicalTest.StudentIDNumber = '"
                                   + IDNumber + "' GROUP BY PsychologicalTest.StudentIDNumber;", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.CareerProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    //Career Problem Raw Score
                    con.Open();

                    cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                            SET CareerProblemRowScore = '" + this.CareerProblemRawScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                               + IDNumber + "';", con);
                    cmd.ExecuteNonQuery();

                    con.Close();


                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, CareerProblemRowScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.CareerProblemRawScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();


                    //Career Problem T Score
                    if (this.CareerProblemRawScore == "0" || this.CareerProblemRawScore == "1" || this.CareerProblemRawScore == "2" || this.CareerProblemRawScore == "3" || this.CareerProblemRawScore == "4" ||
                        this.CareerProblemRawScore == "5" || this.CareerProblemRawScore == "6" || this.CareerProblemRawScore == "7" || this.CareerProblemRawScore == "8" || this.CareerProblemRawScore == "9" ||
                        this.CareerProblemRawScore == "10" || this.CareerProblemRawScore == "11")
                    {
                        this.TScore = "0";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "12")
                    {
                        this.TScore = "36";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    else if (this.CareerProblemRawScore == "13")
                    {
                        this.TScore = "41";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "14")
                    {
                        this.TScore = "44";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "15")
                    {
                        this.TScore = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "16")
                    {
                        this.TScore = "48";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "17")
                    {
                        this.TScore = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "18")
                    {
                        this.TScore = "51";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "19")
                    {
                        this.TScore = "52";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "20")
                    {
                        this.TScore = "53";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "21")
                    {
                        this.TScore = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "22")
                    {
                        this.TScore = "55";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "23")
                    {
                        this.TScore = "56";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "24")
                    {
                        this.TScore = "57";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "25")
                    {
                        this.TScore = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "26")
                    {
                        this.TScore = "59";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "27")
                    {
                        this.TScore = "59";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "28")
                    {
                        this.TScore = "60";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "29")
                    {
                        this.TScore = "61";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "30")
                    {
                        this.TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "31")
                    {
                        this.TScore = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "32")
                    {
                        this.TScore = "63";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "33")
                    {
                        this.TScore = "64";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "34")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "35")
                    {
                        this.TScore = "65";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "36")
                    {
                        this.TScore = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "37")
                    {
                        this.TScore = "67";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "38")
                    {
                        this.TScore = "68";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "39")
                    {
                        this.TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "40")
                    {
                        this.TScore = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "41")
                    {
                        this.TScore = "70";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "42")
                    {
                        this.TScore = "71";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "43")
                    {
                        this.TScore = "72";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "44")
                    {
                        this.TScore = "74";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "45")
                    {
                        this.TScore = "75";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "46")
                    {
                        this.TScore = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "47")
                    {
                        this.TScore = "78";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemRawScore == "48")
                    {
                        this.TScore = "80";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemTScore = '" + this.TScore + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else
                    {
                        this.TScore = "0";
                    }


                    //Depression % ile
                    con.Open();

                    cmd = new SqlCommand(@"SELECT PsychologicalTestInterpretation.StudentIDNumber, CareerProblemTScore
                            FROM PsychologicalTestInterpretation
                            WHERE PsychologicalTestInterpretation.StudentIDNumber = '" + IDNumber + "';", con);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        this.StudentIDNumber = rdr.GetValue(0).ToString();
                        this.CareerProblemTScore = rdr.GetValue(1).ToString();
                    }

                    con.Close();

                    if (this.CareerProblemTScore == "20")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "21")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "22")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "23")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "24")
                    {
                        this.Percentile = "-1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "25")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "26")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "27")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "28")
                    {
                        this.Percentile = "1";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "29")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "30")
                    {
                        this.Percentile = "2";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "31")
                    {
                        this.Percentile = "3";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "32")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "33")
                    {
                        this.Percentile = "4";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "34")
                    {
                        this.Percentile = "5";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "35")
                    {
                        this.Percentile = "7";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "36")
                    {
                        this.Percentile = "8";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "37")
                    {
                        this.Percentile = "10";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "38")
                    {
                        this.Percentile = "12";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "39")
                    {
                        this.Percentile = "14";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "40")
                    {
                        this.Percentile = "16";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "41")
                    {
                        this.Percentile = "18";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "42")
                    {
                        this.Percentile = "21";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "43")
                    {
                        this.Percentile = "24";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "44")
                    {
                        this.Percentile = "28";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "45")
                    {
                        this.Percentile = "31";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "46")
                    {
                        this.Percentile = "34";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "47")
                    {
                        this.Percentile = "38";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "48")
                    {
                        this.Percentile = "42";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "49")
                    {
                        this.Percentile = "46";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "50")
                    {
                        this.Percentile = "50";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "51")
                    {
                        this.Percentile = "54";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "52")
                    {
                        this.Percentile = "58";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "53")
                    {
                        this.Percentile = "62";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "54")
                    {
                        this.Percentile = "66";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "55")
                    {
                        this.Percentile = "69";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "56")
                    {
                        this.Percentile = "73";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "57")
                    {
                        this.Percentile = "76";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "58")
                    {
                        this.Percentile = "79";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "59")
                    {
                        this.Percentile = "82";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "60")
                    {
                        this.Percentile = "84";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "61")
                    {
                        this.Percentile = "86";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "62")
                    {
                        this.Percentile = "88";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "63")
                    {
                        this.Percentile = "90";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "64")
                    {
                        this.Percentile = "92";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "65")
                    {
                        this.Percentile = "93";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "66")
                    {
                        this.Percentile = "95";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "67")
                    {
                        this.Percentile = "96";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "68")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "69")
                    {
                        this.Percentile = "97";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "70")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "71")
                    {
                        this.Percentile = "98";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "72")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "73")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "74")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else if (this.CareerProblemTScore == "75")
                    {
                        this.Percentile = "99";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.CareerProblemTScore == "76")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.CareerProblemTScore == "77")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.CareerProblemTScore == "78")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.CareerProblemTScore == "79")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET CareerProblemPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    //Plus 1
                    else if (this.CareerProblemTScore == "80")
                    {
                        this.Percentile = "100";

                        con.Open();

                        cmd = new SqlCommand(@"UPDATE PsychologicalTestInterpretation
                                    SET DepressionPercentILE = '" + this.Percentile + "' WHERE PsychologicalTestInterpretation.StudentIDNumber = '"
                                       + IDNumber + "';", con);
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    else { }



                    //===========================================================================================================================================================//



                    con.Open();

                    cmd = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM PsychologicalTest WHERE PsychologicalTest.StudentIDNumber = '"
                                + IDNumber + "') BEGIN INSERT INTO PsychologicalTest (StudentIDNumber, PsychologicalTaken) VALUES ('"
                                + IDNumber + "','1') END BEGIN UPDATE PsychologicalTest SET StudentIDNumber = '"
                                + IDNumber + "', PsychologicalTaken = '1' WHERE PsychologicalTest.StudentIDNumber = '"
                                + IDNumber + "' END", con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                }

                catch (ArgumentOutOfRangeException aore)
                {
                    //MessageBox.Show(aore.Data.Values.Count.ToString());
                }
                catch (NullReferenceException nre)
                {
                    //MessageBox.Show(nre.Data.ToString());
                }


                //Feedback when transfer
                pBarTransferData.Value++;

                int percent = (int)(((double)(pBarTransferData.Value - pBarTransferData.Minimum) /
                (double)(pBarTransferData.Maximum - pBarTransferData.Minimum)) * 100);

                using (Graphics gr = pBarTransferData.CreateGraphics())
                {
                    gr.DrawString(percent.ToString() + "%",
                        SystemFonts.DefaultFont,
                        Brushes.Black,
                        new PointF(pBarTransferData.Width / 2 - (gr.MeasureString(percent.ToString() + "%",
                            SystemFonts.DefaultFont).Width / 2.0F),
                        pBarTransferData.Height / 2 - (gr.MeasureString(percent.ToString() + "%",
                            SystemFonts.DefaultFont).Height / 2.0F)));
                }
            }
        }


        private void btnTransfer_Click(object sender, EventArgs e)
        {
            this.pBarTransferData.Visible = true;

            this.pBarTransferData.Maximum = this.dataGridGetExcelData.Rows.Count;
            this.pBarTransferData.Value = pBarTransferData.Minimum;

            LoadPsychTest();
            GetIDByPsychologicalInterpretation();

            CalCulateAnxietyTScoreValue();



            MessageBox.Show("Data Successfully Load", "");

            this.pBarTransferData.Visible = false;
        }
    }
}
