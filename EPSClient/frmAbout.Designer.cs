namespace HRSeat
{
    partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.llblWebSite = new System.Windows.Forms.LinkLabel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblCopyrightCN = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblVer = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // llblWebSite
            // 
            this.llblWebSite.AutoSize = true;
            this.llblWebSite.Location = new System.Drawing.Point(43, 115);
            this.llblWebSite.Name = "llblWebSite";
            this.llblWebSite.Size = new System.Drawing.Size(83, 12);
            this.llblWebSite.TabIndex = 10;
            this.llblWebSite.TabStop = true;
            this.llblWebSite.Text = "www.jfsys.com";
            this.llblWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblWebSite_LinkClicked);
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(12, 12);
            this.picLogo.MinimumSize = new System.Drawing.Size(32, 32);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(32, 32);
            this.picLogo.TabIndex = 9;
            this.picLogo.TabStop = false;
            // 
            // lblCopyrightCN
            // 
            this.lblCopyrightCN.AutoSize = true;
            this.lblCopyrightCN.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyrightCN.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCopyrightCN.Location = new System.Drawing.Point(43, 97);
            this.lblCopyrightCN.Name = "lblCopyrightCN";
            this.lblCopyrightCN.Size = new System.Drawing.Size(215, 12);
            this.lblCopyrightCN.TabIndex = 8;
            this.lblCopyrightCN.Text = "北京尖峰计算机系统有限公司 版权所有";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyright.Font = new System.Drawing.Font("宋体", 9F);
            this.lblCopyright.Location = new System.Drawing.Point(43, 76);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(275, 12);
            this.lblCopyright.TabIndex = 7;
            this.lblCopyright.Text = "Copyright(C) 2013 HR SYS All Rights Reserved.";
            // 
            // lblVer
            // 
            this.lblVer.AutoSize = true;
            this.lblVer.BackColor = System.Drawing.Color.Transparent;
            this.lblVer.Location = new System.Drawing.Point(43, 56);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(197, 12);
            this.lblVer.TabIndex = 5;
            this.lblVer.Text = "版本：V1.0 Release(Build 130726)";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(50, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(242, 22);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "HR员工座位分布系统（客户端）";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(248, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(236)))));
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Location = new System.Drawing.Point(-5, 143);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(348, 33);
            this.panel1.TabIndex = 11;
            // 
            // frmAbout
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(329, 176);
            this.Controls.Add(this.llblWebSite);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.lblCopyrightCN);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblVer);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAbout";
            this.ShowIcon = false;
            this.Text = "关于HR管理系统";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel llblWebSite;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblCopyrightCN;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblVer;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel1;

    }
}