namespace HRSeat
{
    partial class frmLauncher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLauncher));
            this.notifyIconSeat = new System.Windows.Forms.NotifyIcon(this.components);
            this.cMSSeat = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tSMISeat = new System.Windows.Forms.ToolStripMenuItem();
            this.tSMICalendar = new System.Windows.Forms.ToolStripMenuItem();
            this.tSMIHR = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tSMISettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tSMILogout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tSMIManager = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tSMIAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tSMIExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ttLauncher = new System.Windows.Forms.ToolTip(this.components);
            this.cMSSeat.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIconSeat
            // 
            this.notifyIconSeat.BalloonTipText = "正在运行";
            this.notifyIconSeat.BalloonTipTitle = "HR座位管理系统";
            this.notifyIconSeat.ContextMenuStrip = this.cMSSeat;
            this.notifyIconSeat.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconSeat.Icon")));
            this.notifyIconSeat.Text = "HR座位管理系统(在线)";
            this.notifyIconSeat.Visible = true;
            this.notifyIconSeat.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconSeat_MouseClick);
            // 
            // cMSSeat
            // 
            this.cMSSeat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSMISeat,
            this.tSMICalendar,
            this.tSMIHR,
            this.toolStripSeparator1,
            this.tSMISettings,
            this.tSMILogout,
            this.toolStripSeparator2,
            this.tSMIManager,
            this.toolStripSeparator3,
            this.tSMIAbout,
            this.tSMIExit});
            this.cMSSeat.Name = "cMSSeat";
            this.cMSSeat.Size = new System.Drawing.Size(141, 198);
            this.cMSSeat.Text = "座位图";
            // 
            // tSMISeat
            // 
            this.tSMISeat.Name = "tSMISeat";
            this.tSMISeat.Size = new System.Drawing.Size(140, 22);
            this.tSMISeat.Text = "座位图";
            this.tSMISeat.Click += new System.EventHandler(this.tSMISeat_Click);
            // 
            // tSMICalendar
            // 
            this.tSMICalendar.Name = "tSMICalendar";
            this.tSMICalendar.Size = new System.Drawing.Size(140, 22);
            this.tSMICalendar.Text = "我的OA日程";
            this.tSMICalendar.Click += new System.EventHandler(this.tSMICalendar_Click);
            // 
            // tSMIHR
            // 
            this.tSMIHR.Name = "tSMIHR";
            this.tSMIHR.Size = new System.Drawing.Size(140, 22);
            this.tSMIHR.Text = "进入HR系统";
            this.tSMIHR.Click += new System.EventHandler(this.tSMIHR_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(137, 6);
            // 
            // tSMISettings
            // 
            this.tSMISettings.Name = "tSMISettings";
            this.tSMISettings.Size = new System.Drawing.Size(140, 22);
            this.tSMISettings.Text = "系统设置";
            this.tSMISettings.Click += new System.EventHandler(this.tSMISettings_Click);
            // 
            // tSMILogout
            // 
            this.tSMILogout.Name = "tSMILogout";
            this.tSMILogout.Size = new System.Drawing.Size(140, 22);
            this.tSMILogout.Text = "注销";
            this.tSMILogout.Click += new System.EventHandler(this.tSMILogout_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(137, 6);
            // 
            // tSMIManager
            // 
            this.tSMIManager.Name = "tSMIManager";
            this.tSMIManager.Size = new System.Drawing.Size(140, 22);
            this.tSMIManager.Text = "管理设置";
            this.tSMIManager.Click += new System.EventHandler(this.tSMIManager_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(137, 6);
            // 
            // tSMIAbout
            // 
            this.tSMIAbout.Name = "tSMIAbout";
            this.tSMIAbout.Size = new System.Drawing.Size(140, 22);
            this.tSMIAbout.Text = "关于";
            this.tSMIAbout.Click += new System.EventHandler(this.tSMIAbout_Click);
            // 
            // tSMIExit
            // 
            this.tSMIExit.Name = "tSMIExit";
            this.tSMIExit.Size = new System.Drawing.Size(140, 22);
            this.tSMIExit.Text = "退出";
            this.tSMIExit.Click += new System.EventHandler(this.tSMIExit_Click);
            // 
            // ttLauncher
            // 
            this.ttLauncher.IsBalloon = true;
            this.ttLauncher.ToolTipTitle = "HR员工座位图分布系统";
            // 
            // frmLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "frmLauncher";
            this.ShowInTaskbar = false;
            this.Text = "frmLauncher";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.frmLauncher_Load);
            this.cMSSeat.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cMSSeat;
        private System.Windows.Forms.ToolStripMenuItem tSMISeat;
        private System.Windows.Forms.ToolStripMenuItem tSMIHR;
        private System.Windows.Forms.ToolStripMenuItem tSMILogout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tSMIManager;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tSMIAbout;
        private System.Windows.Forms.ToolStripMenuItem tSMIExit;
        public System.Windows.Forms.NotifyIcon notifyIconSeat;
        private System.Windows.Forms.ToolTip ttLauncher;
        private System.Windows.Forms.ToolStripMenuItem tSMICalendar;
        private System.Windows.Forms.ToolStripMenuItem tSMISettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}