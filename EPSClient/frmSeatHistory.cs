using HRSeat.CommonClass;
using HRSeat.HRSeatServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HRSeat
{
    public partial class frmSeatHistory : Form
    {
        private string _ip;
        private HRSeatClient client;
        private DateTime serverTime;

        public frmSeatHistory(string ip)
        {
            InitializeComponent();
            CommonFunction.SetButtonStyle(this);
            CommonClass.FormSet.SetMid(this);
            _ip = ip;
            client = new HRSeatClient();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSeatHistory_Load(object sender, EventArgs e)
        {
            uint days = uint.Parse(txtDays.Text);
            Query(days);
        }

        #region 移动窗体

        private const int HTCAPTION = 0x0002;

        private const int HTCLIENT = 0x1;

        //表示鼠标在窗口客户区的系统消息
        private const int SC_MAXIMIZE = 0xF030;

        //最大化信息
        private const int SC_MINIMIZE = 0xF020;

        private const int SC_MOVE = 0xF010;

        private const int WM_MOVING = 0x216;

        //鼠标移动消息
        //移动信息
        //表示鼠标在窗口标题栏时的系统信息
        private const int WM_NCHITTEST = 0x84;

        private const int WM_SYSCOMMAND = 0x0112;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
            base.OnMouseMove(e);
        }

        //点击窗口左上角那个图标时的系统信息
        //鼠标在窗体客户区（除了标题栏和边框以外的部分）时发送的消息
        //最小化信息
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

        #endregion 移动窗体

        private void frmSeatHistory_Paint(object sender, PaintEventArgs e)
        {
            FormSet.Paint(sender, e);
        }
        private void Query(uint recentDay)
        {
            //Splasher.Show(typeof(frmLoading));
            try
            {
                dgrdSeatHistory.DataSource = client.GetSeatHistoryByIPDay(_ip, recentDay, ref serverTime);
            }
            catch /*(System.Exception ex)*/
            {
                //Splasher.Close(this);
                this.Close();
            }
            if (!dgrdSeatHistory.Columns.Contains("Timespan"))
            {
                dgrdSeatHistory.Columns.Add("Timespan", "在线时长");
            }
            if (!dgrdSeatHistory.Columns.Contains("Name"))
            {
                dgrdSeatHistory.Columns.Add("Name", "姓名");
            }
            dgrdSeatHistory.Columns["Name"].DisplayIndex = 0;
            dgrdSeatHistory.Columns["SeatID"].Visible = false;
            dgrdSeatHistory.Columns["ID"].Visible = false;
            dgrdSeatHistory.Columns["EmployeeID"].Visible = false;
            dgrdSeatHistory.Columns["EmployeeType"].Visible = false;
            dgrdSeatHistory.Columns["OnLineTime"].HeaderText = "登录时间";
            dgrdSeatHistory.Columns["OfflineTime"].HeaderText = "下线时间";
            dgrdSeatHistory.Columns["IP"].Visible = false;
            ShowTimeSpanAndName();
            //Splasher.Close(this);
        }

        private void ShowTimeSpanAndName()
        {
            Dictionary<string, string> empNameDic = new Dictionary<string, string>();
            foreach (DataGridViewRow dr in dgrdSeatHistory.Rows)
            {
                //dickey是通过雇员id和雇员类型构造查询姓名的词典缓存
                string dicKey = dr.Cells["EmployeeID"].Value.ToString() + "|" + dr.Cells["EmployeeType"].Value.ToString();
                if (!empNameDic.ContainsKey(dicKey))
                {
                    empNameDic[dicKey] = client.GetEmployeeNameByIDEmpType((int)dr.Cells["EmployeeID"].Value, (int)dr.Cells["EmployeeType"].Value);
                }
                dr.Cells["Name"].Value = empNameDic[dicKey];
                if (dr.Cells["OnLineTime"].Value != null && dr.Cells["OnLineTime"].Value.ToString() != "")
                {
                    TimeSpan ts = new TimeSpan();
                    if (dr.Cells["OfflineTime"].Value != null && dr.Cells["OfflineTime"].Value.ToString() != "")
                    {
                        ts = ((DateTime)dr.Cells["OfflineTime"].Value) - (DateTime)dr.Cells["OnlineTime"].Value;
                    }
                    else
                    {
                        if (dr.Index == 0)
                        {
                            ts = serverTime - (DateTime)dr.Cells["OnlineTime"].Value;
                            if (ts.TotalSeconds < 0)
                            {
                                ts = new TimeSpan(0, 0, 0);
                            }
                        }
                    }
                    string timestr = "";
                    if (ts.Days != 0)
                    {
                        timestr += ts.Days.ToString() + "天";
                    }
                    if (ts.Hours != 0)
                    {
                        timestr += ts.Hours.ToString() + "小时";
                    }
                    if (ts.Minutes != 0)
                    {
                        timestr += ts.Minutes.ToString() + "分钟";
                    }
                    if (ts.Seconds != 0)
                    {
                        timestr += ts.Seconds.ToString() + "秒";
                    }
                    dr.Cells["Timespan"].Value = timestr;
                }
            }
        }

        private void txtDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunction.InputNumberOnly(sender, e);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            uint days = uint.Parse(txtDays.Text);
            Query(days);
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

        private void dgrdSeatHistory_Sorted(object sender, EventArgs e)
        {
            ShowTimeSpanAndName();
        }
    }
}