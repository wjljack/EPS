namespace HRSeat
{
    partial class frmSeatEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeatEdit));
            this.lblTName = new System.Windows.Forms.Label();
            this.lblTDepartment = new System.Windows.Forms.Label();
            this.lblTRoom = new System.Windows.Forms.Label();
            this.lblTSeatCode = new System.Windows.Forms.Label();
            this.lblTWorkEmail = new System.Windows.Forms.Label();
            this.lblTWorkTelephone = new System.Windows.Forms.Label();
            this.lblTMobile = new System.Windows.Forms.Label();
            this.lblTIP = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.cboRoom = new System.Windows.Forms.ComboBox();
            this.txtSeatCode = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblLocationX = new System.Windows.Forms.Label();
            this.lblLocationY = new System.Windows.Forms.Label();
            this.txtLocationX = new System.Windows.Forms.TextBox();
            this.txtLocationY = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.cboEmployeeType = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.picSelectEMP = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.ttRoom = new System.Windows.Forms.ToolTip(this.components);
            this.ttSeatCode = new System.Windows.Forms.ToolTip(this.components);
            this.ttLocationX = new System.Windows.Forms.ToolTip(this.components);
            this.ttLocationY = new System.Windows.Forms.ToolTip(this.components);
            this.ttIP = new System.Windows.Forms.ToolTip(this.components);
            this.ttName = new System.Windows.Forms.ToolTip(this.components);
            this.txtWorkEmail = new System.Windows.Forms.TextBox();
            this.txtWorkTelephone = new System.Windows.Forms.TextBox();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.ttWorkEmail = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picSelectEMP)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTName
            // 
            this.lblTName.AutoSize = true;
            this.lblTName.BackColor = System.Drawing.Color.Transparent;
            this.lblTName.Location = new System.Drawing.Point(37, 157);
            this.lblTName.Name = "lblTName";
            this.lblTName.Size = new System.Drawing.Size(41, 12);
            this.lblTName.TabIndex = 0;
            this.lblTName.Text = "姓名：";
            // 
            // lblTDepartment
            // 
            this.lblTDepartment.AutoSize = true;
            this.lblTDepartment.BackColor = System.Drawing.Color.Transparent;
            this.lblTDepartment.Location = new System.Drawing.Point(37, 200);
            this.lblTDepartment.Name = "lblTDepartment";
            this.lblTDepartment.Size = new System.Drawing.Size(65, 12);
            this.lblTDepartment.TabIndex = 0;
            this.lblTDepartment.Text = "所属部门：";
            // 
            // lblTRoom
            // 
            this.lblTRoom.AutoSize = true;
            this.lblTRoom.BackColor = System.Drawing.Color.Transparent;
            this.lblTRoom.Location = new System.Drawing.Point(35, 33);
            this.lblTRoom.Name = "lblTRoom";
            this.lblTRoom.Size = new System.Drawing.Size(65, 12);
            this.lblTRoom.TabIndex = 0;
            this.lblTRoom.Text = "所属房间：";
            // 
            // lblTSeatCode
            // 
            this.lblTSeatCode.AutoSize = true;
            this.lblTSeatCode.BackColor = System.Drawing.Color.Transparent;
            this.lblTSeatCode.Location = new System.Drawing.Point(232, 33);
            this.lblTSeatCode.Name = "lblTSeatCode";
            this.lblTSeatCode.Size = new System.Drawing.Size(65, 12);
            this.lblTSeatCode.TabIndex = 0;
            this.lblTSeatCode.Text = "座位编号：";
            // 
            // lblTWorkEmail
            // 
            this.lblTWorkEmail.AutoSize = true;
            this.lblTWorkEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblTWorkEmail.Location = new System.Drawing.Point(37, 238);
            this.lblTWorkEmail.Name = "lblTWorkEmail";
            this.lblTWorkEmail.Size = new System.Drawing.Size(65, 12);
            this.lblTWorkEmail.TabIndex = 0;
            this.lblTWorkEmail.Text = "电子邮件：";
            // 
            // lblTWorkTelephone
            // 
            this.lblTWorkTelephone.AutoSize = true;
            this.lblTWorkTelephone.BackColor = System.Drawing.Color.Transparent;
            this.lblTWorkTelephone.Location = new System.Drawing.Point(37, 279);
            this.lblTWorkTelephone.Name = "lblTWorkTelephone";
            this.lblTWorkTelephone.Size = new System.Drawing.Size(41, 12);
            this.lblTWorkTelephone.TabIndex = 0;
            this.lblTWorkTelephone.Text = "座机：";
            // 
            // lblTMobile
            // 
            this.lblTMobile.AutoSize = true;
            this.lblTMobile.BackColor = System.Drawing.Color.Transparent;
            this.lblTMobile.Location = new System.Drawing.Point(265, 238);
            this.lblTMobile.Name = "lblTMobile";
            this.lblTMobile.Size = new System.Drawing.Size(41, 12);
            this.lblTMobile.TabIndex = 0;
            this.lblTMobile.Text = "手机：";
            // 
            // lblTIP
            // 
            this.lblTIP.AutoSize = true;
            this.lblTIP.BackColor = System.Drawing.Color.Transparent;
            this.lblTIP.Location = new System.Drawing.Point(37, 114);
            this.lblTIP.Name = "lblTIP";
            this.lblTIP.Size = new System.Drawing.Size(29, 12);
            this.lblTIP.TabIndex = 0;
            this.lblTIP.Text = "IP：";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(268, 330);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 28);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "关 闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cboRoom
            // 
            this.cboRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoom.FormattingEnabled = true;
            this.cboRoom.Location = new System.Drawing.Point(106, 30);
            this.cboRoom.Name = "cboRoom";
            this.cboRoom.Size = new System.Drawing.Size(121, 20);
            this.cboRoom.TabIndex = 0;
            // 
            // txtSeatCode
            // 
            this.txtSeatCode.Location = new System.Drawing.Point(291, 30);
            this.txtSeatCode.Name = "txtSeatCode";
            this.txtSeatCode.Size = new System.Drawing.Size(74, 21);
            this.txtSeatCode.TabIndex = 1;
            this.txtSeatCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSeatCode_KeyPress);
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.BackColor = System.Drawing.Color.Transparent;
            this.lblLocation.Location = new System.Drawing.Point(35, 74);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(65, 12);
            this.lblLocation.TabIndex = 0;
            this.lblLocation.Text = "坐标位置：";
            // 
            // lblLocationX
            // 
            this.lblLocationX.AutoSize = true;
            this.lblLocationX.BackColor = System.Drawing.Color.Transparent;
            this.lblLocationX.Location = new System.Drawing.Point(104, 74);
            this.lblLocationX.Name = "lblLocationX";
            this.lblLocationX.Size = new System.Drawing.Size(23, 12);
            this.lblLocationX.TabIndex = 0;
            this.lblLocationX.Text = "X：";
            // 
            // lblLocationY
            // 
            this.lblLocationY.AutoSize = true;
            this.lblLocationY.BackColor = System.Drawing.Color.Transparent;
            this.lblLocationY.Location = new System.Drawing.Point(199, 74);
            this.lblLocationY.Name = "lblLocationY";
            this.lblLocationY.Size = new System.Drawing.Size(23, 12);
            this.lblLocationY.TabIndex = 0;
            this.lblLocationY.Text = "Y：";
            // 
            // txtLocationX
            // 
            this.txtLocationX.Location = new System.Drawing.Point(125, 71);
            this.txtLocationX.Name = "txtLocationX";
            this.txtLocationX.Size = new System.Drawing.Size(48, 21);
            this.txtLocationX.TabIndex = 2;
            this.txtLocationX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLocationX_KeyPress);
            // 
            // txtLocationY
            // 
            this.txtLocationY.Location = new System.Drawing.Point(222, 71);
            this.txtLocationY.Name = "txtLocationY";
            this.txtLocationY.Size = new System.Drawing.Size(49, 21);
            this.txtLocationY.TabIndex = 3;
            this.txtLocationY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLocationY_KeyPress);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(106, 111);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 21);
            this.txtIP.TabIndex = 4;
            this.txtIP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIP_KeyPress);
            // 
            // cboEmployeeType
            // 
            this.cboEmployeeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEmployeeType.FormattingEnabled = true;
            this.cboEmployeeType.Location = new System.Drawing.Point(106, 154);
            this.cboEmployeeType.Name = "cboEmployeeType";
            this.cboEmployeeType.Size = new System.Drawing.Size(100, 20);
            this.cboEmployeeType.TabIndex = 5;
            this.cboEmployeeType.SelectedIndexChanged += new System.EventHandler(this.cboEmployeeType_SelectedIndexChanged);
            this.cboEmployeeType.DropDownClosed += new System.EventHandler(this.cboEmployeeType_DropDownClosed);
            this.cboEmployeeType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cboEmployeeType_KeyUp);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(234, 154);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(63, 21);
            this.txtName.TabIndex = 6;
            // 
            // picSelectEMP
            // 
            this.picSelectEMP.BackColor = System.Drawing.Color.Transparent;
            this.picSelectEMP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSelectEMP.Image = global::HRSeat.Properties.Resources.online;
            this.picSelectEMP.Location = new System.Drawing.Point(312, 152);
            this.picSelectEMP.Name = "picSelectEMP";
            this.picSelectEMP.Size = new System.Drawing.Size(24, 28);
            this.picSelectEMP.TabIndex = 4;
            this.picSelectEMP.TabStop = false;
            this.picSelectEMP.Click += new System.EventHandler(this.picSelectEMP_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(171, 330);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(74, 28);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保 存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDelete.Location = new System.Drawing.Point(76, 330);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(74, 28);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "删除座位";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.BackColor = System.Drawing.Color.Transparent;
            this.lblDepartment.Location = new System.Drawing.Point(110, 200);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(11, 12);
            this.lblDepartment.TabIndex = 0;
            this.lblDepartment.Text = "-";
            // 
            // ttRoom
            // 
            this.ttRoom.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttRoom.ToolTipTitle = "表单有误";
            // 
            // ttSeatCode
            // 
            this.ttSeatCode.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttSeatCode.ToolTipTitle = "表单有误";
            // 
            // ttLocationX
            // 
            this.ttLocationX.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttLocationX.ToolTipTitle = "输入错误";
            // 
            // ttLocationY
            // 
            this.ttLocationY.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttLocationY.ToolTipTitle = "输入错误";
            // 
            // ttIP
            // 
            this.ttIP.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttIP.ToolTipTitle = "输入错误";
            // 
            // ttName
            // 
            this.ttName.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // txtWorkEmail
            // 
            this.txtWorkEmail.Location = new System.Drawing.Point(106, 235);
            this.txtWorkEmail.MaxLength = 50;
            this.txtWorkEmail.Name = "txtWorkEmail";
            this.txtWorkEmail.Size = new System.Drawing.Size(139, 21);
            this.txtWorkEmail.TabIndex = 7;
            // 
            // txtWorkTelephone
            // 
            this.txtWorkTelephone.Location = new System.Drawing.Point(106, 276);
            this.txtWorkTelephone.MaxLength = 20;
            this.txtWorkTelephone.Name = "txtWorkTelephone";
            this.txtWorkTelephone.Size = new System.Drawing.Size(100, 21);
            this.txtWorkTelephone.TabIndex = 9;
            this.txtWorkTelephone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWorkTelephone_KeyPress);
            // 
            // txtMobile
            // 
            this.txtMobile.Location = new System.Drawing.Point(312, 235);
            this.txtMobile.MaxLength = 11;
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(92, 21);
            this.txtMobile.TabIndex = 8;
            this.txtMobile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMobile_KeyPress);
            // 
            // ttWorkEmail
            // 
            this.ttWorkEmail.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttWorkEmail.ToolTipTitle = "输入错误";
            // 
            // frmSeatEdit
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(416, 380);
            this.Controls.Add(this.picSelectEMP);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtLocationY);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.txtLocationX);
            this.Controls.Add(this.txtMobile);
            this.Controls.Add(this.txtWorkTelephone);
            this.Controls.Add(this.txtWorkEmail);
            this.Controls.Add(this.txtSeatCode);
            this.Controls.Add(this.cboEmployeeType);
            this.Controls.Add(this.cboRoom);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblDepartment);
            this.Controls.Add(this.lblTDepartment);
            this.Controls.Add(this.lblTSeatCode);
            this.Controls.Add(this.lblTMobile);
            this.Controls.Add(this.lblTIP);
            this.Controls.Add(this.lblTWorkTelephone);
            this.Controls.Add(this.lblTWorkEmail);
            this.Controls.Add(this.lblLocationY);
            this.Controls.Add(this.lblLocationX);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblTRoom);
            this.Controls.Add(this.lblTName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSeatEdit";
            this.ShowIcon = false;
            this.Text = "员工座位编辑";
            ((System.ComponentModel.ISupportInitialize)(this.picSelectEMP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTName;
        private System.Windows.Forms.Label lblTDepartment;
        private System.Windows.Forms.Label lblTRoom;
        private System.Windows.Forms.Label lblTSeatCode;
        private System.Windows.Forms.Label lblTWorkEmail;
        private System.Windows.Forms.Label lblTWorkTelephone;
        private System.Windows.Forms.Label lblTMobile;
        private System.Windows.Forms.Label lblTIP;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cboRoom;
        private System.Windows.Forms.TextBox txtSeatCode;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblLocationX;
        private System.Windows.Forms.Label lblLocationY;
        private System.Windows.Forms.TextBox txtLocationX;
        private System.Windows.Forms.TextBox txtLocationY;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.ComboBox cboEmployeeType;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.PictureBox picSelectEMP;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.ToolTip ttRoom;
        private System.Windows.Forms.ToolTip ttSeatCode;
        private System.Windows.Forms.ToolTip ttLocationX;
        private System.Windows.Forms.ToolTip ttLocationY;
        private System.Windows.Forms.ToolTip ttIP;
        private System.Windows.Forms.ToolTip ttName;
        private System.Windows.Forms.TextBox txtWorkEmail;
        private System.Windows.Forms.TextBox txtWorkTelephone;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.ToolTip ttWorkEmail;
    }
}