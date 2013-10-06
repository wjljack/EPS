using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using HRSeat.CommonClass;

namespace HRSeat
{
    public partial class frmIPSet : Form
    {
        private IniFile inifile;
        public frmIPSet()
        {
            InitializeComponent();
            CommonFunction.SetButtonStyle(this);
            CommonClass.FormSet.SetMid(this);
        }
        #region 移动窗体
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private const int WM_SYSCOMMAND = 0x0112;//点击窗口左上角那个图标时的系统信息
        private const int WM_MOVING = 0x216;//鼠标移动消息
        private const int SC_MOVE = 0xF010;//移动信息
        private const int HTCAPTION = 0x0002;//表示鼠标在窗口标题栏时的系统信息
        private const int WM_NCHITTEST = 0x84;//鼠标在窗体客户区（除了标题栏和边框以外的部分）时发送的消息
        private const int HTCLIENT = 0x1;//表示鼠标在窗口客户区的系统消息
        private const int SC_MAXIMIZE = 0xF030;//最大化信息
        private const int SC_MINIMIZE = 0xF020;//最小化信息
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_MOVING: //如果鼠标移                  
                    base.WndProc(ref m);//调用基类的窗口过程——WndProc方法处理这个消息
                    if (m.Result == (IntPtr)HTCLIENT)//如果返回的是HTCLIENT
                    {
                        m.Result = (IntPtr)HTCAPTION;//把它改为HTCAPTION
                        return;//直接返回退出方法
                    }
                    break;
            }
            base.WndProc(ref m);//如果不是鼠标移动或单击消息就调用基类的窗口过程进行处理
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
            base.OnMouseMove(e);
        }
        #endregion

        private void frmIPSet_Load(object sender, EventArgs e)
        {
            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW);

            inifile = new IniFile("config.ini");
            IPAddress[] ips = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            string readip = inifile.ReadString("LOCAL_CONFIG", "IP", "");
            for (int i = 0; i < ips.Length; i++)
            {
                int index = lstIP.Items.Add(ips[i].ToString());
                if (ips[i].ToString() == readip)
                {
                    lstIP.SelectedItem = lstIP.Items[index];
                }
            }
            if (lstIP.Items.Count != 0 && lstIP.SelectedItems.Count == 0)
            {
                lstIP.SelectedItem = lstIP.Items[0];
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstIP.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择使用的IP地址");
                return;
            }
            inifile.WriteString("LOCAL_CONFIG", "IP", lstIP.SelectedItems[0].ToString());
            this.Close();
        }
        private void lstIP_DoubleClick(object sender, EventArgs e)
        {
            btnOK.PerformClick();
        }
        #region 窗体边框阴影效果变量申明
        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        #endregion

        private void frmIPSet_Paint(object sender, PaintEventArgs e)
        {
            FormSet.Paint(sender, e);
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