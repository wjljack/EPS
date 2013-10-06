namespace HRSeat
{
    partial class frmSettings
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
            this.btnSave = new System.Windows.Forms.Button();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.chkSeat = new System.Windows.Forms.CheckBox();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.chkOA = new System.Windows.Forms.CheckBox();
            this.gbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(80, 130);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(74, 28);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保 存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.chkSeat);
            this.gbSettings.Controls.Add(this.chkAutoStart);
            this.gbSettings.Controls.Add(this.chkOA);
            this.gbSettings.Location = new System.Drawing.Point(18, 20);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(200, 92);
            this.gbSettings.TabIndex = 4;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "系统设置";
            // 
            // chkSeat
            // 
            this.chkSeat.AutoSize = true;
            this.chkSeat.Location = new System.Drawing.Point(6, 64);
            this.chkSeat.Name = "chkSeat";
            this.chkSeat.Size = new System.Drawing.Size(120, 16);
            this.chkSeat.TabIndex = 1;
            this.chkSeat.Text = "启动时显示座位图";
            this.chkSeat.UseVisualStyleBackColor = true;
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(6, 20);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(96, 16);
            this.chkAutoStart.TabIndex = 0;
            this.chkAutoStart.Text = "开机自动启动";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            // 
            // chkOA
            // 
            this.chkOA.AutoSize = true;
            this.chkOA.Location = new System.Drawing.Point(6, 42);
            this.chkOA.Name = "chkOA";
            this.chkOA.Size = new System.Drawing.Size(120, 16);
            this.chkOA.TabIndex = 0;
            this.chkOA.Text = "启动时显示OA日程";
            this.chkOA.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(238, 174);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.ShowIcon = false;
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.CheckBox chkSeat;
        private System.Windows.Forms.CheckBox chkOA;
        private System.Windows.Forms.CheckBox chkAutoStart;
    }
}