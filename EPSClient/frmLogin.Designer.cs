namespace HRSeat
{
    partial class frmLogin
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkRemeberPWD = new System.Windows.Forms.CheckBox();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ttUserName = new System.Windows.Forms.ToolTip(this.components);
            this.ttPassword = new System.Windows.Forms.ToolTip(this.components);
            this.ttError = new System.Windows.Forms.ToolTip(this.components);
            this.pnlErrorMsg = new System.Windows.Forms.Panel();
            this.lblerror3 = new System.Windows.Forms.Label();
            this.lblerror2 = new System.Windows.Forms.Label();
            this.lblerror1 = new System.Windows.Forms.Label();
            this.lblerrorT = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.llblSelectIP = new System.Windows.Forms.LinkLabel();
            this.chkAnonymous = new System.Windows.Forms.CheckBox();
            this.pnlErrorMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTitle.Location = new System.Drawing.Point(7, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(158, 17);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HR员工座位图分布系统V1.0";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblUserName.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblUserName.Location = new System.Drawing.Point(81, 127);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(49, 14);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "帐号：";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPassword.Location = new System.Drawing.Point(81, 169);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(49, 14);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "密码：";
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.Font = new System.Drawing.Font("宋体", 12F);
            this.txtUserName.Location = new System.Drawing.Point(142, 123);
            this.txtUserName.MaxLength = 20;
            this.txtUserName.Multiline = true;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(197, 24);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserName_KeyPress);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("宋体", 12F);
            this.txtPassword.Location = new System.Drawing.Point(142, 165);
            this.txtPassword.MaxLength = 20;
            this.txtPassword.Multiline = true;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(197, 24);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // chkRemeberPWD
            // 
            this.chkRemeberPWD.AutoSize = true;
            this.chkRemeberPWD.BackColor = System.Drawing.Color.Transparent;
            this.chkRemeberPWD.Location = new System.Drawing.Point(84, 205);
            this.chkRemeberPWD.Name = "chkRemeberPWD";
            this.chkRemeberPWD.Size = new System.Drawing.Size(72, 16);
            this.chkRemeberPWD.TabIndex = 5;
            this.chkRemeberPWD.Text = "记住密码";
            this.chkRemeberPWD.UseVisualStyleBackColor = false;
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoStart.Location = new System.Drawing.Point(249, 205);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(96, 16);
            this.chkAutoStart.TabIndex = 5;
            this.chkAutoStart.Text = "开机自动启用";
            this.chkAutoStart.UseVisualStyleBackColor = false;
            this.chkAutoStart.CheckedChanged += new System.EventHandler(this.chkAutoStart_CheckedChanged);
            // 
            // btnLogin
            // 
            this.btnLogin.BackgroundImage = global::HRSeat.Properties.Resources.button_lod;
            this.btnLogin.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLogin.Location = new System.Drawing.Point(142, 243);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(74, 28);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "登 录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::HRSeat.Properties.Resources.button_lod;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(265, 243);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "关 闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ttUserName
            // 
            this.ttUserName.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // ttPassword
            // 
            this.ttPassword.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // ttError
            // 
            this.ttError.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // pnlErrorMsg
            // 
            this.pnlErrorMsg.BackColor = System.Drawing.Color.Transparent;
            this.pnlErrorMsg.Controls.Add(this.lblerror3);
            this.pnlErrorMsg.Controls.Add(this.lblerror2);
            this.pnlErrorMsg.Controls.Add(this.lblerror1);
            this.pnlErrorMsg.Controls.Add(this.lblerrorT);
            this.pnlErrorMsg.Controls.Add(this.btnOK);
            this.pnlErrorMsg.Location = new System.Drawing.Point(21, 30);
            this.pnlErrorMsg.Name = "pnlErrorMsg";
            this.pnlErrorMsg.Size = new System.Drawing.Size(396, 246);
            this.pnlErrorMsg.TabIndex = 7;
            this.pnlErrorMsg.Visible = false;
            // 
            // lblerror3
            // 
            this.lblerror3.AutoSize = true;
            this.lblerror3.Font = new System.Drawing.Font("宋体", 11F);
            this.lblerror3.Location = new System.Drawing.Point(72, 147);
            this.lblerror3.Name = "lblerror3";
            this.lblerror3.Size = new System.Drawing.Size(112, 15);
            this.lblerror3.TabIndex = 2;
            this.lblerror3.Text = "③未开启小键盘";
            // 
            // lblerror2
            // 
            this.lblerror2.AutoSize = true;
            this.lblerror2.Font = new System.Drawing.Font("宋体", 11F);
            this.lblerror2.Location = new System.Drawing.Point(72, 128);
            this.lblerror2.Name = "lblerror2";
            this.lblerror2.Size = new System.Drawing.Size(142, 15);
            this.lblerror2.TabIndex = 2;
            this.lblerror2.Text = "②未区分字母大小写";
            // 
            // lblerror1
            // 
            this.lblerror1.AutoSize = true;
            this.lblerror1.Font = new System.Drawing.Font("宋体", 11F);
            this.lblerror1.Location = new System.Drawing.Point(72, 108);
            this.lblerror1.Name = "lblerror1";
            this.lblerror1.Size = new System.Drawing.Size(82, 15);
            this.lblerror1.TabIndex = 2;
            this.lblerror1.Text = "①忘记密码";
            // 
            // lblerrorT
            // 
            this.lblerrorT.AutoSize = true;
            this.lblerrorT.Font = new System.Drawing.Font("宋体", 11F);
            this.lblerrorT.Location = new System.Drawing.Point(52, 83);
            this.lblerrorT.Name = "lblerrorT";
            this.lblerrorT.Size = new System.Drawing.Size(292, 15);
            this.lblerrorT.TabIndex = 2;
            this.lblerrorT.Text = "您输入的帐号或密码不正确，原因可能是：";
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::HRSeat.Properties.Resources.button_lod;
            this.btnOK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOK.Location = new System.Drawing.Point(162, 213);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 28);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确 定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // llblSelectIP
            // 
            this.llblSelectIP.AutoSize = true;
            this.llblSelectIP.BackColor = System.Drawing.Color.Transparent;
            this.llblSelectIP.LinkColor = System.Drawing.Color.Navy;
            this.llblSelectIP.Location = new System.Drawing.Point(32, 251);
            this.llblSelectIP.Name = "llblSelectIP";
            this.llblSelectIP.Size = new System.Drawing.Size(89, 12);
            this.llblSelectIP.TabIndex = 8;
            this.llblSelectIP.TabStop = true;
            this.llblSelectIP.Text = "重新选择IP地址";
            this.llblSelectIP.Visible = false;
            this.llblSelectIP.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblSelectIP_LinkClicked);
            // 
            // chkAnonymous
            // 
            this.chkAnonymous.AutoSize = true;
            this.chkAnonymous.BackColor = System.Drawing.Color.Transparent;
            this.chkAnonymous.Location = new System.Drawing.Point(166, 205);
            this.chkAnonymous.Name = "chkAnonymous";
            this.chkAnonymous.Size = new System.Drawing.Size(72, 16);
            this.chkAnonymous.TabIndex = 5;
            this.chkAnonymous.Text = "匿名登陆";
            this.chkAnonymous.UseVisualStyleBackColor = false;
            this.chkAnonymous.CheckedChanged += new System.EventHandler(this.chkAnonymous_CheckedChanged);
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::HRSeat.Properties.Resources.Login_bj;
            this.ClientSize = new System.Drawing.Size(441, 288);
            this.Controls.Add(this.pnlErrorMsg);
            this.Controls.Add(this.llblSelectIP);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.chkAnonymous);
            this.Controls.Add(this.chkAutoStart);
            this.Controls.Add(this.chkRemeberPWD);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.Text = "系统登录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogin_FormClosing);
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.Shown += new System.EventHandler(this.frmLogin_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmLogin_Paint);
            this.pnlErrorMsg.ResumeLayout(false);
            this.pnlErrorMsg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkRemeberPWD;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip ttUserName;
        private System.Windows.Forms.ToolTip ttPassword;
        private System.Windows.Forms.ToolTip ttError;
        private System.Windows.Forms.Panel pnlErrorMsg;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.LinkLabel llblSelectIP;
        private System.Windows.Forms.Label lblerror3;
        private System.Windows.Forms.Label lblerror2;
        private System.Windows.Forms.Label lblerror1;
        private System.Windows.Forms.Label lblerrorT;
        private System.Windows.Forms.CheckBox chkAnonymous;
    }
}