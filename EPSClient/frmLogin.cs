using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using HRSeat.CommonClass;
using System.Drawing.Drawing2D;
using HRSeat.HRSeatServiceReference;
using System.Net;
using System.Threading;
using DWMSCrpyt;
using System.Reflection;
using log4net;
namespace HRSeat
{
    public partial class frmLogin : Form
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //登陆名
        public string _username;
        //登录密码
        public string _password;
        //本地保存密码用非对称加密密钥
        private const string encryptionKey = "jfsys"; //密匙
        private IniFile inifile;
        private HRSeatClient client;
        private int _employeeid;
        private Enums.EmployeeType _employeetype;
        private frmLauncher frml = null;
        private HRSeat.HRSeatServiceReference.LoginReturnStatus lrs;
        private bool _isadmin = false;
        private bool _isAnonymous = false;
        private ThreadStart loginTS = null;
        private Thread loginThread = null;
        public frmLogin()
        {
            InitializeComponent();
            SetButtonStyle();
        }
        private void ChangeLoginLayout(bool isAnonymous)
        {
            _isAnonymous = isAnonymous;
            if (isAnonymous)
            {
                lblUserName.Text = "姓名：";
                lblPassword.Enabled = false;
                txtPassword.Enabled = false; ;
                chkRemeberPWD.Visible = false;
            }
            else
            {
                lblUserName.Text = "帐号：";
                lblPassword.Enabled = true;
                txtPassword.Enabled = true; ;
                chkRemeberPWD.Visible = true;
            }
        }
        private void SetButtonStyle()
        {
            this.btnLogin.MouseDown += new MouseEventHandler(CommonFunction.ButtonDown);
            this.btnLogin.MouseHover += new EventHandler(CommonFunction.ButtonOver);
            this.btnLogin.MouseLeave += new EventHandler(CommonFunction.ButtonLeave);
            this.btnLogin.MouseUp += new MouseEventHandler(CommonFunction.ButtonLeave);

            this.btnCancel.MouseDown += new MouseEventHandler(CommonFunction.ButtonDown);
            this.btnCancel.MouseHover += new EventHandler(CommonFunction.ButtonOver);
            this.btnCancel.MouseLeave += new EventHandler(CommonFunction.ButtonLeave);
            this.btnCancel.MouseUp += new MouseEventHandler(CommonFunction.ButtonLeave);

            this.btnOK.MouseDown += new MouseEventHandler(CommonFunction.ButtonDown);
            this.btnOK.MouseHover += new EventHandler(CommonFunction.ButtonOver);
            this.btnOK.MouseLeave += new EventHandler(CommonFunction.ButtonLeave);
            this.btnOK.MouseUp += new MouseEventHandler(CommonFunction.ButtonLeave);
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string configPath = Application.StartupPath + "\\config.ini";
            inifile = new IniFile(configPath);
            SetIP();
            CommonClass.FormSet.SetMid(this);
            //FormSet.AnimateWindow(this.Handle, 800, FormSet.AW_BLEND);
            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW);
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormSet.AnimateWindow(this.Handle, 400, FormSet.AW_SLIDE | FormSet.AW_HIDE | FormSet.AW_BLEND);
            //Environment.Exit(0);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void frmLogin_Paint(object sender, PaintEventArgs e)
        {
            if (pnlErrorMsg.Visible)
            {
                return;
            }
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(76, 155, 196), 1.0f);
            foreach (Control ctr in this.Controls)
            {
                if (ctr is TextBox)
                {
                    g.DrawRectangle(pen, new Rectangle(new Point(ctr.Location.X - 1, ctr.Location.Y - 1), new Size(ctr.Size.Width + 1, ctr.Height + 1)));
                }
            }
            pen.Dispose();
            FormSet.Paint(sender, e);
        }

        private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (sender as CheckBox);
            try
            {
                if (chk.Checked)
                {
                    chkRemeberPWD.Checked = true;
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
            }
        }
        public delegate void Adelegate();
        private void LoginRegular()
        {
            _employeetype = 0;
            Splasher.Show(typeof(frmLoading));

            SaveConfig();


            client = new HRSeatClient();


            try
            {
                lrs = client.UserLogin(txtUserName.Text.Trim(), txtPassword.Text.Trim(), CommonFunction.GetIPAddress(), out _isadmin);
                _employeeid = client.GetEmployeeIDByUserName(txtUserName.Text.Trim());
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message, ex);
                Splasher.Close(this);
                MessageBox.Show("网络连接失败！");
                this.Visible = true;
                return;
            }
            bool iswrongseat = false;
            switch (lrs)
            {
                case LoginReturnStatus.OK:
                    //登录成功！
                    break;
                case LoginReturnStatus.PWDERROR:
                    pnlErrorMsg.Visible = true;
                    Splasher.Close(this);
                    return;
                case LoginReturnStatus.ALREADYOK:
                    //已经登录，请不要重复登录
                    break;
                case LoginReturnStatus.NOUSER:
                    //没有此用户，登录失败！
                    pnlErrorMsg.Visible = true;
                    Splasher.Close(this);
                    return;
                case LoginReturnStatus.WRONGSEAT:
                    //您坐在其他人的座位上
                    iswrongseat = true;
                    break;
                case LoginReturnStatus.NOUSERSEAT:
                    Adelegate dgt = new Adelegate(() =>
                    {
                        MessageBox.Show("没有分配您的座位，请联系管理员分配！");
                    });
                    dgt.BeginInvoke(null, null);
                    iswrongseat = true;
                    break;
                default:
                    break;
            }
            if (frml == null)
            {
                _username = txtUserName.Text.Trim();
                _password = txtPassword.Text.Trim();
            }
            Splasher.Close(this);
            Thread threadLauncher = new Thread(() => Application.Run(new frmLauncher(_employeeid, _username, _password, _employeetype, _isadmin, iswrongseat)));
            threadLauncher.SetApartmentState(ApartmentState.STA);
            threadLauncher.Start();
            this.Close();

        }
        private void LoginAnonymous()
        {
            Splasher.Show(typeof(frmLoading));

            SaveConfig();


            client = new HRSeatClient();


            try
            {
                lrs = client.UserLoginAnonymous(CommonFunction.GetIPAddress(), txtUserName.Text.Trim());
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message, ex);
                Splasher.Close(this);
                MessageBox.Show("网络连接失败！");
                this.Visible = true;
                return;
            }
            bool iswrongseat = false;
            switch (lrs)
            {
                case LoginReturnStatus.OK:
                    //登录成功！
                    break;
                case LoginReturnStatus.ALREADYOK:
                    //已经登录，请不要重复登录
                    break;
                case LoginReturnStatus.NOUSER:
                    Splasher.Close(this);
                    MessageBox.Show("没有此用户，登录失败！");
                    return;
                case LoginReturnStatus.WRONGSEAT:
                    //您坐在其他人的座位上
                    iswrongseat = true;
                    break;
                case LoginReturnStatus.NOUSERSEAT:
                    Splasher.Close(this);
                    MessageBox.Show("没有此用户，登录失败！");
                    return;
                default:
                    break;
            }
            //this.Close();
            //frmLoading.CloseForm();
            DataSet_HRSeat.Hr_EP_SeatDataTable _seatTable = null;
            try
            {
                _seatTable = client.GetSeatByIP(CommonFunction.GetIPAddress());
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message, ex);
                Splasher.Close(this);
                MessageBox.Show("网络连接失败！");
                this.Visible = true;
                return;
            }

            _employeetype = (Enums.EmployeeType)_seatTable[0].EmployeeType;
            _employeeid = _seatTable[0].EmployeeID;
            if (frml == null)
            {
                _username = txtUserName.Text.Trim();
                _password = txtPassword.Text.Trim();
            }

            Splasher.Close(this);
            Thread threadLauncher = new Thread(() => Application.Run(new frmLauncher(_employeeid, _username, _password, _employeetype, _isadmin, iswrongseat)));
            threadLauncher.SetApartmentState(ApartmentState.STA);
            threadLauncher.Start();
            this.Close();
        }
        private void LoginThread()
        {
            loginTS = new ThreadStart(Login);
            loginThread = new Thread(loginTS);
            loginThread.Start();
        }
        private void Login()
        {
            if (!CheckInput(_isAnonymous))
            {
                return;
            }
            if (_isAnonymous)
            {
                LoginAnonymous();
            }
            else
            {
                LoginRegular();
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private bool CheckInput(bool isAnonymous)
        {
            if (isAnonymous)
            {

                if (txtUserName.Text.Trim() == "")
                {

                    ttUserName.Show("", txtUserName, 0);
                    ttUserName.Show("请输入姓名后再登录", txtUserName, 0, txtUserName.Height, 2000);

                    return false;
                }
                return true;
            }
            else
            {
                if (txtUserName.Text.Trim() == "")
                {

                    ttUserName.Show("", txtUserName, 0);
                    ttUserName.Show("请输入帐号后再登录", txtUserName, 0, txtUserName.Height, 2000);

                    return false;
                }
                if (txtPassword.Text.Trim() == "")
                {
                    ttPassword.Show("", txtPassword, 0);
                    ttPassword.Show("请输入密码后再登录", txtPassword, 0, txtPassword.Height, 2000);
                    return false;
                }
                return true;
            }
        }
        private void SetIP()
        {
            IPAddress[] iplist = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            int ipcount = iplist.Length;
            if (ipcount > 1)
            {
                llblSelectIP.Visible = true;
            }
            else if (ipcount == 0)
            {
                MessageBox.Show("无网络连接！");
                Environment.Exit(0);
            }
            string savedIP = inifile.ReadString("LOCAL_CONFIG", "IP", "");
            if (ipcount > 1)
            {
                bool isexist = false;
                for (int i = 0; i < ipcount; i++)
                {
                    if (iplist[i].ToString() == savedIP)
                    {
                        isexist = true;
                        break;
                    }
                }
                //如果设置的ip不在本地ip地址列表里面 则需要重新设置
                if (!isexist)
                {
                    frmIPSet fipset = new frmIPSet();
                    fipset.ShowDialog();
                }
            }
            else if (ipcount == 1)
            {
                if (iplist[0].ToString() != savedIP)
                {
                    inifile.WriteString("LOCAL_CONFIG", "IP", iplist[0].ToString());
                }
            }
        }
        /// <summary>
        /// 本地保存登录数据
        /// </summary>
        private void SaveConfig()
        {
            if (inifile.KeyExists("LOCAL_CONFIG", "FirstShow"))
            {
                inifile.WriteInt("LOCAL_CONFIG", "FirstShow", 0);
            }
            inifile.WriteString("LOCAL_CONFIG", "UserName", txtUserName.Text.Trim());
            inifile.WriteBool("LOCAL_CONFIG", "IsAnonymous", chkAnonymous.Checked);
            inifile.WriteBool("LOCAL_CONFIG", "IsRemeberPWD", chkRemeberPWD.Checked);
            inifile.WriteBool("LOCAL_CONFIG", "IsAutoStart", chkAutoStart.Checked);
            if (chkRemeberPWD.Checked)
            {
                inifile.WriteString("LOCAL_CONFIG", "Password", Encryption.Encrypt(txtPassword.Text.Trim(), encryptionKey));
            }
        }
        /// <summary>
        /// 载入本地保存的登录数据
        /// </summary>
        private void LoadConfig()
        {
            txtUserName.Text = inifile.ReadString("LOCAL_CONFIG", "UserName", null);
            chkAnonymous.Checked = inifile.ReadBool("LOCAL_CONFIG", "IsAnonymous", false);
            chkRemeberPWD.Checked = inifile.ReadBool("LOCAL_CONFIG", "IsRemeberPWD", false);
            chkAutoStart.Checked = inifile.ReadBool("LOCAL_CONFIG", "IsAutoStart", false);
            if (chkRemeberPWD.Checked)
            {
                txtPassword.Text = Encryption.Decrypt(inifile.ReadString("LOCAL_CONFIG", "Password", null), encryptionKey);
            }
            if (chkAutoStart.Checked)
            {
                this.Visible = false;
                Login();
                //this.Hide();
                //btnLogin.PerformClick();
            }
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
            if (this.Visible == false)
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
        #region 窗体边框阴影效果变量申明
        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        #endregion

        //private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    char kc = e.KeyChar;
        //    if ((kc < 48 || kc > 57) && kc != 8)
        //        e.Handled = true;
        //}

        private void btnOK_Click(object sender, EventArgs e)
        {
            pnlErrorMsg.Visible = false;
        }

        private void llblSelectIP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmIPSet fipset = new frmIPSet();
            fipset.ShowDialog();
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!chkAnonymous.Checked)
            {
                CommonFunction.InputEngNumber(sender, e);
            }
            if ((sender as TextBox).Text.Length == 1)
            {
                txtPassword.Text = "";
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunction.InputPWD(sender, e);
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void chkAnonymous_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ChangeLoginLayout(chk.Checked);
        }
        //Memory Optimise
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
        //
    }
}
