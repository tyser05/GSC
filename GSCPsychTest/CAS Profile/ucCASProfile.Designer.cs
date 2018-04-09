namespace GSCPsychTest.CAS_Profile
{
    partial class ucCASProfile
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.crViewerCASProf = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lnkSearchStudent = new System.Windows.Forms.LinkLabel();
            this.lblName = new System.Windows.Forms.Label();
            this.lblIDNum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // crViewerCASProf
            // 
            this.crViewerCASProf.ActiveViewIndex = -1;
            this.crViewerCASProf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crViewerCASProf.Cursor = System.Windows.Forms.Cursors.Default;
            this.crViewerCASProf.DisplayStatusBar = false;
            this.crViewerCASProf.Location = new System.Drawing.Point(25, 79);
            this.crViewerCASProf.Name = "crViewerCASProf";
            this.crViewerCASProf.ShowCloseButton = false;
            this.crViewerCASProf.ShowGotoPageButton = false;
            this.crViewerCASProf.ShowGroupTreeButton = false;
            this.crViewerCASProf.ShowLogo = false;
            this.crViewerCASProf.ShowPageNavigateButtons = false;
            this.crViewerCASProf.ShowParameterPanelButton = false;
            this.crViewerCASProf.ShowTextSearchButton = false;
            this.crViewerCASProf.Size = new System.Drawing.Size(998, 526);
            this.crViewerCASProf.TabIndex = 0;
            this.crViewerCASProf.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // lnkSearchStudent
            // 
            this.lnkSearchStudent.AutoSize = true;
            this.lnkSearchStudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSearchStudent.LinkColor = System.Drawing.Color.Gold;
            this.lnkSearchStudent.Location = new System.Drawing.Point(856, 46);
            this.lnkSearchStudent.Name = "lnkSearchStudent";
            this.lnkSearchStudent.Size = new System.Drawing.Size(167, 20);
            this.lnkSearchStudent.TabIndex = 1;
            this.lnkSearchStudent.TabStop = true;
            this.lnkSearchStudent.Text = "Search Student Name";
            this.lnkSearchStudent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSearchStudent_LinkClicked);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(21, 46);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(55, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            // 
            // lblIDNum
            // 
            this.lblIDNum.AutoSize = true;
            this.lblIDNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIDNum.Location = new System.Drawing.Point(21, 22);
            this.lblIDNum.Name = "lblIDNum";
            this.lblIDNum.Size = new System.Drawing.Size(90, 20);
            this.lblIDNum.TabIndex = 3;
            this.lblIDNum.Text = "ID Number:";
            // 
            // ucCASProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblIDNum);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lnkSearchStudent);
            this.Controls.Add(this.crViewerCASProf);
            this.Name = "ucCASProfile";
            this.Size = new System.Drawing.Size(1053, 620);
            this.Load += new System.EventHandler(this.ucCASProfile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crViewerCASProf;
        private System.Windows.Forms.LinkLabel lnkSearchStudent;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblIDNum;
    }
}
