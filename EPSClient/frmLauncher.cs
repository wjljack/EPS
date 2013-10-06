using HRSeat.CommonClass;
using HRSeat.HRSeatServiceReference;
using MySchedule;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HRSeat
{
    public partial class frmLauncher : Form
    {
        public Enums.OnlineType OnlineStatus;
        private string _ipAddress;
        private int _employeeid;
        private string _username;
        private string _password;
        private string _employeename;
        private Enums.EmployeeType _employeeType;
        private bool _isadmin;
        private HRSeatClient client;
        private int comboFloorIndex = 0;
        private frmView fv;
        private System.Timers.Timer heartbeatTimer = null;
        private IPEndPoint ipep;
        private bool isLogout = false;
        private HRSeat.HRSeatServiceReference.LoginReturnStatus lrs;
        private Socket udpClientSocket = null;
        private bool showOffline = false;
        private ThreadStart loginTS = null;
        private Thread loginThread = null;
        private FormMain frmOA = null;
        private Icon[] launcherIcons;
        private IniFile _iniFile;
        private int offlineCount = 0;
        private byte[] udpData = null;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="employeeid">员工id</param>
        /// <param name="isadmin">是否管理员</param>
        /// <param name="iswrongseat">是否坐在错误的位置</param>
        /// <param name="fl">登录form引用</param>
        public frmLauncher(int employeeid, string username, string password, Enums.EmployeeType employeetype, bool isadmin, bool iswrongseat)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.Hide();
            this._employeeid = employeeid;
            this._employeeType = employeetype;
            this._username = username;
            this._password = password;
            this._isadmin = isadmin;
            this._ipAddress = CommonFunction.GetIPAddress();
            this._iniFile = new IniFile("config.ini");
            launcherIcons = new Icon[]{
                     new Icon(@"Images\online.ico"),
                    new Icon(@"Images\online2.ico"),
                    new Icon(@"Images\offline.ico")
            };
            client = new HRSeatClient();
            try
            {
                if (_isadmin)
                {
                    this._employeename = client.GetEmployeeNameByIDEmpType(employeeid, (int)employeetype) + "(管理员)";
                }
                else
                {
                    this._employeename = client.GetEmployeeNameByIDEmpType(employeeid, (int)employeetype);
                }
            }
            catch /*(System.Exception ex)*/
            {
                this.Close();
            }
            if (iswrongseat)
            {
                SetOnlineStatus(Enums.OnlineType.Online2);
            }
            else
            {
                SetOnlineStatus(Enums.OnlineType.Online);
            }
            SetLauncherMenu();

            StartHeartBeat();

            // Initialize IPEndPoint for loopback network adapter on Port 10000.
            int heartbeatPort = 0;
            try
            {
                heartbeatPort = int.Parse(CommonFunction.GetAppConfig("ServerHeartBeatPort"));
            }
            catch
            {
                MessageBox.Show("端口配置错误！");
                Environment.Exit(0);
            }
            try
            {
                ipep = new IPEndPoint(IPAddress.Parse(CommonFunction.GetAppConfig("ServerIP")), heartbeatPort);
            }
            catch
            {
                MessageBox.Show("IP地址配置错误！");
                Environment.Exit(0);
            }
        }

        public int ComboFloorIndex
        {
            get { return comboFloorIndex; }
            set { comboFloorIndex = value; }
        }

        public void ReLoginThread()
        {
            loginTS = new ThreadStart(ReLogin);
            loginThread = new Thread(loginTS);
            loginThread.Start();
        }
        /// <summary>
        /// 在线状态枚举
        /// </summary>
        /// <summary>
        /// 掉线重新登录
        /// </summary>
        public void ReLogin()
        {
            _ipAddress = CommonFunction.GetIPAddress();
            //string username = inifile.ReadString("LOCAL_CONFIG", "UserName", null);
            //string password = inifile.ReadString("LOCAL_CONFIG", "Password", null);
            try
            {
                lrs = client.UserLogin(_username, _password, _ipAddress, out _isadmin);
            }
            catch /*(System.Exception ex)*/
            {
                if (!showOffline)
                {
                    MessageBox.Show("网络连接失败！");
                }
                StopHeartBeat();
                SetOnlineStatus(Enums.OnlineType.Offline);
            }
            switch (lrs)
            {
                case LoginReturnStatus.OK:
                    //登录成功！
                    SetOnlineStatus(Enums.OnlineType.Online);
                    break;

                case LoginReturnStatus.PWDERROR:
                    //密码错误！
                    return;

                case LoginReturnStatus.ALREADYOK:
                    //已经登录，请不要重复登录
                    SetOnlineStatus(Enums.OnlineType.Online);
                    break;

                case LoginReturnStatus.NOUSER:
                    //没有此用户，登录失败！
                    return;

                case LoginReturnStatus.WRONGSEAT:
                    //您坐在其他人的座位上
                    SetOnlineStatus(Enums.OnlineType.Online2);
                    break;

                case LoginReturnStatus.NOUSERSEAT:
                    //没有分配您的座位，只能查看其他人的座位情况
                    SetOnlineStatus(Enums.OnlineType.Online2);
                    break;

                case LoginReturnStatus.ADMIN:
                    //管理员登录
                    SetOnlineStatus(Enums.OnlineType.Online);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 设置托盘的在线状态
        /// </summary>
        /// <param name="onlineType"></param>
        public void SetOnlineStatus(Enums.OnlineType onlineType)
        {
            //设置统一在线状态
            switch (onlineType)
            {
                case Enums.OnlineType.Online:
                case Enums.OnlineType.Online2:
                    notifyIconSeat.Text = "HR座位管理系统-" + _employeename + "(在线)";
                    break;
                case Enums.OnlineType.Offline:
                case Enums.OnlineType.ForceOffline:
                    notifyIconSeat.Text = "HR座位管理系统-" + _employeename + "(离线)";
                    break;
                default:
                    break;
            }
            //已经掉线则返回
            if (this.OnlineStatus == Enums.OnlineType.ForceOffline && onlineType == Enums.OnlineType.Offline)
            {
                return;
            }
            else if (this.OnlineStatus == Enums.OnlineType.Offline && onlineType == Enums.OnlineType.ForceOffline)
            {
                return;
            }
            //已经点击注销则返回
            else if (isLogout)
            {
                return;
            }
            //状态没有发生变化 返回
            else if (this.OnlineStatus == onlineType && onlineType != Enums.OnlineType.Offline)
            {
                return;
            }
            //更新当前在线状态
            this.OnlineStatus = onlineType;
            switch (onlineType)
            {
                case Enums.OnlineType.Online:
                    notifyIconSeat.Icon = launcherIcons[0];
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(new SetMenuEnableCallback(SetMenuEnable), true);
                    }
                    StartHeartBeat();
                    break;

                case Enums.OnlineType.Online2:
                    notifyIconSeat.Icon = launcherIcons[1];
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(new SetMenuEnableCallback(SetMenuEnable), true);
                    }
                    StartHeartBeat();
                    break;

                case Enums.OnlineType.Offline:
                    CloseExistForm();
                    StopHeartBeat();
                    notifyIconSeat.Icon = launcherIcons[2];
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(new SetMenuEnableCallback(SetMenuEnable), false);
                    }
                    if (!showOffline)
                    {
                        showOffline = true;
                        DialogResult dr = MessageBox.Show("已经掉线，是否重新登录？", "HR座位管理系统", MessageBoxButtons.YesNo);
                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {
                            showOffline = false;
                            ReLoginThread();
                        }
                        else
                        {
                            showOffline = false;
                            Exit();
                        }
                    }
                    break;

                case Enums.OnlineType.ForceOffline:
                    CloseExistForm();
                    StopHeartBeat();
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(new SetMenuEnableCallback(SetMenuEnable), false);
                    }
                    notifyIconSeat.Icon = launcherIcons[2];
                    if (!showOffline)
                    {
                        showOffline = true;
                        DialogResult drForce = MessageBox.Show("您已经在其他机器登录或IP地址配置改变，是否重新登录？", "HR座位管理系统", MessageBoxButtons.YesNo);
                        if (drForce == System.Windows.Forms.DialogResult.Yes)
                        {
                            ReLoginThread();
                            showOffline = false;
                        }
                        else
                        {
                            showOffline = false;
                            Exit();
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void Exit()
        {
            if (notifyIconSeat != null)
            {
                notifyIconSeat.Dispose();
            }
            SaveFloorConfig();
            Environment.Exit(0);
        }

        private void frmLauncher_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            notifyIconSeat.ShowBalloonTip(5000);
            int firstShowStatus = _iniFile.ReadInt("LOCAL_CONFIG", "FirstShow", 0);
            bool bShowOA = _iniFile.ReadBool("LOCAL_CONFIG", "ShowOA", false);
            ComboFloorIndex = _iniFile.ReadInt("LOCAL_CONFIG", "cboFloorIndex", 0);

            if (firstShowStatus == 0 || firstShowStatus == 2)
            {
                ShowfrmView(false);
            }
            if (bShowOA)
            {
                Splasher.Show(typeof(frmLoading));
                frmOA = new MySchedule.FormMain(_username);
                try
                {
                    frmOA.Show();
                }
                catch { }
                Splasher.Close(this);
            }
        }

        /// <summary>
        /// 心跳检查掉线委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (OnlineStatus == Enums.OnlineType.Offline || OnlineStatus == Enums.OnlineType.ForceOffline)
            {
                return;
            }
            SendUdpPacket();
        }

        private void Logout()
        {
            DialogResult dr = MessageBox.Show("确实要注销吗？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            isLogout = true;
            //inifile.WriteString("LOCAL_CONFIG", "UserName", "");
            _iniFile.WriteBool("LOCAL_CONFIG", "IsRemeberPWD", false);
            _iniFile.WriteBool("LOCAL_CONFIG", "IsAutoStart", false);
            _iniFile.WriteInt("LOCAL_CONFIG", "FirstShow", 0);
            _iniFile.WriteString("LOCAL_CONFIG", "Password", "");
            _iniFile.WriteString("LOCAL_CONFIG", "cboFloorIndex", "0");

            try
            {
                client.UserLogout(_ipAddress);
                CommonFunction.SetAutoRun(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, false);
            }
            catch /*(System.Exception ex)*/
            {
            }
            Restart();
        }
        private void notifyIconSeat_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (OnlineStatus == Enums.OnlineType.Offline || OnlineStatus == Enums.OnlineType.ForceOffline)
                {
                    return;
                }
                ShowfrmView(false);
            }
        }


        private void OpenHR()
        {
            string encodeUsername = CommonClass.Encryption.Encrypt(_username, "jfsys");
            string encodePassword = CommonClass.Encryption.Encrypt(_password, "jfsys");
            string URL = CommonFunction.GetAppConfig("HROpenURL");
            ShellExecute(IntPtr.Zero, "open", URL + "?name=" + encodeUsername + "&password=" + encodePassword, "", "", Enums.ShowCommands.SW_SHOWNOACTIVATE);
        }

        private void Restart()
        {
            try
            {
                //run the program again and close this one
                Process.Start(Application.StartupPath + "\\EPSClient.exe");
                //or you can use Application.ExecutablePath

                //close this one
                Process.GetCurrentProcess().Kill();
            }
            catch
            { }
        }

        private void SaveFloorConfig()
        {
            _iniFile.WriteInt("LOCAL_CONFIG", "cboFloorIndex", ComboFloorIndex);
        }

        /// <summary>
        /// 发送UDP心跳包
        /// </summary>
        //private bool changeSend = true;
        private void SendUdpPacket()
        {
            try
            {
                //正向心跳包
                //创建UDP Socket
                if (udpClientSocket == null)
                {
                    udpClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    udpClientSocket.SendTimeout = 5000;
                }
                // 将ip地址作为数据发送
                if (udpData == null)
                {
                    udpData = Encoding.ASCII.GetBytes(_ipAddress);
                }
                // 发送心跳包
                udpClientSocket.SendTo(udpData, 0, udpData.Length, SocketFlags.None, ipep);





                //逆向wcf心跳检测
                if (OnlineStatus != Enums.OnlineType.ForceOffline && OnlineStatus != Enums.OnlineType.Offline)
                {
                    int status = client.CheckOnline(_ipAddress, (int)OnlineStatus);
                    switch (status)
                    {
                        case 0:
                            SetOnlineStatus(Enums.OnlineType.Offline);
                            break;
                        case 2:
                            SetOnlineStatus(Enums.OnlineType.ForceOffline);
                            break;
                        default:
                            break;
                    }
                }
                offlineCount = 0;
            }
            catch /*(System.Exception ex)*/
            {
                if (OnlineStatus != Enums.OnlineType.Offline)
                {
                    //OnlineStatus = Enums.OnlineType.Offline;
                    if (offlineCount > 4)
                    {
                        SetOnlineStatus(Enums.OnlineType.Offline);
                    }
                    offlineCount++;
                }
            }
        }

        /// <summary>
        /// 根据登陆用户决定菜单是否可用
        /// </summary>
        private void SetLauncherMenu()
        {
            if (!_isadmin)
            {
                tSMIManager.Enabled = false;
            }
            if (_employeeType == Enums.EmployeeType.Employee_temporary || _employeeType == Enums.EmployeeType.Intern)
            {

                tSMIHR.Enabled = false;
                tSMICalendar.Enabled = false;
            }
        }
        /// <summary>
        /// 进入查看楼层界面
        /// </summary>
        private void ShowfrmView(bool adminView)
        {
            if (fv == null || fv.IsDisposed)
            {
                try
                {
                    fv = new frmView(this, _employeeid, _employeeType, _isadmin, adminView);
                    fv.Show();
                }
                catch
                {
                }
            }
            else
            {
                if (adminView)
                {
                    fv.tctlMain.SelectedIndex = 1;
                }
            }
        }

        private void StartHeartBeat()
        {
            //登陆成功 开始心跳前终止登陆线程
            if (loginThread != null)
            {
                loginThread.Abort();
                loginThread = null;
            }
            heartbeatTimer = new System.Timers.Timer();
            //心跳频率
            heartbeatTimer.Interval = 3000;
            heartbeatTimer.AutoReset = true;
            heartbeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(heartbeatTimer_Elapsed);
            heartbeatTimer.Start();
        }

        private void StopHeartBeat()
        {
            if (heartbeatTimer != null)
            {
                heartbeatTimer.Dispose();
                heartbeatTimer = null;
            }
        }
        public void DisposeFrmView()
        {
            if (fv != null && (!fv.IsDisposed))
            {
                fv.Dispose();
                fv = null;
            }
            GC.Collect();
        }

        private void tSMIAbout_Click(object sender, EventArgs e)
        {
            frmAbout fa = new frmAbout();
            fa.ShowDialog();
            fa.Dispose();
        }

        private void tSMICalendar_Click(object sender, EventArgs e)
        {
            if (frmOA == null || frmOA.IsDisposed)
            {
                Splasher.Show(typeof(frmLoading));
                frmOA = new MySchedule.FormMain(_username);
                try
                {
                    frmOA.Show();
                }
                catch { }
                Splasher.Close(this);
            }
        }

        private void tSMIExit_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否退出？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            try
            {
                client.UserLogout(_ipAddress);
            }
            catch /*(System.Exception ex)*/
            {
            }
            Exit();
        }

        private void tSMIHR_Click(object sender, EventArgs e)
        {
            OpenHR();
        }

        private void tSMILogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void tSMIManager_Click(object sender, EventArgs e)
        {
            if (OnlineStatus == Enums.OnlineType.Offline || OnlineStatus == Enums.OnlineType.ForceOffline)
            {
                return;
            }
            ShowfrmView(true);
        }

        private void tSMISeat_Click(object sender, EventArgs e)
        {
            if (OnlineStatus == Enums.OnlineType.Offline || OnlineStatus == Enums.OnlineType.ForceOffline)
            {
                return;
            }
            ShowfrmView(false);
        }

        private void CloseExistForm()
        {
            try
            {
                if (fv != null)
                {
                    fv.Close();
                    fv = null;
                }
                if (frmOA != null)
                {
                    frmOA.Close();
                    frmOA = null;
                }
            }
            catch
            {
            }
        }

        #region Open HR



        [DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            Enums.ShowCommands nShowCmd);

        #endregion Open HR

        private void tSMISettings_Click(object sender, EventArgs e)
        {
            frmSettings fs = new frmSettings();
            fs.ShowDialog();
            fs.Dispose();
        }
        private delegate void SetMenuEnableCallback(bool isOnline);
        private void SetMenuEnable(bool isOnline)
        {
            tSMICalendar.Enabled = isOnline;
            tSMIHR.Enabled = isOnline;
            tSMIManager.Enabled = isOnline;
            tSMISeat.Enabled = isOnline;
            if (isOnline)
            {
                SetLauncherMenu();
            }
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