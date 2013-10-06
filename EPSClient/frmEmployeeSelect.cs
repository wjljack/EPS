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
    public partial class frmEmployeeSelect : Form
    {
        //选中的正式员工
        public DataSet_HRSeat.Hr_EmployeeRow selectEMP = null;
        //选中的临时人员
        public DataSet_HRSeat.Hr_Employee_TemporaryRow selectEMPTmp = null;

        //选中的实习生
        public DataSet_HRSeat.Hr_InternRow selectIntern = null;
        private HRSeatClient client;
        private Enums.EmployeeType employeeType = Enums.EmployeeType.Employee;
        private Enums.SelectType selectType;
        private DataSet_HRSeat.DivisionEmployeeDataTable tDivisionEmployee = null;
        private DataSet_HRSeat.Hr_Employee_TemporaryDataTable tEmployeeTmp = null;
        private DataSet_HRSeat.Hr_InternDataTable tIntern = null;
        public frmEmployeeSelect(Enums.SelectType st, Enums.EmployeeType employeeType)
        {
            InitializeComponent();
            CommonFunction.SetButtonStyle(this);
            this.selectType = st;
            this.employeeType = employeeType;
            CommonClass.FormSet.SetMid(this);
        }

       
        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectEMP = null;
            selectEMPTmp = null;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (tvwEmployeeSelect.SelectedNode == null)
            {
                MessageBox.Show("请选择员工");
                return;
            }
            TreeNode selectNode = tvwEmployeeSelect.SelectedNode;
            if (selectNode.Tag.ToString() == "0")
            {
                MessageBox.Show("请选择员工");
                return;
            }
            //    if (lstEmployee.SelectedItems.Count == 0)
            //    {
            //        MessageBox.Show("请选择员工");
            //    }
            //    listBoxItem lbi = lstEmployee.SelectedItems[0] as listBoxItem;

            selectEMP = null;
            selectEMPTmp = null;
            DataSet_HRSeat.Hr_EmployeeDataTable tEmployee = null;
            try
            {
                switch (employeeType)
                {
                    case Enums.EmployeeType.Employee:
                        tEmployee = client.GetEmployeeByID(int.Parse(selectNode.Name));
                        selectEMP = tEmployee[0];
                        break;
                    case Enums.EmployeeType.Intern:
                        selectIntern = tIntern.FindByID(int.Parse(selectNode.Name));
                        break;
                    case Enums.EmployeeType.Employee_temporary:
                        selectEMPTmp = tEmployeeTmp.FindByID(int.Parse(selectNode.Name));
                        break;
                    default:
                        break;
                }
            }
            catch /*(System.Exception ex)*/
            {
                this.Close();
            }
            this.Close();
        }

        private void CreateEmployeeTmpTree()
        {
            DataRow[] drArray = tEmployeeTmp.Select();
            if (drArray.Length == 0) return;
            TreeNode tnRoot = tvwEmployeeSelect.Nodes.Add("0", "临时人员", 0);
            tnRoot.Tag = 0;
            foreach (DataRow dr in drArray)
            {
                TreeNode tnNew = null;
                tnNew = tnRoot.Nodes.Add(dr["ID"].ToString(), dr["FullName"].ToString(), 1, 2);
                tnNew.Tag = 1;
            }
        }

        /// <summary>
        /// 创建员工选择树
        /// </summary>
        private void CreateEmployeeTree()
        {
            DataRow[] drArray = tDivisionEmployee.Select("ParentDivisionId=0",
                                "DivisionID ASC",
                                DataViewRowState.CurrentRows);
            if (drArray.Length == 0) return;

            TreeNode tnNew = null;
            foreach (DataRow dr in drArray)
            {
                if (!tvwEmployeeSelect.Nodes.ContainsKey(dr["DivisionID"].ToString()))
                {
                    tnNew = tvwEmployeeSelect.Nodes.Add(dr["DivisionID"].ToString(), dr["DivisionName"].ToString(), 0);
                    tnNew.Tag = 0;
                    CreateEmployeeTreeNode(ref tnNew);
                }
            }
        }

        /// <summary>
        /// 创建员工选择树节点
        /// </summary>
        /// <param name="tnParent"></param>
        private void CreateEmployeeTreeNode(ref TreeNode tnParent)
        {
            foreach (DataSet_HRSeat.DivisionEmployeeRow row in tDivisionEmployee)
            {
                if (row.DivisionID.ToString() == tnParent.Name && row["EmployeeID"] != DBNull.Value)
                {
                    TreeNode tmpNode = tnParent.Nodes.Add(row["EmployeeID"].ToString(), row["EmployeeName"].ToString(), 1, 2);
                    tmpNode.Tag = 1;
                }
            }
            DataRow[] drArray = tDivisionEmployee.Select(
                string.Format("ParentDivisionId = {0}", tnParent.Name),
                "ParentDivisionId ASC",
                DataViewRowState.CurrentRows);
            if (drArray.Length == 0) return;

            TreeNode tnNew = null;
            foreach (DataRow dr in drArray)
            {
                bool isexist = false;
                foreach (TreeNode t in tnParent.Nodes)
                {
                    if (t.Tag.ToString() == "0" && t.Name == dr["DivisionID"].ToString())
                    {
                        isexist = true;
                    }
                }
                if (!isexist)
                {
                    tnNew = tnParent.Nodes.Add(dr["DivisionID"].ToString(), dr["DivisionName"].ToString(), 0);
                    tnNew.Tag = 0;
                    CreateEmployeeTreeNode(ref tnNew);
                }
            }
        }

        private void CreateInternTree()
        {
            DataRow[] drArray = tIntern.Select();
            if (drArray.Length == 0) return;
            TreeNode tnRoot = tvwEmployeeSelect.Nodes.Add("0", "实习生", 0);
            tnRoot.Tag = 0;
            foreach (DataRow dr in drArray)
            {
                TreeNode tnNew = null;
                tnNew = tnRoot.Nodes.Add(dr["ID"].ToString(), dr["FullName"].ToString(), 1, 2);
                tnNew.Tag = 1;
            }
        }

        private void frmEmployeeSelect_Load(object sender, EventArgs e)
        {
            tvwEmployeeSelect.ImageList = ilEmployeeSelect;
            client = new HRSeatClient();

            //DataSet_HRSeat.Hr_Employee_TemporaryDataTable tEmployeeTmp = null;
            //DataSet_HRSeat.Hr_EmployeeDataTable tEmployee = null;
            try
            {
                //tEmployeeTmp = client.GetEmployeeTmpByNoSeat();
                //tEmployee = client.GetEmployeeByNoSeat();
                switch (selectType)
                {
                    case Enums.SelectType.AllEmployee:
                        {
                            switch (employeeType)
                            {
                                case Enums.EmployeeType.Employee:
                                    tDivisionEmployee = client.GetDivisionEmployee();
                                    break;
                                case Enums.EmployeeType.Intern:
                                    tIntern = client.GetIntern();
                                    break;
                                case Enums.EmployeeType.Employee_temporary:
                                    tEmployeeTmp = client.GetEmployeeTmp();
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case Enums.SelectType.NoSeatEmployee:
                        {
                            switch (employeeType)
                            {
                                case Enums.EmployeeType.Employee:
                                    tDivisionEmployee = client.GetDivisionEmployeeNoSeat();
                                    break;
                                case Enums.EmployeeType.Intern:
                                    tIntern = client.GetInternByNoSeat();
                                    break;
                                case Enums.EmployeeType.Employee_temporary:
                                    tEmployeeTmp = client.GetEmployeeTmpByNoSeat();
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch /*(System.Exception ex)*/
            {
                this.Close();
            }
            switch (employeeType)
            {
                case Enums.EmployeeType.Employee:
                    CreateEmployeeTree();
                    break;
                case Enums.EmployeeType.Intern:
                    CreateInternTree();
                    break;
                case Enums.EmployeeType.Employee_temporary:
                    CreateEmployeeTmpTree();
                    break;
                default:
                    break;
            }
            if (tvwEmployeeSelect.Nodes.Count != 0)
            {
                tvwEmployeeSelect.Nodes[0].Expand();
            }
            //foreach (DataSet_HRSeat.Hr_Employee_TemporaryRow rEmployeeTmp in tEmployeeTmp)
            //{
            //    lstEmployee.Items.Add(new listBoxItem(rEmployeeTmp.ID, rEmployeeTmp.Code, rEmployeeTmp.FullName));
            //}
            //foreach (DataSet_HRSeat.Hr_EmployeeRow rEmployee in tEmployee)
            //{
            //    lstEmployee.Items.Add(new listBoxItem(rEmployee.ID, rEmployee.Code, rEmployee.FullName));
            //}
        }

        private void frmEmployeeSelect_Paint(object sender, PaintEventArgs e)
        {
            FormSet.Paint(sender, e);
        }

        private void lstEmployee_DoubleClick(object sender, EventArgs e)
        {
            btnSelect.PerformClick();
        }

        private void tvwEmployeeSelect_DoubleClick(object sender, EventArgs e)
        {
            TreeView tv = sender as TreeView;
            if (tv.SelectedNode != null && tv.SelectedNode.Tag.ToString() == "0")
            {
                return;
            }
            btnSelect.PerformClick();
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
        class listBoxItem
        {
            public listBoxItem(int id, string code, string name)
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
                return this.Code + " " + this.Name;
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
        #region 窗体边框阴影效果变量申明
        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        #endregion
    }
}
