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
    public partial class frmSeatView : Form
    {
        private HRSeatClient client;
        private string _seatIP;
        private int _employeeid;
        private Enums.EmployeeType _employeeType;
        public frmSeatView()
        {
            InitializeComponent();
        }
        public frmSeatView(frmView frmview, int seatid, int employeeid)
        {
            InitializeComponent();
            CommonFunction.SetButtonStyle(this);
            CommonClass.FormSet.SetMid(this);
            AddLabelFunction();


            client = new HRSeatClient();
            this._employeeid = employeeid;



            DataSet_HRSeat.Hr_EP_SeatRow seat = frmview.seatTable.FindByID(seatid);
            _employeeType = (Enums.EmployeeType)seat.EmployeeType;


            if (seat["EmployeeID"].ToString() != _employeeid.ToString() && frmview._isadmin == false)
            {
                llblTViewHistory.Visible = false;
            }
            switch (_employeeType)
            {
                case Enums.EmployeeType.Employee:
                    DataSet_HRSeat.Hr_EmployeeRow emp = frmview.employeeTable.FindByID(seat.EmployeeID);
                    lblMobile.Text = emp["Mobile"].ToString();
                    lblWorkTelephone.Text = emp["WorkTelephone"].ToString();
                    lblWorkEmail.Text = emp["WorkEmail"].ToString();
                    lblName.Text = emp["FullName"].ToString();
                    break;
                case Enums.EmployeeType.Intern:
                    DataSet_HRSeat.Hr_InternRow intern = frmview.internTable.FindByID(seat.EmployeeID);
                    lblMobile.Text = intern["Mobile"].ToString();
                    lblWorkTelephone.Text = intern["HomeTelephone"].ToString();
                    lblName.Text = intern["FullName"].ToString();
                    break;
                case Enums.EmployeeType.Employee_temporary:
                    DataSet_HRSeat.Hr_Employee_TemporaryRow emptmp = frmview.employeeTmpTable.FindByID(seat.EmployeeID);
                    lblMobile.Text = emptmp["Mobile"].ToString();
                    lblWorkTelephone.Text = emptmp["WorkTelephone"].ToString();
                    lblWorkEmail.Text = emptmp["WorkEmail"].ToString();
                    lblName.Text = emptmp["FullName"].ToString();
                    break;
                default:
                    break;
            }


            ShowDept(seat);

            _seatIP = seat["IP"].ToString();
            lblIP.Text = seat["IP"].ToString();
            try
            {
                lblRoom.Text = client.GetRoomByID(seat.RoomID)[0]["Name"].ToString() + "(" + client.GetRoomByID(seat.RoomID)[0]["Code"].ToString() + ")";
            }
            catch /*(System.Exception ex)*/
            {
                this.Close();
            }
            lblSeatCode.Text = seat["Code"].ToString();
        }

        private void ShowDept(DataSet_HRSeat.Hr_EP_SeatRow seat)
        {
            switch (_employeeType)
            {
                case Enums.EmployeeType.Employee:
                    //显示部门
                    DataSet_HRSeat.Hr_Employment_DivisionDataTable divTable = null;
                    try
                    {
                        divTable = client.GetDivisionByEmployeeID(seat.EmployeeID);
                    }
                    catch /*(System.Exception ex)*/
                    {
                        this.Close();
                    }
                    if (divTable != null && divTable.Count != 0)
                    {
                        lblDepartment.Text = divTable[0].Name;
                    }
                    else
                    {
                        lblDepartment.Text = "未指定部门";
                    }
                    //--显示部门
                    break;
                case Enums.EmployeeType.Intern:
                    lblDepartment.Text = "实习生";
                    break;
                case Enums.EmployeeType.Employee_temporary:
                    lblDepartment.Text = "临时人员";
                    break;
                default:
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void llblViewHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSeatHistory fsh = new frmSeatHistory(_seatIP);
            fsh.ShowDialog();
        }

        private void frmSeatView_Paint(object sender, PaintEventArgs e)
        {
            FormSet.Paint(sender, e);
        }

        private void ClickLabelToClipboard(object sender, EventArgs e)
        {
            Label thislbl = (sender as Label);
            if ((sender as Label).Text != "-")
            {
                Clipboard.SetDataObject(thislbl.Text);
                ttLabel.Show("", thislbl, 0);
                ttLabel.Show("已复制", thislbl, 0, thislbl.Height * -2, 2000);
            }

        }
        private void LabelMouseHover(object sender, EventArgs e)
        {
            Label l = sender as Label;
            l.BackColor = Color.Aquamarine;
            ttLabel.Show("", l, 0);
            ttLabel.Show("点击复制到剪贴板", l, 0, l.Height * -2, 2000);
        }
        private void LabelMouseLeave(object sender, EventArgs e)
        {
            Label l = sender as Label;
            l.BackColor = Color.Transparent;
            ttLabel.Show("", l, 0);
        }

        private void AddLabelFunction()
        {
            foreach (Control c in this.Controls)
            {
                if (c is Label)
                {
                    Label l = c as Label;
                    if (l.Name.IndexOf("lblT") == -1)
                    {
                        l.Cursor = Cursors.Hand;
                        l.Click += new EventHandler(ClickLabelToClipboard);
                        l.MouseLeave += new EventHandler(LabelMouseLeave);
                        l.MouseHover += new EventHandler(LabelMouseHover);
                    }
                }
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
