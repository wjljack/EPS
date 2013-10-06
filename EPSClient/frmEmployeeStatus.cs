using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HRSeat.HRSeatServiceReference;
using System.Runtime.InteropServices;
using HRSeat.CommonClass;

namespace HRSeat
{
    public partial class frmEmployeeStatus : Form
    {
        private int _employeeid;
        private Enums.EmployeeType _employeeType;
        private HRSeatClient client;
        private DateTime serverTime;
        public frmEmployeeStatus()
        {
            InitializeComponent();
            CommonFunction.SetButtonStyle(this);
            client = new HRSeatClient();
            CommonClass.FormSet.SetMid(this);
            dtpBegin.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            FillTypeCombobox();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("请选择员工！");
                return;
            }
            try
            {
                Splasher.Show(typeof(frmLoading));
                DateTime beginTime = new DateTime(dtpBegin.Value.Year, dtpBegin.Value.Month, dtpBegin.Value.Day);
                DateTime endTime = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day).AddDays(1);
                dgrdEmployeeStatus.DataSource = client.GetEmployeeStatus(_employeeid, (int)_employeeType, beginTime, endTime, ref serverTime);
                Splasher.Close(this);
            }
            catch /*(System.Exception ex)*/
            {
                Splasher.Close(this);
                this.Close();

            }
            if (!dgrdEmployeeStatus.Columns.Contains("Timespan"))
            {
                dgrdEmployeeStatus.Columns.Add("Timespan", "在线时长");
            }
            CalcTimeSpan();
        }

        /// <summary>
        /// 填充员工类型combobox
        /// </summary>
        private void FillTypeCombobox()
        {
            cboEmployeeType.Items.Clear();
            cboEmployeeType.Items.Add(new cboEmployeeTypeItem(0, "正式员工"));
            cboEmployeeType.Items.Add(new cboEmployeeTypeItem(1, "实习生"));
            cboEmployeeType.Items.Add(new cboEmployeeTypeItem(2, "临时人员"));
            cboEmployeeType.SelectedIndex = 0;
        }
        private void FillTypeCombobox(int typeid)
        {
            FillTypeCombobox();
            cboEmployeeType.SelectedIndex = typeid;
        }

        private void frmEmployeeStatus_Paint(object sender, PaintEventArgs e)
        {
            FormSet.Paint(sender, e);
        }

        private void picSelectEMP_Click(object sender, EventArgs e)
        {
            _employeeType = (Enums.EmployeeType)(cboEmployeeType.SelectedItem as cboEmployeeTypeItem).Id;
            frmEmployeeSelect selectEmployee = new frmEmployeeSelect(Enums.SelectType.AllEmployee, _employeeType);
            selectEmployee.ShowDialog();
            this.Show();
            this.Focus();
            if (selectEmployee.selectEMP != null || selectEmployee.selectEMPTmp != null || selectEmployee.selectIntern != null)
            {

                switch (_employeeType)
                {
                    case Enums.EmployeeType.Employee:
                        txtName.Text = selectEmployee.selectEMP["FullName"].ToString();
                        _employeeid = selectEmployee.selectEMP.ID;
                        break;
                    case Enums.EmployeeType.Intern:
                        txtName.Text = selectEmployee.selectIntern["FullName"].ToString();
                        _employeeid = selectEmployee.selectIntern.ID;
                        break;
                    case Enums.EmployeeType.Employee_temporary:
                        txtName.Text = selectEmployee.selectEMPTmp["FullName"].ToString();
                        _employeeid = selectEmployee.selectEMPTmp.ID;
                        break;
                    default:
                        break;
                }
            }
        }

        private void picSelectEMP_MouseHover(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            ttEmpSelect.Show("", pic, 0);
            ttEmpSelect.Show("选择员工", pic, 0, pic.Height * -1, 2000);
        }

        private class cboEmployeeTypeItem
        {
            public cboEmployeeTypeItem(int id, string name)
            {
                this.Id = id;
                this.Name = name;
            }

            public int Id
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public override string ToString()
            {
                return this.Name;
            }
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
        #endregion

        private void cboEmployeeType_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                //重新选择人员类型后 人员选择框清空
                _employeeType = (Enums.EmployeeType)(cboEmployeeType.SelectedItem as cboEmployeeTypeItem).Id;
                txtName.Text = "";
            }
        }

        private void cboEmployeeType_DropDownClosed(object sender, EventArgs e)
        {
            //重新选择人员类型后 人员选择框清空
            _employeeType = (Enums.EmployeeType)(cboEmployeeType.SelectedItem as cboEmployeeTypeItem).Id;
            txtName.Text = "";
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

        private void dgrdEmployeeStatus_Sorted(object sender, EventArgs e)
        {
            CalcTimeSpan();
        }

        private void CalcTimeSpan()
        {
            foreach (DataGridViewRow dr in dgrdEmployeeStatus.Rows)
            {
                if (dr.Cells["开机时间"].Value != null && dr.Cells["开机时间"].Value.ToString() != "")
                {
                    TimeSpan ts = new TimeSpan();
                    if (dr.Cells["关机时间"].Value != null && dr.Cells["关机时间"].Value.ToString() != "")
                    {
                        ts = ((DateTime)dr.Cells["关机时间"].Value) - (DateTime)dr.Cells["开机时间"].Value;
                    }
                    else
                    {
                        if (dr.Index == 0)
                        {
                            ts = serverTime - (DateTime)dr.Cells["开机时间"].Value;
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
    }
}
