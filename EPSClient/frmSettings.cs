﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HRSeat.CommonClass;
using System.Runtime.InteropServices;
using System.Reflection;
using log4net;

namespace HRSeat
{
    public partial class frmSettings : Form
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IniFile inifile;
        public frmSettings()
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

        private void frmSettings_Load(object sender, EventArgs e)
        {
            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW);
            LoadConfig();
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
        private void frmSettings_Paint(object sender, PaintEventArgs e)
        {
            FormSet.Paint(sender, e);
        }
        private void LoadConfig()
        {
            string configPath = Application.StartupPath + "\\config.ini";
            inifile = new IniFile(configPath);
            chkAutoStart.Checked = inifile.ReadBool("LOCAL_CONFIG", "IsAutoStart", false);
            int firstShow = inifile.ReadInt("LOCAL_CONFIG", "FirstShow", 0);
            if (firstShow == 1)
            {
                chkSeat.Checked = false;
            }
            else
            {
                chkSeat.Checked = true;
            }
            chkOA.Checked = inifile.ReadBool("LOCAL_CONFIG", "ShowOA", true);
        }

        private void SaveConfig()
        {
            try
            {
                if (chkAutoStart.Checked)
                {
                    CommonFunction.SetAutoRun(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, true);
                }
                else
                {
                    CommonFunction.SetAutoRun(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, false);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                chkAutoStart.Checked = inifile.ReadBool("LOCAL_CONFIG", "IsAutoStart", false);
                MessageBox.Show("设置自动启动失败！");
                return;
            }
            inifile.WriteBool("LOCAL_CONFIG", "IsAutoStart", chkAutoStart.Checked);
            if (inifile.KeyExists("LOCAL_CONFIG", "FirstShow"))
            {
                inifile.WriteInt("LOCAL_CONFIG", "FirstShow", 0);
            }
            if (chkSeat.Checked)
            {
                inifile.WriteInt("LOCAL_CONFIG", "FirstShow", 2);
            }
            else
            {
                inifile.WriteInt("LOCAL_CONFIG", "FirstShow", 1);
            }
            if (chkOA.Checked)
            {
                inifile.WriteBool("LOCAL_CONFIG", "ShowOA", true);
            }
            else
            {
                inifile.WriteBool("LOCAL_CONFIG", "ShowOA", false);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
            this.Close();
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
