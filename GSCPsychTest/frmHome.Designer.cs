namespace GSCPsychTest
{
    partial class frmHome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnCASProf = new System.Windows.Forms.Button();
            this.btnPysch = new System.Windows.Forms.Button();
            this.btnTransferData = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtDB = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lnkExit = new System.Windows.Forms.LinkLabel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pnlProcess = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Gold;
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(277, 748);
            this.panel1.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnCASProf);
            this.panel6.Controls.Add(this.btnPysch);
            this.panel6.Controls.Add(this.btnTransferData);
            this.panel6.Controls.Add(this.btnHome);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 89);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panel6.Size = new System.Drawing.Size(277, 659);
            this.panel6.TabIndex = 1;
            // 
            // btnCASProf
            // 
            this.btnCASProf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCASProf.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCASProf.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCASProf.Location = new System.Drawing.Point(0, 232);
            this.btnCASProf.Name = "btnCASProf";
            this.btnCASProf.Size = new System.Drawing.Size(277, 74);
            this.btnCASProf.TabIndex = 3;
            this.btnCASProf.Text = "CAS Profile";
            this.btnCASProf.UseVisualStyleBackColor = true;
            this.btnCASProf.Click += new System.EventHandler(this.btnCASProf_Click);
            // 
            // btnPysch
            // 
            this.btnPysch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPysch.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPysch.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPysch.Location = new System.Drawing.Point(0, 158);
            this.btnPysch.Name = "btnPysch";
            this.btnPysch.Size = new System.Drawing.Size(277, 74);
            this.btnPysch.TabIndex = 1;
            this.btnPysch.Text = "Psychoogical Test";
            this.btnPysch.UseVisualStyleBackColor = true;
            this.btnPysch.Click += new System.EventHandler(this.btnPysch_Click);
            // 
            // btnTransferData
            // 
            this.btnTransferData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTransferData.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTransferData.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransferData.Location = new System.Drawing.Point(0, 84);
            this.btnTransferData.Name = "btnTransferData";
            this.btnTransferData.Size = new System.Drawing.Size(277, 74);
            this.btnTransferData.TabIndex = 2;
            this.btnTransferData.Text = "Transfer Name";
            this.btnTransferData.UseVisualStyleBackColor = true;
            this.btnTransferData.Visible = false;
            this.btnTransferData.Click += new System.EventHandler(this.btnTransferData_Click);
            // 
            // btnHome
            // 
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(0, 10);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(277, 74);
            this.btnHome.TabIndex = 0;
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            this.btnHome.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnHome_KeyDown);
            this.btnHome.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnHome_KeyPress);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pbImage);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(277, 89);
            this.panel4.TabIndex = 0;
            // 
            // pbImage
            // 
            this.pbImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbImage.Image = global::GSCPsychTest.Properties.Resources.GuidanceLogoFinal;
            this.pbImage.Location = new System.Drawing.Point(0, 0);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(102, 89);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 1;
            this.pbImage.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(103, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "CPU-GSC";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.Blue;
            this.panel2.Controls.Add(this.txtDB);
            this.panel2.Controls.Add(this.txtServer);
            this.panel2.Controls.Add(this.lblHeader);
            this.panel2.Controls.Add(this.lnkExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(287, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1069, 89);
            this.panel2.TabIndex = 1;
            // 
            // txtDB
            // 
            this.txtDB.Location = new System.Drawing.Point(661, 36);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(164, 20);
            this.txtDB.TabIndex = 3;
            this.txtDB.Text = "gscPsych";
            this.txtDB.Visible = false;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(491, 36);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(164, 20);
            this.txtServer.TabIndex = 2;
            this.txtServer.Text = "127.0.0.1";
            this.txtServer.Visible = false;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.Gold;
            this.lblHeader.Location = new System.Drawing.Point(7, 19);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(175, 39);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "CPU-GSC";
            // 
            // lnkExit
            // 
            this.lnkExit.AutoSize = true;
            this.lnkExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExit.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkExit.LinkColor = System.Drawing.Color.Gold;
            this.lnkExit.Location = new System.Drawing.Point(971, 33);
            this.lnkExit.Name = "lnkExit";
            this.lnkExit.Size = new System.Drawing.Size(44, 25);
            this.lnkExit.TabIndex = 0;
            this.lnkExit.TabStop = true;
            this.lnkExit.Text = "Exit";
            this.lnkExit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExit_LinkClicked);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Control;
            this.panel5.Location = new System.Drawing.Point(-3, 90);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1366, 10);
            this.panel5.TabIndex = 1;
            // 
            // pnlProcess
            // 
            this.pnlProcess.AutoScroll = true;
            this.pnlProcess.BackColor = System.Drawing.Color.Blue;
            this.pnlProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProcess.Location = new System.Drawing.Point(287, 99);
            this.pnlProcess.Name = "pnlProcess";
            this.pnlProcess.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.pnlProcess.Size = new System.Drawing.Size(1069, 659);
            this.pnlProcess.TabIndex = 2;
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.pnlProcess);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmHome";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pysch Test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmHome_Load);
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.LinkLabel lnkExit;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnPysch;
        private System.Windows.Forms.Panel pnlProcess;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TextBox txtDB;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button btnTransferData;
        private System.Windows.Forms.Button btnCASProf;
    }
}

