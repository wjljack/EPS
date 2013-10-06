namespace HRSeat
{
    partial class frmEmployeeStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmployeeStatus));
            this.lblTimespan = new System.Windows.Forms.Label();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.lblDTP = new System.Windows.Forms.Label();
            this.lblEmployeeName = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.picSelectEMP = new System.Windows.Forms.PictureBox();
            this.dgrdEmployeeStatus = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.ttEmpSelect = new System.Windows.Forms.ToolTip(this.components);
            this.cboEmployeeType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectEMP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgrdEmployeeStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTimespan
            // 
            this.lblTimespan.AutoSize = true;
            this.lblTimespan.BackColor = System.Drawing.Color.Transparent;
            this.lblTimespan.Location = new System.Drawing.Point(12, 31);
            this.lblTimespan.Name = "lblTimespan";
            this.lblTimespan.Size = new System.Drawing.Size(41, 12);
            this.lblTimespan.TabIndex = 0;
            this.lblTimespan.Text = "时间：";
            // 
            // dtpBegin
            // 
            this.dtpBegin.Location = new System.Drawing.Point(62, 27);
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Size = new System.Drawing.Size(105, 21);
            this.dtpBegin.TabIndex = 2;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(198, 27);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(105, 21);
            this.dtpEnd.TabIndex = 2;
            // 
            // lblDTP
            // 
            this.lblDTP.AutoSize = true;
            this.lblDTP.BackColor = System.Drawing.Color.Transparent;
            this.lblDTP.Location = new System.Drawing.Point(175, 31);
            this.lblDTP.Name = "lblDTP";
            this.lblDTP.Size = new System.Drawing.Size(17, 12);
            this.lblDTP.TabIndex = 0;
            this.lblDTP.Text = "－";
            // 
            // lblEmployeeName
            // 
            this.lblEmployeeName.AutoSize = true;
            this.lblEmployeeName.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployeeName.Location = new System.Drawing.Point(331, 30);
            this.lblEmployeeName.Name = "lblEmployeeName";
            this.lblEmployeeName.Size = new System.Drawing.Size(65, 12);
            this.lblEmployeeName.TabIndex = 0;
            this.lblEmployeeName.Text = "员工姓名：";
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnQuery.Location = new System.Drawing.Point(653, 23);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(74, 28);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(403, 27);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(73, 21);
            this.txtName.TabIndex = 4;
            // 
            // picSelectEMP
            // 
            this.picSelectEMP.BackColor = System.Drawing.Color.Transparent;
            this.picSelectEMP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSelectEMP.Image = global::HRSeat.Properties.Resources.online;
            this.picSelectEMP.Location = new System.Drawing.Point(609, 23);
            this.picSelectEMP.Name = "picSelectEMP";
            this.picSelectEMP.Size = new System.Drawing.Size(24, 28);
            this.picSelectEMP.TabIndex = 5;
            this.picSelectEMP.TabStop = false;
            this.picSelectEMP.Click += new System.EventHandler(this.picSelectEMP_Click);
            this.picSelectEMP.MouseHover += new System.EventHandler(this.picSelectEMP_MouseHover);
            // 
            // dgrdEmployeeStatus
            // 
            this.dgrdEmployeeStatus.AllowUserToAddRows = false;
            this.dgrdEmployeeStatus.AllowUserToResizeRows = false;
            this.dgrdEmployeeStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgrdEmployeeStatus.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgrdEmployeeStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrdEmployeeStatus.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgrdEmployeeStatus.Location = new System.Drawing.Point(12, 74);
            this.dgrdEmployeeStatus.Name = "dgrdEmployeeStatus";
            this.dgrdEmployeeStatus.ReadOnly = true;
            this.dgrdEmployeeStatus.RowHeadersVisible = false;
            this.dgrdEmployeeStatus.RowTemplate.Height = 23;
            this.dgrdEmployeeStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrdEmployeeStatus.Size = new System.Drawing.Size(715, 362);
            this.dgrdEmployeeStatus.TabIndex = 6;
            this.dgrdEmployeeStatus.Sorted += new System.EventHandler(this.dgrdEmployeeStatus_Sorted);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(332, 454);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 28);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关 闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cboEmployeeType
            // 
            this.cboEmployeeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEmployeeType.FormattingEnabled = true;
            this.cboEmployeeType.Location = new System.Drawing.Point(496, 27);
            this.cboEmployeeType.Name = "cboEmployeeType";
            this.cboEmployeeType.Size = new System.Drawing.Size(100, 20);
            this.cboEmployeeType.TabIndex = 7;
            this.cboEmployeeType.DropDownClosed += new System.EventHandler(this.cboEmployeeType_DropDownClosed);
            this.cboEmployeeType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cboEmployeeType_KeyUp);
            // 
            // frmEmployeeStatus
            // 
            this.AcceptButton = this.btnQuery;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(739, 498);
            this.Controls.Add(this.cboEmployeeType);
            this.Controls.Add(this.dgrdEmployeeStatus);
            this.Controls.Add(this.picSelectEMP);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpBegin);
            this.Controls.Add(this.lblDTP);
            this.Controls.Add(this.lblEmployeeName);
            this.Controls.Add(this.lblTimespan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEmployeeStatus";
            this.ShowIcon = false;
            this.Text = "员工历史记录查询";
            ((System.ComponentModel.ISupportInitialize)(this.picSelectEMP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgrdEmployeeStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTimespan;
        private System.Windows.Forms.DateTimePicker dtpBegin;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label lblDTP;
        private System.Windows.Forms.Label lblEmployeeName;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.PictureBox picSelectEMP;
        private System.Windows.Forms.DataGridView dgrdEmployeeStatus;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip ttEmpSelect;
        private System.Windows.Forms.ComboBox cboEmployeeType;
    }
}