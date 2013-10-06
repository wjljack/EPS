namespace HRSeat
{
    partial class frmSeatHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeatHistory));
            this.dgrdSeatHistory = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.lblDays = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgrdSeatHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // dgrdSeatHistory
            // 
            this.dgrdSeatHistory.AllowUserToAddRows = false;
            this.dgrdSeatHistory.AllowUserToResizeRows = false;
            this.dgrdSeatHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgrdSeatHistory.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgrdSeatHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrdSeatHistory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgrdSeatHistory.Location = new System.Drawing.Point(12, 12);
            this.dgrdSeatHistory.Name = "dgrdSeatHistory";
            this.dgrdSeatHistory.ReadOnly = true;
            this.dgrdSeatHistory.RowHeadersVisible = false;
            this.dgrdSeatHistory.RowTemplate.Height = 23;
            this.dgrdSeatHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrdSeatHistory.Size = new System.Drawing.Size(591, 332);
            this.dgrdSeatHistory.TabIndex = 0;
            this.dgrdSeatHistory.Sorted += new System.EventHandler(this.dgrdSeatHistory_Sorted);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(272, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 28);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关 闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnQuery.Location = new System.Drawing.Point(166, 360);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(74, 28);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtDays
            // 
            this.txtDays.Location = new System.Drawing.Point(107, 365);
            this.txtDays.MaxLength = 3;
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(28, 21);
            this.txtDays.TabIndex = 4;
            this.txtDays.Text = "7";
            this.txtDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDays_KeyPress);
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = true;
            this.lblDays.Location = new System.Drawing.Point(12, 368);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(89, 12);
            this.lblDays.TabIndex = 5;
            this.lblDays.Text = "查询最近天数：";
            // 
            // frmSeatHistory
            // 
            this.AcceptButton = this.btnQuery;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(615, 400);
            this.Controls.Add(this.lblDays);
            this.Controls.Add(this.txtDays);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgrdSeatHistory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSeatHistory";
            this.ShowIcon = false;
            this.Text = "员工登录历史记录";
            this.Load += new System.EventHandler(this.frmSeatHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgrdSeatHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgrdSeatHistory;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.TextBox txtDays;
        private System.Windows.Forms.Label lblDays;
    }
}