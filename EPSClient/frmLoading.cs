using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HRSeat.CommonClass;
using System.Threading;
namespace HRSeat
{
    public partial class frmLoading : Form, ISplashForm
    {
        public frmLoading()
        {
            InitializeComponent();
            try
            {
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                FormSet.SetMid(this);
                this.BackColor = Color.LimeGreen;
                this.TransparencyKey = Color.LimeGreen;
            }
            catch
            {

            }
        }
        void ISplashForm.SetStatusInfo(string NewStatusInfo)
        {
            //lbStatusInfo.Text = NewStatusInfo;//lbStatusInfo为窗体上用来显示信息的Label控件
        }

        private void frmLoading_Load(object sender, EventArgs e)
        {

            Thread.Sleep(250);
            picLoading.Visible = true;

        }
        #region 内存优化
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet =
System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        #endregion
    }
}
