using HRSeat.CommonClass;
using HRSeat.HRSeatServiceReference;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HRSeat
{
    public partial class frmSeatEdit : Form
    {
        private int _employeeid = -1;
        private Enums.EmployeeType _employeeType;
        private frmView _firmview;
        private Size _imageSize;
        private int _seatid = -1;
        private HRSeatClient client;
        private string oldFullName = "";
        private string oldWorkTelephone = "";
        private string oldMobile = "";
        private string oldWorkEmail = "";
        public frmSeatEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 新增座位
        /// </summary>
        /// <param name="frmview">frmView引用</param>
        /// <param name="location">员工位置坐标</param>
        /// <param name="imageSize">图片大小</param>
        public frmSeatEdit(frmView frmview, Point location, Size imageSize)
        {
            InitializeComponent();
            btnDelete.Visible = false;
            //center
            int btnDis = btnClose.Location.X - btnSave.Location.X - btnSave.Width;
            int btnLocX = (this.Width - btnClose.Width - btnSave.Width - btnDis) / 2;
            btnSave.Location = new Point(btnLocX, btnSave.Location.Y);
            btnClose.Location = new Point(btnLocX + btnSave.Width + btnDis, btnClose.Location.Y);
            this.Text = "新建员工座位";
            CommonFunction.SetButtonStyle(this);

            this._firmview = frmview;
            this._imageSize = imageSize;
            CommonClass.FormSet.SetMid(this);

            client = new HRSeatClient();
            foreach (DataSet_HRSeat.Hr_EP_RoomRow roomRow in _firmview.roomTable)
            {
                cboRoomItem cri = new cboRoomItem(roomRow.ID, roomRow.Code, roomRow.Name);
                cboRoom.Items.Add(cri);
                cboRoom.SelectedIndex = 0;
            }

            txtLocationX.Text = location.X.ToString();
            txtLocationY.Text = location.Y.ToString();
            FillTypeCombobox();
        }

        /// <summary>
        /// 修改座位
        /// </summary>
        /// <param name="frmview"></param>
        /// <param name="seatid"></param>
        /// <param name="imageSize"></param>
        public frmSeatEdit(frmView frmview, int seatid, Size imageSize)
        {
            InitializeComponent();

            btnDelete.Visible = true;
            CommonFunction.SetButtonStyle(this);
            this._imageSize = imageSize;
            this._firmview = frmview;
            this._seatid = seatid;
            CommonClass.FormSet.SetMid(this);

            client = new HRSeatClient();
            DataSet_HRSeat.Hr_EP_SeatRow seat = frmview.seatTable.FindByID(seatid);
            _employeeid = seat.EmployeeID;
            _employeeType = (Enums.EmployeeType)seat.EmployeeType;
            //正式员工、实习生
            switch (_employeeType)
            {
                case Enums.EmployeeType.Employee:
                    txtMobile.Text = frmview.employeeTable.FindByID(seat.EmployeeID)["Mobile"].ToString();
                    txtWorkTelephone.Text = frmview.employeeTable.FindByID(seat.EmployeeID)["WorkTelephone"].ToString();
                    txtWorkEmail.Text = frmview.employeeTable.FindByID(seat.EmployeeID)["WorkEmail"].ToString();
                    txtName.Text = frmview.employeeTable.FindByID(seat.EmployeeID)["FullName"].ToString();
                    break;

                case Enums.EmployeeType.Intern:
                    txtMobile.Text = frmview.internTable.FindByID(seat.EmployeeID)["Mobile"].ToString();
                    txtWorkTelephone.Text = frmview.internTable.FindByID(seat.EmployeeID)["HomeTelephone"].ToString();
                    //txtWorkEmail.Text = frmview.internDataTable.FindByID(seat.EmployeeID)["WorkEmail"].ToString();
                    txtName.Text = frmview.internTable.FindByID(seat.EmployeeID)["FullName"].ToString();
                    break;

                case Enums.EmployeeType.Employee_temporary:
                    txtMobile.Text = frmview.employeeTmpTable.FindByID(seat.EmployeeID)["Mobile"].ToString();
                    txtWorkTelephone.Text = frmview.employeeTmpTable.FindByID(seat.EmployeeID)["WorkTelephone"].ToString();
                    txtWorkEmail.Text = frmview.employeeTmpTable.FindByID(seat.EmployeeID)["WorkEmail"].ToString();
                    txtName.Text = frmview.employeeTmpTable.FindByID(seat.EmployeeID)["FullName"].ToString();
                    break;

                default:
                    break;
            }
            txtIP.Text = seat["IP"].ToString();

            DataSet_HRSeat.Hr_EP_RoomRow rr = _firmview.roomTable.FindByID(seat.RoomID);

            foreach (DataSet_HRSeat.Hr_EP_RoomRow roomRow in _firmview.roomTable)
            {
                cboRoomItem cri = new cboRoomItem(roomRow.ID, roomRow.Code, roomRow.Name);
                cboRoom.Items.Add(cri);
                if (roomRow.ID == rr.ID)
                {
                    cboRoom.SelectedItem = cri;
                }
            }

            txtSeatCode.Text = frmview.seatTable.FindByID(seatid)["Code"].ToString();
            txtLocationX.Text = frmview.seatTable.FindByID(seatid)["PosX"].ToString();
            txtLocationY.Text = frmview.seatTable.FindByID(seatid)["PosY"].ToString();
            FillTypeCombobox(seat.EmployeeType);
            ShowDept();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 删除座位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_seatid != -1)
            {
                DialogResult dr = MessageBox.Show("确实要删除吗？", "删除确认", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        if (client.DeleteSeatBySeatID(_seatid))
                        {
                            _firmview.RefreshSeatTable();
                            this.Close();
                            MessageBox.Show("删除成功");
                        }
                    }
                    catch /*(System.Exception ex)*/
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("没有设置位置！");
                return;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckInput())
            {
                return;
            }
            //修改座位
            DataSet_HRSeat.Hr_EP_SeatRow seat = null;
            if (_seatid != -1)
            {
                seat = _firmview.seatTable.FindByID(_seatid);
            }
            //新增座位
            else
            {
                seat = _firmview.seatTable.NewHr_EP_SeatRow();
                seat.OnlineStatus = 0;
            }
            seat.EmployeeID = _employeeid;
            seat.PosX = double.Parse(txtLocationX.Text.Trim());
            seat.PosY = double.Parse(txtLocationY.Text.Trim());
            seat.RoomID = (cboRoom.SelectedItem as cboRoomItem).Id;
            seat.IP = txtIP.Text.Trim();
            seat.EmployeeType = (short)(cboEmployeeType.SelectedItem as cboEmployeeTypeItem).Id;
            seat.Code = txtSeatCode.Text.Trim();

            try
            {
                if (SaveEmployeeData())
                {
                    seat.EmployeeID = (int)client.GetEmployeeTmpNewID();
                }

                if (_seatid == -1)
                {
                    _firmview.seatTable.Rows.Add(seat);
                }
                if (client.UpdateSeats(_firmview.seatTable))
                {
                    _firmview.seatTable.AcceptChanges();
                }
                this.Close();
                MessageBox.Show("保存成功");
            }
            catch /*(System.Exception ex)*/
            {
                this.Close();
            }
        }
        private void cboEmployeeTxtSwitch()
        {
            if (_employeeType == Enums.EmployeeType.Intern)
            {
                txtWorkEmail.Enabled = false;
            }
            else
            {
                txtWorkEmail.Enabled = true;
            }
            if (_employeeType == Enums.EmployeeType.Employee_temporary)
            {
                txtName.ReadOnly = false;
            }
            else
            {
                txtName.ReadOnly = true;
            }
        }

        private void cboEmployeeType_DropDownClosed(object sender, EventArgs e)
        {
            //重新选择人员类型后 人员选择框清空
            _employeeType = (Enums.EmployeeType)(cboEmployeeType.SelectedItem as cboEmployeeTypeItem).Id;
            txtName.Text = "";
            txtWorkEmail.Text = "";
            txtWorkTelephone.Text = "";
            cboEmployeeTxtSwitch();
        }

        private void cboEmployeeType_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                //重新选择人员类型后 人员选择框清空
                _employeeType = (Enums.EmployeeType)(cboEmployeeType.SelectedItem as cboEmployeeTypeItem).Id;
                txtName.Text = "";
                txtWorkEmail.Text = "";
                txtWorkTelephone.Text = "";
                cboEmployeeTxtSwitch();
            }

        }
        private void cboEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboEmployeeTxtSwitch();
        }

        /// <summary>
        /// 表单输入验证
        /// </summary>
        /// <returns>验证状态</returns>
        private bool CheckInput()
        {
            if (txtWorkEmail.Text != "")
            {
                if (!CommonFunction.IsEmail(txtWorkEmail.Text.Trim()))
                {

                    ttWorkEmail.Show("", txtWorkEmail, 0);
                    ttWorkEmail.Show("Email格式错误（例如a@b.com）", txtWorkEmail, 2000);
                    return false;
                }
            }
            if (cboRoom.Text == "")
            {
                //ttUserName.SetToolTip(txtUserName, "请输入帐号后再登录");
                ttRoom.Show("", cboRoom, 0);
                ttRoom.Show("请选择房间", cboRoom, 2000);
                return false;
            }
            if (txtSeatCode.Text == "")
            {
                //ttUserName.SetToolTip(txtUserName, "请输入帐号后再登录");
                ttSeatCode.Show("", txtSeatCode, 0);
                ttSeatCode.Show("请输入座位编号", txtSeatCode, 0, txtSeatCode.Height, 2000);
                return false;
            }
            if (txtLocationX.Text == "")
            {
                //ttUserName.SetToolTip(txtUserName, "请输入帐号后再登录");
                ttLocationX.Show("", txtLocationX, 0);
                ttLocationX.Show("请输入X坐标", txtLocationX, 0, txtLocationX.Height, 2000);
                return false;
            }
            if (txtLocationY.Text == "")
            {
                //ttUserName.SetToolTip(txtUserName, "请输入帐号后再登录");
                ttLocationY.Show("", txtLocationY, 0);
                ttLocationY.Show("请输入Y坐标", txtLocationY, 0, txtLocationX.Height, 2000);
                return false;
            }

            int locationx = int.Parse(txtLocationX.Text.Trim());
            int locationy = int.Parse(txtLocationY.Text.Trim());
            if (locationx < 0 || locationx > _imageSize.Width)
            {
                ttLocationX.Show("", txtLocationX, 0);
                ttLocationX.Show("X坐标超出范围", txtLocationX, 0, txtLocationX.Height, 2000);
                return false;
            }
            if (locationy < 0 || locationy > _imageSize.Height)
            {
                ttLocationY.Show("", txtLocationY, 0);
                ttLocationY.Show("Y坐标超出范围", txtLocationY, 0, txtLocationY.Height, 2000);
                return false;
            }
            //检查附近有无其他人
            DataSet_HRSeat.Hr_EP_SeatDataTable _allSeats = null;
            try
            {
                _allSeats = client.GetAllSeats();
            }
            catch
            {
                MessageBox.Show("网络连接错误");
                this.Close();
            }
            foreach (DataSet_HRSeat.Hr_EP_SeatRow row in _firmview.seatTable)
            {
                if (Math.Sqrt((row.PosX - locationx) * (row.PosX - locationx) + (row.PosY - locationy) * (row.PosY - locationy)) < 10 && row.ID != _seatid)
                {
                    MessageBox.Show("附近有其它座位，请重新设置坐标");
                    txtIP.Focus();
                    return false;
                }
            }
            foreach (DataSet_HRSeat.Hr_EP_SeatRow row in _allSeats)
            {
                if (row.IP == txtIP.Text && row.ID != _seatid)
                {
                    ttIP.Show("", txtIP, 0);
                    ttIP.Show("IP地址已被使用，请重新设置", txtIP, 0, txtIP.Height, 2000);
                    txtIP.Focus();
                    return false;
                }
            }
            string regip = @"\b(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\b";
            if (!Regex.IsMatch(txtIP.Text, regip))
            {
                ttIP.Show("", txtIP, 0);
                ttIP.Show("IP地址格式错误。", txtIP, 0, txtIP.Height, 2000);
                txtIP.Focus();
                return false;
            }
            if (txtName.Text == "")
            {
                ttName.Show("", picSelectEMP, 0);
                ttName.Show("请选择雇员", picSelectEMP, 0, picSelectEMP.Height, 2000);
                picSelectEMP.Focus();
                return false;
            }
            return true;
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

        private void frmSeatEdit_Paint(object sender, PaintEventArgs e)
        {
            FormSet.Paint(sender, e);
        }

        private void picSelectEMP_Click(object sender, EventArgs e)
        {
            frmEmployeeSelect selectEmployee = new frmEmployeeSelect(Enums.SelectType.NoSeatEmployee, _employeeType);
            selectEmployee.ShowDialog();
            this.Show();
            this.Focus();
            if (selectEmployee.selectEMP != null || selectEmployee.selectEMPTmp != null || selectEmployee.selectIntern != null)
            {

                switch (_employeeType)
                {
                    //正式员工
                    case Enums.EmployeeType.Employee:
                        txtMobile.Text = selectEmployee.selectEMP["Mobile"].ToString();
                        txtWorkTelephone.Text = selectEmployee.selectEMP["WorkTelephone"].ToString();
                        txtWorkEmail.Text = selectEmployee.selectEMP["WorkEmail"].ToString();
                        txtName.Text = selectEmployee.selectEMP["FullName"].ToString();
                        _employeeid = selectEmployee.selectEMP.ID;
                        ShowDept();
                        break;

                    //实习生 没有WorkEmail
                    case Enums.EmployeeType.Intern:
                        txtMobile.Text = selectEmployee.selectIntern["Mobile"].ToString();
                        txtWorkTelephone.Text = selectEmployee.selectIntern["HomeTelephone"].ToString();
                        //txtWorkEmail.Text = selectEmployee.selectEMPTmp["WorkEmail"].ToString();
                        txtName.Text = selectEmployee.selectIntern["FullName"].ToString();
                        _employeeid = selectEmployee.selectIntern.ID;
                        //ShowDept();
                        break;

                    //临时人员
                    case Enums.EmployeeType.Employee_temporary:
                        txtMobile.Text = selectEmployee.selectEMPTmp["Mobile"].ToString();
                        txtWorkTelephone.Text = selectEmployee.selectEMPTmp["WorkTelephone"].ToString();
                        txtWorkEmail.Text = selectEmployee.selectEMPTmp["WorkEmail"].ToString();
                        txtName.Text = selectEmployee.selectEMPTmp["FullName"].ToString();
                        _employeeid = selectEmployee.selectEMPTmp.ID;
                        ShowDept();
                        break;
                    default:
                        break;
                }
            }
            //记录变化前的值
            oldWorkTelephone = txtWorkTelephone.Text.Trim();
            oldMobile = txtMobile.Text.Trim();
            oldWorkEmail = txtWorkEmail.Text.Trim();
            oldFullName = txtName.Text.Trim();
        }

        private bool SaveEmployeeData()
        {
            if (oldWorkEmail == txtWorkEmail.Text.Trim() && oldMobile == txtMobile.Text.Trim() && oldWorkTelephone == txtWorkTelephone.Text.Trim() && oldFullName == txtName.Text.Trim())
            {
                return false;
            }
            switch (_employeeType)
            {
                case Enums.EmployeeType.Employee:
                    DataSet_HRSeat.Hr_EmployeeDataTable dtEmployee = client.GetEmployeeByID(_employeeid);
                    dtEmployee[0].WorkEmail = txtWorkEmail.Text.Trim();
                    dtEmployee[0].WorkTelephone = txtWorkTelephone.Text.Trim();
                    dtEmployee[0].Mobile = txtMobile.Text.Trim();
                    client.UpdateEmployee(dtEmployee);
                    return false;
                    break;
                case Enums.EmployeeType.Intern:
                    DataSet_HRSeat.Hr_InternDataTable dtIntern = client.GetInternByID(_employeeid);
                    dtIntern[0].HomeTelephone = txtWorkTelephone.Text.Trim();
                    dtIntern[0].Mobile = txtMobile.Text.Trim();
                    client.UpdateIntern(dtIntern);
                    return false;
                    break;
                case Enums.EmployeeType.Employee_temporary:
                    DataSet_HRSeat.Hr_Employee_TemporaryDataTable dtEmpTmp = client.GetEmployeeTmpByID(_employeeid);
                    if (dtEmpTmp.Count == 1)
                    {
                        dtEmpTmp[0].WorkEmail = txtWorkEmail.Text.Trim();
                        dtEmpTmp[0].WorkTelephone = txtWorkTelephone.Text.Trim();
                        dtEmpTmp[0].Mobile = txtMobile.Text.Trim();
                        dtEmpTmp[0].FullName = txtName.Text.Trim();
                        dtEmpTmp[0].DivisionID = 0;
                        client.UpdateEmployeeTmp(dtEmpTmp);
                        return false;
                    }
                    else
                    {
                        DataSet_HRSeat.Hr_Employee_TemporaryDataTable newDtEmpTmp = new DataSet_HRSeat.Hr_Employee_TemporaryDataTable();
                        DataSet_HRSeat.Hr_Employee_TemporaryRow rowEmpTmp = newDtEmpTmp.NewHr_Employee_TemporaryRow();
                        rowEmpTmp.WorkEmail = txtWorkEmail.Text.Trim();
                        rowEmpTmp.WorkTelephone = txtWorkTelephone.Text.Trim();
                        rowEmpTmp.HomeTelephone = string.Empty;
                        rowEmpTmp.Mobile = txtMobile.Text.Trim();
                        rowEmpTmp.FullName = txtName.Text.Trim();
                        rowEmpTmp.EmployeeCategoryID = 2;
                        rowEmpTmp.DivisionID = 0;
                        rowEmpTmp.JoinedDate = DateTime.Now;
                        newDtEmpTmp.AddHr_Employee_TemporaryRow(rowEmpTmp);
                        client.UpdateEmployeeTmp(newDtEmpTmp);
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }
        }
        private void ShowDept()
        {
            switch (_employeeType)
            {
                case Enums.EmployeeType.Employee:
                    //显示部门
                    DataSet_HRSeat.Hr_Employment_DivisionDataTable divTable = null;
                    try
                    {
                        divTable = client.GetDivisionByEmployeeID(_employeeid);
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

        private void txtWorkTelephone_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunction.InputPhoneNumber(sender, e);
        }

        private void txtIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunction.InputIP(sender, e);
        }

        /// <summary>
        /// 坐标只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLocationX_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunction.InputNumberOnly(sender, e);
        }

        /// <summary>
        /// 坐标只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLocationY_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunction.InputNumberOnly(sender, e);
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunction.InputPhoneNumber(sender, e);
        }

        private void txtSeatCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunction.InputEngNumber(sender, e);
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

        private class cboRoomItem
        {
            public cboRoomItem(int id, string code, string name)
            {
                this.Id = id;
                this.Name = name;
                this.Code = code;
            }

            public string Code
            {
                get;
                set;
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
    }
}