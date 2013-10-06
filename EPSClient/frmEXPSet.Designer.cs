namespace HRSeat
{
    partial class frmEXPSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEXPSet));
            this.btnOK = new System.Windows.Forms.Button();
            this.gbxExpType = new System.Windows.Forms.GroupBox();
            this.rbtnDept = new System.Windows.Forms.RadioButton();
            this.rbtnName = new System.Windows.Forms.RadioButton();
            this.gbxExpType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOK.Location = new System.Drawing.Point(65, 89);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 28);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确 定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbxExpType
            // 
            this.gbxExpType.Controls.Add(this.rbtnDept);
            this.gbxExpType.Controls.Add(this.btnOK);
            this.gbxExpType.Controls.Add(this.rbtnName);
            this.gbxExpType.Location = new System.Drawing.Point(12, 12);
            this.gbxExpType.Name = "gbxExpType";
            this.gbxExpType.Size = new System.Drawing.Size(200, 123);
            this.gbxExpType.TabIndex = 2;
            this.gbxExpType.TabStop = false;
            this.gbxExpType.Text = "请选择导出方式";
            // 
            // rbtnDept
            // 
            this.rbtnDept.AutoSize = true;
            this.rbtnDept.Location = new System.Drawing.Point(46, 63);
            this.rbtnDept.Name = "rbtnDept";
            this.rbtnDept.Size = new System.Drawing.Size(119, 16);
            this.rbtnDept.TabIndex = 1;
            this.rbtnDept.TabStop = true;
            this.rbtnDept.Text = "按照部门房间导出";
            this.rbtnDept.UseVisualStyleBackColor = true;
            // 
            // rbtnName
            // 
            this.rbtnName.AutoSize = true;
            this.rbtnName.Location = new System.Drawing.Point(46, 31);
            this.rbtnName.Name = "rbtnName";
            this.rbtnName.Size = new System.Drawing.Size(107, 16);
            this.rbtnName.TabIndex = 0;
            this.rbtnName.TabStop = true;
            this.rbtnName.Text = "按姓名排序导出";
            this.rbtnName.UseVisualStyleBackColor = true;
            // 
            // frmEXPSet
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(236, 147);
            this.Controls.Add(this.gbxExpType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEXPSet";
            this.ShowIcon = false;
            this.Text = "导出通讯录";
            this.gbxExpType.ResumeLayout(false);
            this.gbxExpType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbxExpType;
        private System.Windows.Forms.RadioButton rbtnDept;
        private System.Windows.Forms.RadioButton rbtnName;
    }
}