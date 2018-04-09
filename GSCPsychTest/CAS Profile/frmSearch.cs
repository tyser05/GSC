using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GSCPsychTest.CAS_Profile
{
    public partial class frmSearch : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr;

        private CAS_Profile.ucCASProfile ucSetCASProfile;
        private frmHome fSetHome;
        private Label lblSetName, lblSetIDNum;

        string sSetServer, sSetDBName;
        public frmSearch(string sGetServer, string sGetDBName, CAS_Profile.ucCASProfile ucGetCASProfile, Label lblGetName, Label lblGetIDNum, frmHome fGetHome)
        {
            InitializeComponent();

            this.sSetServer = sGetServer;
            this.sSetDBName = sGetDBName;
            this.con = new SqlConnection(@"Data Source = "+sGetServer+"; Initial Catalog = "+sGetDBName+"; Integrated Security = true;");
            this.ucSetCASProfile = ucGetCASProfile;
            this.lblSetName = lblGetName;
            this.lblSetIDNum = lblGetIDNum;
            this.fSetHome = fGetHome;
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            this.LoadStudRec();
        }

        public void LoadStudRec()
        {
            this.dataGridStudent.Rows.Clear();
            con.Open();

            cmd = new SqlCommand(@"select *
                from tblStudent", con);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.dataGridStudent.Rows.Add(rdr["studID"].ToString(), rdr["studIDNum"].ToString(), rdr["studName"].ToString());
            }

            con.Close();
        }
        
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            this.dataGridStudent.Rows.Clear();
            con.Open();

            cmd = new SqlCommand(@"select *
                from tblStudent
                where studIDNum like '%" + this.txtSearch.Text.Replace("'", "") + "%' or studName like '%" 
                                         + this.txtSearch.Text.Replace("'", "") + "%'", con);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.dataGridStudent.Rows.Add(rdr["studID"].ToString(), rdr["studIDNum"].ToString(), rdr["studName"].ToString());
            }

            con.Close();
        }

        private void dataGridStudent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.lblSetIDNum.Text = "ID Number: " + this.dataGridStudent.SelectedCells[1].Value.ToString();
            this.lblSetName.Text = "Name: " + this.dataGridStudent.SelectedCells[2].Value.ToString();

            
            this.ucSetCASProfile.CASProfileSummary(this.dataGridStudent.SelectedCells[1].Value.ToString());
            //this.fSetHome.Navigate(new CAS_Profile.ucCASProfile(this.sSetServer, this.sSetDBName,
            //    this.dataGridStudent.SelectedCells[1].Value.ToString(), this.fSetHome));

            this.Close();
            Cursor.Current = Cursors.Default;
        }
    }
}
