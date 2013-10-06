namespace HRSeatHost
{
    partial class HRSeatHost
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HRSeatHost));
            this.notifyIconServer = new System.Windows.Forms.NotifyIcon(this.components);
            this.cMSServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tSMIAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tSMIExit = new System.Windows.Forms.ToolStripMenuItem();
            this.cMSServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIconServer
            // 
            this.notifyIconServer.BalloonTipText = "正在运行中……";
            this.notifyIconServer.BalloonTipTitle = "EPS服务器";
            this.notifyIconServer.ContextMenuStrip = this.cMSServer;
            this.notifyIconServer.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconServer.Icon")));
            this.notifyIconServer.Text = "EPS服务器";
            this.notifyIconServer.Visible = true;
            // 
            // cMSServer
            // 
            this.cMSServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSMIAbout,
            this.tSMIExit});
            this.cMSServer.Name = "cMSSeat";
            this.cMSServer.Size = new System.Drawing.Size(99, 48);
            this.cMSServer.Text = "座位图";
            // 
            // tSMIAbout
            // 
            this.tSMIAbout.Name = "tSMIAbout";
            this.tSMIAbout.Size = new System.Drawing.Size(98, 22);
            this.tSMIAbout.Text = "关于";
            this.tSMIAbout.Click += new System.EventHandler(this.tSMIAbout_Click);
            // 
            // tSMIExit
            // 
            this.tSMIExit.Name = "tSMIExit";
            this.tSMIExit.Size = new System.Drawing.Size(98, 22);
            this.tSMIExit.Text = "退出";
            this.tSMIExit.Click += new System.EventHandler(this.tSMIExit_Click);
            // 
            // HRSeatHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HRSeatHost";
            this.ShowInTaskbar = false;
            this.Text = "HRSeatHost";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.HRSeatHost_Load);
            this.cMSServer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIconServer;
        private System.Windows.Forms.ContextMenuStrip cMSServer;
        private System.Windows.Forms.ToolStripMenuItem tSMIAbout;
        private System.Windows.Forms.ToolStripMenuItem tSMIExit;
    }
}

