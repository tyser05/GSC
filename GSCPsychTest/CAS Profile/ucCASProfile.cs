using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;

namespace GSCPsychTest.CAS_Profile
{
    public partial class ucCASProfile : UserControl
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr;
        ReportDocument rd = new ReportDocument();

        int ANrow, ANts, ANile, DProw, DPts, DPile, SIrow, SIts, SIile, SArow, SAts, SAile, SErow, SEts, SEile, IProw, IPts, IPile,
                FProw, FPts, FPile, AProw, APts, APile, CProw, CPts, CPile;


        private frmHome fSetHome;
        string sSetServer, sSetDBName, sSetIDNum;
        public ucCASProfile(string sGetServer, string sGetDBName, string sGetIDNum, frmHome fGetHome)
        {
            InitializeComponent();

            this.sSetServer = sGetServer;
            this.sSetDBName = sGetDBName;
            this.sSetIDNum = sGetIDNum;
            this.fSetHome = fGetHome;
            this.con = new SqlConnection(@"Data Source = " + sGetServer + "; Initial Catalog = " + sGetDBName + "; Integrated Security = true;");
        }

        private void ucCASProfile_Load(object sender, EventArgs e)
        {
            
        }

        public void CASProfileSummary(string sIDNum)
        {
            //string sPath = Path.GetFullPath("crCASProfile.rpt");
            //string sNewPath = sPath.Replace(@"bin\Debug", "Cumulative Record");
            //rd.Load(sNewPath);

           
            this.crViewerCASProf.Refresh();
            this.crViewerCASProf.RefreshReport();
            rd.Load(@"Report\crCASProfile.rpt");
            this.lblIDNum.Text = sIDNum;
            //Student
            TextObject toStudName = rd.ReportDefinition.ReportObjects["onStudName"] as TextObject;
            TextObject toCY = rd.ReportDefinition.ReportObjects["onCY"] as TextObject;
            toStudName.Text = this.lblName.Text;

            TextObject toDept = rd.ReportDefinition.ReportObjects["onDept"] as TextObject;
            toDept.Text = "N/A";

            //Counselor Name
            TextObject toCounselorName = rd.ReportDefinition.ReportObjects["onCouselorName"] as TextObject;
            toCounselorName.Text = "";

            //Raw Score
            TextObject toRSAN = rd.ReportDefinition.ReportObjects["onRSAN"] as TextObject;
            TextObject toRSDP = rd.ReportDefinition.ReportObjects["onRSDP"] as TextObject;
            TextObject toRSSI = rd.ReportDefinition.ReportObjects["onRSSI"] as TextObject;
            TextObject toRSSA = rd.ReportDefinition.ReportObjects["onRSSA"] as TextObject;
            TextObject toRSSE = rd.ReportDefinition.ReportObjects["onRSSE"] as TextObject;
            TextObject toRSIP = rd.ReportDefinition.ReportObjects["onRSIP"] as TextObject;
            TextObject toRSFP = rd.ReportDefinition.ReportObjects["onRSFP"] as TextObject;
            TextObject toRSAP = rd.ReportDefinition.ReportObjects["onRSAP"] as TextObject;
            TextObject toRSCP = rd.ReportDefinition.ReportObjects["onRSCP"] as TextObject;
            //T-Score
            TextObject toTSAN = rd.ReportDefinition.ReportObjects["onTSAN"] as TextObject;
            TextObject toTSDP = rd.ReportDefinition.ReportObjects["onTSDP"] as TextObject;
            TextObject toTSSI = rd.ReportDefinition.ReportObjects["onTSSI"] as TextObject;
            TextObject toTSSA = rd.ReportDefinition.ReportObjects["onTSSA"] as TextObject;
            TextObject toTSSE = rd.ReportDefinition.ReportObjects["onTSSE"] as TextObject;
            TextObject toTSIP = rd.ReportDefinition.ReportObjects["onTSIP"] as TextObject;
            TextObject toTSFP = rd.ReportDefinition.ReportObjects["onTSFP"] as TextObject;
            TextObject toTSAP = rd.ReportDefinition.ReportObjects["onTSAP"] as TextObject;
            TextObject toTSCP = rd.ReportDefinition.ReportObjects["onTSCP"] as TextObject;
            //% ile
            TextObject toPerAN = rd.ReportDefinition.ReportObjects["onPerAN"] as TextObject;
            TextObject toPerDP = rd.ReportDefinition.ReportObjects["onPerDP"] as TextObject;
            TextObject toPerSI = rd.ReportDefinition.ReportObjects["onPerSI"] as TextObject;
            TextObject toPerSA = rd.ReportDefinition.ReportObjects["onPerSA"] as TextObject;
            TextObject toPerSE = rd.ReportDefinition.ReportObjects["onPerSE"] as TextObject;
            TextObject toPerIP = rd.ReportDefinition.ReportObjects["onPerIP"] as TextObject;
            TextObject toPerFP = rd.ReportDefinition.ReportObjects["onPerFP"] as TextObject;
            TextObject toPerAP = rd.ReportDefinition.ReportObjects["onPerAP"] as TextObject;
            TextObject toPerCP = rd.ReportDefinition.ReportObjects["onPerCP"] as TextObject;


            try
            {
                con.Open();

                cmd = new SqlCommand(@"SELECT [PsychologicalTestInterpretationID]
                                  ,[AnxietyRowScore],[AnxietyTScore] ,[AnxietyPercentILE],[DepressionRowScore],[DepressionTScore],[DepressionPercentILE]
                                  ,[SuicidalIDeationRowScore],[SuicidalIDeationTScore],[SuicidalIDeationPercentILE],[SubstanceAbuseRowScore],[SubstanceAbuseTScore],
                                  [SubstanceAbusePercentILE],[Self_EsteemProblemRowScore],[Self_EsteemProblemTScore],[Self_EsteemProblemPercentILE],
                                  [InterpersonalProblemRowScore],[InterpersonalProblemTScore],[InterpersonalProblemPercentILE],[FamilyProblemRowScore],[FamilyProblemTScore]
                                  ,[FamilyProblemPercentILE],[AcademicProblemRowScore],[AcademicProblemTScore],[AcademicProblemPercentILE],[CareerProblemRowScore]
                                  ,[CareerProblemTScore],[CareerProblemPercentILE]
                              FROM [dbo].[PsychologicalTestInterpretation]
                              WHERE [StudentIDNumber] = '" + this.lblIDNum.Text.Replace("ID Number:","") + "'", con);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.ANrow = int.Parse(rdr["AnxietyRowScore"].ToString());
                    this.ANts = int.Parse(rdr["AnxietyTScore"].ToString());
                    this.ANile = int.Parse(rdr["AnxietyPercentILE"].ToString());
                    //
                    this.DProw = int.Parse(rdr["DepressionRowScore"].ToString());
                    this.DPts = int.Parse(rdr["DepressionTScore"].ToString());
                    this.DPile = int.Parse(rdr["DepressionPercentILE"].ToString());
                    //
                    this.SIrow = int.Parse(rdr["SuicidalIDeationRowScore"].ToString());
                    this.SIts = int.Parse(rdr["SuicidalIDeationTScore"].ToString());
                    this.SIile = int.Parse(rdr["SuicidalIDeationPercentILE"].ToString());
                    //
                    this.SArow = int.Parse(rdr["SubstanceAbuseRowScore"].ToString());
                    this.SAts = int.Parse(rdr["SubstanceAbuseTScore"].ToString());
                    this.SAile = int.Parse(rdr["SubstanceAbusePercentILE"].ToString());
                    //
                    this.SErow = int.Parse(rdr["Self_EsteemProblemRowScore"].ToString());
                    this.SEts = int.Parse(rdr["Self_EsteemProblemTScore"].ToString());
                    this.SEile = int.Parse(rdr["Self_EsteemProblemPercentILE"].ToString());
                    //
                    this.IProw = int.Parse(rdr["InterpersonalProblemRowScore"].ToString());
                    this.IPts = int.Parse(rdr["InterpersonalProblemTScore"].ToString());
                    this.IPile = int.Parse(rdr["InterpersonalProblemPercentILE"].ToString());
                    //
                    this.FProw = int.Parse(rdr["FamilyProblemRowScore"].ToString());
                    this.FPts = int.Parse(rdr["FamilyProblemTScore"].ToString());
                    this.FPile = int.Parse(rdr["FamilyProblemPercentILE"].ToString());
                    //
                    this.AProw = int.Parse(rdr["AcademicProblemRowScore"].ToString());
                    this.APts = int.Parse(rdr["AcademicProblemTScore"].ToString());
                    this.APile = int.Parse(rdr["AcademicProblemPercentILE"].ToString());
                    //
                    this.CProw = int.Parse(rdr["CareerProblemRowScore"].ToString());
                    this.CPts = int.Parse(rdr["CareerProblemTScore"].ToString());
                    this.CPile = int.Parse(rdr["CareerProblemPercentILE"].ToString());
                }

                con.Close();

                //Anxiety
                toRSAN.Text = ANrow.ToString();
                toTSAN.Text = ANts.ToString();
                toPerAN.Text = ANile.ToString();


                if (this.ANts >= 60)
                {
                    toTSAN.Color = Color.Red;
                }

                if (this.ANile >= 84)
                {
                    toPerAN.Color = Color.Red;
                }


                //Depression
                toRSDP.Text = DProw.ToString();
                toTSDP.Text = DPts.ToString();
                toPerDP.Text = DPile.ToString();


                if (this.DPts >= 60)
                {
                    toTSDP.Color = Color.Red;
                }

                if (this.DPile >= 84)
                {
                    toPerDP.Color = Color.Red;
                }


                //Suicidal Ideation
                toRSSI.Text = SIrow.ToString();
                toTSSI.Text = SIts.ToString();
                toPerSI.Text = SIile.ToString();


                if (this.SIts >= 60)
                {
                    toTSSI.Color = Color.Red;
                }

                if (this.SIile >= 84)
                {
                    toPerSI.Color = Color.Red;
                }


                //Substance Abuse
                toRSSA.Text = SArow.ToString();
                toTSSA.Text = SAts.ToString();
                toPerSA.Text = SAile.ToString();


                if (this.SAts >= 60)
                {
                    toTSSA.Color = Color.Red;
                }

                if (this.SAile >= 84)
                {
                    toPerSA.Color = Color.Red;
                }


                //Self-esteem Problems
                toRSSE.Text = SErow.ToString();
                toTSSE.Text = SEts.ToString();
                toPerSE.Text = SEile.ToString();


                if (this.SEts >= 60)
                {
                    toTSSE.Color = Color.Red;
                }

                if (this.SEile >= 84)
                {
                    toPerSE.Color = Color.Red;
                }


                //Interpersonal Problems
                toRSIP.Text = IProw.ToString();
                toTSIP.Text = IPts.ToString();
                toPerIP.Text = IPile.ToString();


                if (this.IPts >= 60)
                {
                    toTSIP.Color = Color.Red;
                }

                if (this.IPile >= 84)
                {
                    toPerIP.Color = Color.Red;
                }


                //Family Problems
                toRSFP.Text = FProw.ToString();
                toTSFP.Text = FPts.ToString();
                toPerFP.Text = FPile.ToString();


                if (this.FPts >= 60)
                {
                    toTSFP.Color = Color.Red;
                }

                if (this.FPile >= 84)
                {
                    toPerFP.Color = Color.Red;
                }


                //Academic Problems
                toRSAP.Text = AProw.ToString();
                toTSAP.Text = APts.ToString();
                toPerAP.Text = APile.ToString();


                if (this.APts >= 60)
                {
                    toTSAP.Color = Color.Red;
                }

                if (this.APile >= 84)
                {
                    toPerAP.Color = Color.Red;
                }


                //Career Problems
                toRSCP.Text = CProw.ToString();
                toTSCP.Text = CPts.ToString();
                toPerCP.Text = CPile.ToString();


                if (this.CPts >= 60)
                {
                    toTSCP.Color = Color.Red;
                }

                if (this.CPile >= 84)
                {
                    toPerCP.Color = Color.Red;
                }

                this.rd.Refresh();
                this.crViewerCASProf.ReportSource = rd;
                this.crViewerCASProf.Refresh();
                this.crViewerCASProf.RefreshReport();
               
                
               
            }
            catch { }
        }

        private void lnkSearchStudent_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form fSearch = new CAS_Profile.frmSearch(this.sSetServer, this.sSetDBName, this, this.lblName, this.lblIDNum,this.fSetHome);
            fSearch.ShowDialog();
        }
    }
}
