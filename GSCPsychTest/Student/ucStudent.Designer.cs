namespace GSCPsychTest.Student
{
    partial class ucStudent
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
            this.lblNumOfPsychTaken = new System.Windows.Forms.Label();
            this.pBarTransferData = new System.Windows.Forms.ProgressBar();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.dataGridGetExcelData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGetExcelData)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNumOfPsychTaken
            // 
            this.lblNumOfPsychTaken.AutoSize = true;
            this.lblNumOfPsychTaken.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumOfPsychTaken.Location = new System.Drawing.Point(176, 27);
            this.lblNumOfPsychTaken.Name = "lblNumOfPsychTaken";
            this.lblNumOfPsychTaken.Size = new System.Drawing.Size(326, 20);
            this.lblNumOfPsychTaken.TabIndex = 9;
            this.lblNumOfPsychTaken.Text = "Number of Student take Psychological Test 0";
            // 
            // pBarTransferData
            // 
            this.pBarTransferData.Location = new System.Drawing.Point(15, 456);
            this.pBarTransferData.Name = "pBarTransferData";
            this.pBarTransferData.Size = new System.Drawing.Size(863, 33);
            this.pBarTransferData.TabIndex = 8;
            // 
            // btnTransfer
            // 
            this.btnTransfer.Enabled = false;
            this.btnTransfer.Location = new System.Drawing.Point(884, 456);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(141, 33);
            this.btnTransfer.TabIndex = 7;
            this.btnTransfer.Text = "Transfer Data";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(15, 22);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(141, 33);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "Browse File";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // dataGridGetExcelData
            // 
            this.dataGridGetExcelData.AllowUserToAddRows = false;
            this.dataGridGetExcelData.AllowUserToDeleteRows = false;
            this.dataGridGetExcelData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridGetExcelData.Location = new System.Drawing.Point(15, 61);
            this.dataGridGetExcelData.Name = "dataGridGetExcelData";
            this.dataGridGetExcelData.ReadOnly = true;
            this.dataGridGetExcelData.Size = new System.Drawing.Size(1010, 389);
            this.dataGridGetExcelData.TabIndex = 5;
            // 
            // ucStudent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblNumOfPsychTaken);
            this.Controls.Add(this.pBarTransferData);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.dataGridGetExcelData);
            this.Name = "ucStudent";
            this.Size = new System.Drawing.Size(1041, 511);
            this.Load += new System.EventHandler(this.ucStudent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGetExcelData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNumOfPsychTaken;
        private System.Windows.Forms.ProgressBar pBarTransferData;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.DataGridView dataGridGetExcelData;
    }
}
