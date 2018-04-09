using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GSCPsychTest
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        public void Navigate(UserControl uc)
        {
            if (this.pnlProcess.Controls.Count > 0)
                this.pnlProcess.Controls.Clear();

            this.pnlProcess.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            this.lblHeader.Text = this.btnHome.Text;
            this.btnHome.BackColor = Color.Blue;
        }

        //Exit
        private void lnkExit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.btnHome.BackColor = Color.Blue;
            this.btnPysch.BackColor = SystemColors.Control;
            this.btnCASProf.BackColor = SystemColors.Control;

            this.lblHeader.Text = this.btnHome.Text;

            this.pnlProcess.Controls.Clear();
        }

        private void btnPysch_Click(object sender, EventArgs e)
        {
            this.btnHome.BackColor = SystemColors.Control;
            this.btnPysch.BackColor = Color.Blue;
            this.btnCASProf.BackColor = SystemColors.Control;
            this.lblHeader.Text = this.btnPysch.Text;
            this.Navigate(new Psych_Test.ucPsychTest(this.txtServer.Text,this.txtDB.Text));
        }

        private void btnHome_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnHome_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                if (this.txtServer.Visible == false)
                {
                    this.txtServer.Visible = true;
                    this.txtDB.Visible = true;
                }
                else
                {
                    this.txtServer.Visible = false;
                    this.txtDB.Visible = false;
                }
            }
        }

        private void btnTransferData_Click(object sender, EventArgs e)
        {
            this.btnHome.BackColor = SystemColors.Control;
            this.btnPysch.BackColor = SystemColors.Control;
            this.btnTransferData.BackColor = Color.Blue;

            this.lblHeader.Text = this.btnTransferData.Text;
        }

        private void btnCASProf_Click(object sender, EventArgs e)
        {
            this.btnHome.BackColor = SystemColors.Control;
            this.btnPysch.BackColor = SystemColors.Control;
            this.btnCASProf.BackColor = Color.Blue;

            this.lblHeader.Text = this.btnCASProf.Text;
            this.Navigate(new CAS_Profile.ucCASProfile(this.txtServer.Text,this.txtDB.Text,"",this));
        }

        
    }
}
