using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HRSeat.HRSeatServiceReference;
using System.IO;
using HRSeat.CommonClass;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Runtime.Serialization;
using System.Reflection;
using log4net;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
namespace HRSeat
{
    public partial class frmView : Form
    {
        #region SerializableDictionary
        [Serializable()]
        public class SerializableDictionary<TKey, TVal> : Dictionary<TKey, TVal>, IXmlSerializable, ISerializable
        {
            #region Constants
            private const string DictionaryNodeName = "Dictionary";
            private const string ItemNodeName = "Item";
            private const string KeyNodeName = "Key";
            private const string ValueNodeName = "Value";
            #endregion
            #region Constructors
            public SerializableDictionary()
            {
            }

            public SerializableDictionary(IDictionary<TKey, TVal> dictionary)
                : base(dictionary)
            {
            }

            public SerializableDictionary(IEqualityComparer<TKey> comparer)
                : base(comparer)
            {
            }

            public SerializableDictionary(int capacity)
                : base(capacity)
            {
            }

            public SerializableDictionary(IDictionary<TKey, TVal> dictionary, IEqualityComparer<TKey> comparer)
                : base(dictionary, comparer)
            {
            }

            public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
                : base(capacity, comparer)
            {
            }

            #endregion
            #region ISerializable Members

            protected SerializableDictionary(SerializationInfo info, StreamingContext context)
            {
                int itemCount = info.GetInt32("ItemCount");
                for (int i = 0; i < itemCount; i++)
                {
                    KeyValuePair<TKey, TVal> kvp = (KeyValuePair<TKey, TVal>)info.GetValue(String.Format("Item{0}", i), typeof(KeyValuePair<TKey, TVal>));
                    this.Add(kvp.Key, kvp.Value);
                }
            }

            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("ItemCount", this.Count);
                int itemIdx = 0;
                foreach (KeyValuePair<TKey, TVal> kvp in this)
                {
                    info.AddValue(String.Format("Item{0}", itemIdx), kvp, typeof(KeyValuePair<TKey, TVal>));
                    itemIdx++;
                }
            }

            #endregion
            #region IXmlSerializable Members

            void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
            {
                //writer.WriteStartElement(DictionaryNodeName);
                foreach (KeyValuePair<TKey, TVal> kvp in this)
                {
                    writer.WriteStartElement(ItemNodeName);
                    writer.WriteStartElement(KeyNodeName);
                    KeySerializer.Serialize(writer, kvp.Key);
                    writer.WriteEndElement();
                    writer.WriteStartElement(ValueNodeName);
                    ValueSerializer.Serialize(writer, kvp.Value);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                //writer.WriteEndElement();
            }

            void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
            {
                if (reader.IsEmptyElement)
                {
                    return;
                }

                // Move past container
                if (!reader.Read())
                {
                    throw new XmlException("Error in Deserialization of Dictionary");
                }

                //reader.ReadStartElement(DictionaryNodeName);
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    reader.ReadStartElement(ItemNodeName);
                    reader.ReadStartElement(KeyNodeName);
                    TKey key = (TKey)KeySerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadStartElement(ValueNodeName);
                    TVal value = (TVal)ValueSerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadEndElement();
                    this.Add(key, value);
                    reader.MoveToContent();
                }
                //reader.ReadEndElement();

                reader.ReadEndElement(); // Read End Element to close Read of containing node
            }

            System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
            {
                return null;
            }

            #endregion
            #region Private Properties
            protected XmlSerializer ValueSerializer
            {
                get
                {
                    if (valueSerializer == null)
                    {
                        valueSerializer = new XmlSerializer(typeof(TVal));
                    }
                    return valueSerializer;
                }
            }

            private XmlSerializer KeySerializer
            {
                get
                {
                    if (keySerializer == null)
                    {
                        keySerializer = new XmlSerializer(typeof(TKey));
                    }
                    return keySerializer;
                }
            }
            #endregion
            #region Private Members
            private XmlSerializer keySerializer = null;
            private XmlSerializer valueSerializer = null;
            #endregion
        }
        private SerializableDictionary<int, byte[]> dicFloorImages = new SerializableDictionary<int, byte[]>();

        #endregion
        private ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        frmLauncher _fl = null;
        private HRSeatClient client;
        private int seatPicSizeX = 50;
        private int seatPicSizeY = 40;
        private System.Timers.Timer RefreshTimer = new System.Timers.Timer();
        public DataSet_HRSeat.Hr_EP_FloorDataTable floorTable;
        public DataSet_HRSeat.Hr_EP_RoomDataTable roomTable;
        public DataSet_HRSeat.Hr_EP_SeatDataTable seatTable;
        public DataSet_HRSeat.Hr_EmployeeDataTable employeeTable = null;
        public DataSet_HRSeat.Hr_Employee_TemporaryDataTable employeeTmpTable = null;
        public DataSet_HRSeat.Hr_InternDataTable internTable = null;
        public bool _isadmin;
        public bool _adminView;
        private Bitmap myNewCursor = null;
        private Cursor handCursor = null;
        private Graphics gCursor = null;
        private int _employeeid;
        private Enums.EmployeeType _employeeType;
        private string _employeename;
        //0 新增 1查看 2编辑
        /// <summary>
        /// 楼层编辑状态
        /// </summary>
        private enum FloorEditMode
        {
            New,
            View,
            Edit
        }
        private FloorEditMode floorEditMode;
        private int floorEditFloorID;
        /// <summary>
        /// 员工在线状态
        /// </summary>
        public enum OnlineType
        {
            Online,
            Online2,
            Offline
        }
        public frmView(frmLauncher fl, int employeeid, Enums.EmployeeType employeetype, bool isadmin, bool adminview)
        {
            this._isadmin = isadmin;
            this._employeeid = employeeid;
            this._employeeType = employeetype;
            this._adminView = adminview;
            this._fl = fl;
            InitializeComponent();
            //禁用选择楼层Combobox
            cboFloor.MouseWheel += new MouseEventHandler(cboFloor_MouseWheel);
            CommonFunction.SetButtonStyle(this);
            CommonFunction.SetComboBoxStyle(cboFloor);

            if (!isadmin)
            {
                tctlMain.TabPages.Remove(tpgSettings);
                picHisQuery.Enabled = false;
                picHisQuery.Image = HRSeat.Properties.Resources.UserInline_diable;
            }

        }




        //禁用选择楼层Combobox
        private void cboFloor_MouseWheel(object sender, EventArgs e)
        {
            HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
            ee.Handled = true;
        }
        /// <summary>
        /// 楼层图自动刷新委托方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //try
            //{
            //    this.Invoke(new RefreshMapCallBack(RefreshMap));
            //}
            //catch
            //{

            //}
            RefreshMap();
        }
        private delegate void RefreshMapCallBack();
        private void RefreshMap()
        {

            if (tctlMain.SelectedTab != tpgView)
            {
                return;
            }
            if (cboFloor.Items.Count == 0)
            {
                try
                {
                    seatTable = null;
                    employeeTable = null;
                    internTable = null;
                    employeeTmpTable = null;
                    picBaseMap.Image = null;
                }
                catch (System.Exception ex)
                {
                    log.Error(ex.Message, ex);
                    this.Close();
                }
                return;
            }
            if (!cboFloor.DroppedDown)
            {
                cboFloorItem cfi = cboFloor.SelectedItem as cboFloorItem;
                //lock (picBaseMap)
                //{
                RefreshMapOnTime(picBaseMap, cfi.Id);
                //}
            }
        }
        private void frmView_Load(object sender, EventArgs e)
        {
            client = new HRSeatClient();
            LoadMap();
            if (_adminView)
            {
                tctlMain.SelectedTab = tpgSettings;
            }
            try
            {
                //frmLoading.ShowSplashScreen();
                this._employeename = client.GetEmployeeNameByIDEmpType(_employeeid, (int)_employeeType);
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message, ex);
                this.Close();
            }
            CommonClass.FormSet.SetMid(this);
            FormSet.AnimateWindow(this.Handle, 800, FormSet.AW_BLEND);

            RefreshTimer.Interval = 1000;
            RefreshTimer.AutoReset = true;
            RefreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(RefreshTimer_Elapsed);
            //跨线程访问
            //Control.CheckForIllegalCrossThreadCalls = true;
            RefreshTimer.Start();
            lblUserName.Text = this._employeename + " 欢迎您！";
            lblIP.Text = CommonFunction.GetIPAddress();
            CenterPic(pnlBaseMap, picBaseMap);
            //frmLoading.CloseForm();
            //第一次启动提示
            IniFile iniFile = new IniFile("config.ini");
            if (iniFile.ReadInt("LOCAL_CONFIG", "FirstShow", 0) == 0)
            {
                DialogResult dr = MessageBox.Show("再次启动时不显示该界面，最小化到托盘。", "启动设置", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    //最小化到托盘
                    iniFile.WriteInt("LOCAL_CONFIG", "FirstShow", 1);
                }
                else
                {
                    //每次都弹出
                    iniFile.WriteInt("LOCAL_CONFIG", "FirstShow", 2);
                }
            }
        }
        /// <summary>
        /// 楼层combobox对象
        /// </summary>
        class cboFloorItem
        {
            public cboFloorItem(int id, string name)
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
        private void SerializableDicToFile(SerializableDictionary<int, byte[]> dic)
        {
            try
            {
                string imageFileName = Application.StartupPath + "\\" + CommonFunction.GetAppConfig("ImageFilePath");
                using (FileStream fileStream = new FileStream(imageFileName, FileMode.Create))
                {
                    XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<int, byte[]>));
                    xmlFormatter.Serialize(fileStream, dic);
                }
            }
            catch
            {

            }
        }
        private void SerializableFloorImg(DataSet_HRSeat.Hr_EP_FloorDataTable dt, SerializableDictionary<int, byte[]> dic)
        {
            dic.Clear();
            foreach (DataSet_HRSeat.Hr_EP_FloorRow row in dt)
            {
                dic.Add(row.ID, row.Image);
            }
            try
            {
                string imageFileName = Application.StartupPath + "\\" + CommonFunction.GetAppConfig("ImageFilePath");
                using (FileStream fileStream = new FileStream(imageFileName, FileMode.Create))
                {
                    XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<int, byte[]>));
                    xmlFormatter.Serialize(fileStream, dic);
                }
            }
            catch
            {

            }
        }
        private DataSet_HRSeat.Hr_EP_FloorDataTable GetFloorTable()
        {
            DataSet_HRSeat.Hr_EP_FloorDataTable dt = null;
            DataSet_HRSeat.Hr_EP_FloorNoImgDataTable floorNoImg = null;
            string imageFileName = Application.StartupPath + "\\" + CommonFunction.GetAppConfig("ImageFilePath");
            try
            {
                floorNoImg = client.GetAllFloorsNoImg();
            }
            catch
            {
                this.Close();
            }
            //没有楼层
            if (floorNoImg.Count == 0)
            {
                if (File.Exists(imageFileName))
                {
                    File.Delete(imageFileName);
                }
                return new DataSet_HRSeat.Hr_EP_FloorDataTable();
            }
            try
            {
                //反序列化文件
                using (FileStream fileStream = new FileStream(imageFileName, FileMode.Open))
                {
                    XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<int, byte[]>));
                    dicFloorImages = (SerializableDictionary<int, byte[]>)xmlFormatter.Deserialize(fileStream);
                }
            }
            catch
            {
                try
                {
                    dt = client.GetAllFloors();
                }
                catch
                {
                    this.Close();
                }
                SerializableFloorImg(dt, dicFloorImages);
                return dt;
            }
            //本地字典为空
            if (dicFloorImages.Count == 0)
            {
                try
                {
                    dt = client.GetAllFloors();
                }
                catch
                {
                    this.Close();

                }
                SerializableFloorImg(dt, dicFloorImages);
                return dt;
            }

            bool imgChange = false;
            if (floorNoImg.Count != dicFloorImages.Count)
            {
                imgChange = true;
            }
            else
            {
                foreach (DataSet_HRSeat.Hr_EP_FloorNoImgRow row in floorNoImg)
                {

                    if (dicFloorImages.ContainsKey(row.ID) && row.ImageLength != dicFloorImages[row.ID].Length)
                    {
                        imgChange = true;
                        break;
                    }
                }

            }
            if (imgChange)
            {
                //移出过期图片字典

                SerializableDictionary<int, byte[]> dicFloorImagesTmp = new SerializableDictionary<int, byte[]>();
                foreach (KeyValuePair<int, byte[]> kvp in dicFloorImages)
                {
                    dicFloorImagesTmp.Add(kvp.Key, kvp.Value);
                }
                foreach (KeyValuePair<int, byte[]> kvp in dicFloorImagesTmp)
                {
                    if (floorNoImg.FindByID(kvp.Key) == null)
                    {
                        dicFloorImages.Remove(kvp.Key);
                    }
                    else if (kvp.Value.Length != floorNoImg.FindByID(kvp.Key).ImageLength)
                    {
                        dicFloorImages.Remove(kvp.Key);
                    }
                }
                foreach (DataSet_HRSeat.Hr_EP_FloorNoImgRow floorNoImgRow in floorNoImg)
                {
                    if (!dicFloorImages.ContainsKey(floorNoImgRow.ID))
                    {
                        try
                        {

                            DataSet_HRSeat.Hr_EP_FloorDataTable dtTmp = client.GetFloorByID(floorNoImgRow.ID);
                            dicFloorImages.Add(floorNoImgRow.ID, dtTmp[0].Image);
                        }
                        catch
                        {
                            this.Close();
                        }
                    }
                }


                //try
                //{
                //    dt = client.GetAllFloors();
                //}
                //catch
                //{
                //    this.Close();

                //}
                SerializableDicToFile(dicFloorImages);
            }
            if (dt == null)
            {
                dt = new DataSet_HRSeat.Hr_EP_FloorDataTable();
            }
            else
            {
                dt.Clear();
            }
            foreach (DataSet_HRSeat.Hr_EP_FloorNoImgRow floorNoImgRow in floorNoImg)
            {
                DataSet_HRSeat.Hr_EP_FloorRow newRow = dt.NewHr_EP_FloorRow();
                newRow.ID = floorNoImgRow.ID;
                newRow.Code = floorNoImgRow.Code;
                newRow.Name = floorNoImgRow.Name;
                newRow.ImageWidth = floorNoImgRow.ImageWidth;
                newRow.ImageHeight = floorNoImgRow.ImageHeight;
                newRow.Image = dicFloorImages[floorNoImgRow.ID];
                dt.AddHr_EP_FloorRow(newRow);
            }
            return dt;
        }

        //return dt;
        /// <summary>
        /// 载入楼层图
        /// </summary>
        private void LoadMap()
        {
            Splasher.Show(typeof(frmLoading));
            floorTable = GetFloorTable();
            if (floorTable.Count == 0 && _isadmin == false)
            {
                MessageBox.Show("请联系管理员新增座位图！");
            }
            cboFloor.Items.Clear();
            foreach (DataSet_HRSeat.Hr_EP_FloorRow r in floorTable)
            {
                cboFloorItem cfi = new cboFloorItem(r.ID, r.Name);
                cboFloor.Items.Add(cfi);
            }
            if (floorTable.Count != 0)
            {
                if (_fl.ComboFloorIndex + 1 > cboFloor.Items.Count)
                {
                    _fl.ComboFloorIndex = cboFloor.Items.Count - 1;
                }
                cboFloor.SelectedIndex = _fl.ComboFloorIndex;
            }

            if (_isadmin)
            {
                RefreshdgrdFloorView(floorTable);
            }
            Splasher.Close(this);
        }
        /// <summary>
        /// 刷新楼层管理datagridview
        /// </summary>
        /// <param name="dt">楼层datatable</param>
        private void RefreshdgrdFloorView(DataSet_HRSeat.Hr_EP_FloorDataTable dt)
        {
            if (tctlMain.SelectedTab != tpgSettings)
            {
                return;
            }
            dgrdFloorView.DataSource = dt;

            if (dgrdFloorView.Columns.Count != dt.Columns.Count)
            {
                return;
            }
            dgrdFloorView.Columns["Image"].Visible = false;
            dgrdFloorView.Columns["ImageWidth"].Visible = false;
            dgrdFloorView.Columns["ImageHeight"].Visible = false;
            //dgrdFloorView.Columns["ID"].Width = 26;
            dgrdFloorView.Columns["ID"].Visible = false;
            dgrdFloorView.Columns["Code"].HeaderText = "编号";
            dgrdFloorView.Columns["Code"].Width = 80;
            dgrdFloorView.Columns["Name"].HeaderText = "名称";
            //dgrdFloorView.Columns["Name"].Width = dgrdFloorView.Width - 80 - 26 - 60 * 3 - 90 - dgrdFloorView.RowHeadersWidth;
            //dgrdFloorView.Columns["Name"].Width = dgrdFloorView.Width - 80 - 60 * 3 - 90;
            dgrdFloorView.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgrdFloorView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            DataGridViewColumn dgvc = new DataGridViewColumn();

            DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
            btnView.HeaderText = "操作";
            btnView.Name = "btnView";
            btnView.Text = "查看";
            btnView.Width = 60;
            btnView.UseColumnTextForButtonValue = true;

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.HeaderText = "";
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "删除";
            btnDelete.Width = 60;
            btnDelete.UseColumnTextForButtonValue = true;

            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
            btnEdit.HeaderText = "";
            btnEdit.Name = "btnEdit";
            btnEdit.Text = "编辑";
            btnEdit.Width = 60;
            btnEdit.UseColumnTextForButtonValue = true;

            DataGridViewButtonColumn btnEditSeat = new DataGridViewButtonColumn();
            btnEditSeat.HeaderText = "";
            btnEditSeat.Name = "btnEditSeat";
            btnEditSeat.Text = "编辑员工工位";
            btnEditSeat.Width = 90;
            btnEditSeat.UseColumnTextForButtonValue = true;

            dgrdFloorView.Columns.Add(btnView);
            dgrdFloorView.Columns.Add(btnDelete);
            dgrdFloorView.Columns.Add(btnEdit);
            dgrdFloorView.Columns.Add(btnEditSeat);
        }
        private bool CompareTwoDataTable(DataTable dt1, DataTable dt2)
        {

            if (dt1.Rows.Count != dt2.Rows.Count || dt1.Columns.Count != dt2.Columns.Count)
            {
                return false;
            }
            if ((dt1.Rows.Count == 0 || dt2.Rows.Count == 0) && dt1.Rows.Count == dt2.Rows.Count)
            {
                return false;
            }
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                for (int j = 0; j < dt1.Columns.Count; j++)
                {
                    //if (dt1.Rows.Count != dt2.Rows.Count)
                    //{
                    //    return false;
                    //}
                    if (dt1.Rows[i][j].ToString() != dt2.Rows[i][j].ToString())
                    {
                        return false;
                    }

                }
            }
            return true;
        }
        public void RefreshSeatTable()
        {
            seatTable = client.GetSeatsByFloorIDRefresh(floorEditFloorID);
        }
        /// <summary>
        /// 实时刷新楼层图状态
        /// </summary>
        /// <param name="picMap">地图</param>
        /// <param name="floorid">楼层id</param>
        private void RefreshMapOnTime(PictureBox picMap, int floorid)
        {
            cboFloorItem cfi = cboFloor.SelectedItem as cboFloorItem;
            if (cfi.Id != floorid)
            {
                return;
            }
            try
            {
                DataSet_HRSeat.Hr_EP_SeatDataTable seatTableTmp = client.GetSeatsByFloorID(floorid);
                if (seatTable != null)
                {
                    if (CompareTwoDataTable(seatTable, seatTableTmp))
                    {
                        return;
                    }
                    else
                    {
                        seatTable = seatTableTmp.Copy() as DataSet_HRSeat.Hr_EP_SeatDataTable;
                    }
                }
                else
                {
                    seatTable = seatTableTmp.Copy() as DataSet_HRSeat.Hr_EP_SeatDataTable;
                }
                //seatTable = client.GetSeatsByFloorID(floorid);
                employeeTable = client.GetEmployeeByFloorID(floorid);
                employeeTmpTable = client.GetEmployeeTmpByFloorID(floorid);
                internTable = client.GetInternByFloorID(floorid);
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message, ex);
                this.Close();
            }

            try
            {
                DrawMap(picMap, floorid);
            }
            catch
            {
                this.Close();
            }

        }
        private void RefreshMap(PictureBox picMap, int floorid)
        {
            //Splasher.Show(typeof(frmLoading));
            try
            {
                seatTable = client.GetSeatsByFloorID(floorid);
                employeeTable = client.GetEmployeeByFloorID(floorid);
                internTable = client.GetInternByFloorID(floorid);
                employeeTmpTable = client.GetEmployeeTmpByFloorID(floorid);
            }
            catch (System.Exception ex)
            {
                //Splasher.Close(this);
                log.Error(ex.Message, ex);
                return;
                //this.Close();
            }

            try
            {
                DrawMap(picMap, floorid);
            }
            catch
            {
                this.Close();
            }
            //Splasher.Close(this);
        }
        private void DrawMap(PictureBox picMap, int floorid)
        {
            int onlineCount = 0;
            int online2Count = 0;
            int offlineCount = 0;
            //绘制图片
            DataSet_HRSeat.Hr_EP_FloorRow floorrow = floorTable.FindByID(floorid);
            if (floorrow == null)
            {
                floorTable = GetFloorTable();
                floorrow = floorTable.FindByID(floorid);
            }
            Image img = ByteToImage(floorrow.Image);
            Graphics graBaseMap = Graphics.FromImage(img);
            foreach (DataSet_HRSeat.Hr_EP_SeatRow r in seatTable)
            {

                OnlineType type = OnlineType.Offline;
                switch (r.OnlineStatus)
                {
                    case 0:
                        type = OnlineType.Offline;
                        offlineCount++;
                        break;
                    case 1:
                        type = OnlineType.Online;
                        onlineCount++;
                        break;
                    case 2:
                        type = OnlineType.Online2;
                        online2Count++;
                        break;
                    default:
                        break;
                }
                System.Drawing.Image seatpic;
                switch (r.EmployeeType)
                {
                    case 0:
                        seatpic = CreateSeatPic(employeeTable.FindByID(r.EmployeeID).FullName, type, r.IP == CommonFunction.GetIPAddress());
                        break;
                    case 1:
                        seatpic = CreateSeatPic(internTable.FindByID(r.EmployeeID).FullName, type, r.IP == CommonFunction.GetIPAddress());
                        break;
                    case 2:
                        seatpic = CreateSeatPic(employeeTmpTable.FindByID(r.EmployeeID).FullName, type, r.IP == CommonFunction.GetIPAddress());
                        break;
                    default:
                        seatpic = CreateSeatPic(employeeTable.FindByID(r.EmployeeID).FullName, type, r.IP == CommonFunction.GetIPAddress());
                        break;
                }
                graBaseMap.DrawImage(seatpic, new Point((int)r.PosX - seatPicSizeX / 2, (int)r.PosY - seatPicSizeY / 2));
            }
            picMap.Image = img;

            //设置人数
            if (picMap == picBaseMap)
            {
                lblOnlineCount.Text = onlineCount.ToString();
                lblOnline2Count.Text = online2Count.ToString();
                lblOfflineCount.Text = offlineCount.ToString();
            }
            graBaseMap.Dispose();
        }

        /// <summary>
        /// 二进制流转图片
        /// </summary>
        /// <param name="imagebyte">二进制流</param>
        /// <returns>图片对象</returns>
        private Image ByteToImage(byte[] imagebyte)
        {
            MemoryStream ms = new MemoryStream();
            ms = new MemoryStream(imagebyte);
            return System.Drawing.Image.FromStream(ms);
        }
        /// <summary>
        /// 图片转二进制流
        /// </summary>
        /// <param name="imageIn">图片对象</param>
        /// <returns>二进制流</returns>
        public static byte[] ImageToByte(Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (imageIn != null)
                {
                    Bitmap t = new Bitmap(imageIn);
                    t.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                return ms.ToArray();
            }
        }
        /// <summary>
        /// 创建单个座位图片
        /// </summary>
        /// <param name="name">座位显示姓名</param>
        /// <param name="onlinetype">在线状态</param>
        /// <param name="iscurrent">是否当前机器</param>
        /// <returns>座位图片位图</returns>
        private Bitmap CreateSeatPic(string name, OnlineType onlinetype, bool iscurrent)
        {
            System.Drawing.Image seatimage = null;
            switch (onlinetype)
            {
                case OnlineType.Online:
                    seatimage = System.Drawing.Image.FromFile(Application.StartupPath + "\\Images\\online.png");
                    break;
                case OnlineType.Online2:
                    seatimage = System.Drawing.Image.FromFile(Application.StartupPath + "\\Images\\online2.png");
                    break;
                case OnlineType.Offline:
                    seatimage = System.Drawing.Image.FromFile(Application.StartupPath + "\\Images\\offline.png");
                    break;
                default:
                    break;
            }


            Bitmap bitmap = new Bitmap(seatPicSizeX, seatPicSizeY);
            bitmap.MakeTransparent(Color.Transparent);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            //g.DrawRectangle(new Pen(Color.Black), 0, 0, bitmap.Width - 1, bitmap.Height - 1);

            g.DrawImage(seatimage, bitmap.Width / 2 - seatimage.Width / 2, bitmap.Height - seatimage.Height, seatimage.Width, seatimage.Height);
            if (iscurrent)
            {
                name = "当前座位";
            }
            float fontsize = 9f;
            name = CommonFunction.bSubstring(name, 8, "…");

            //if (name.Length > 3)
            //{
            //    fontsize = 8f;
            //}
            Font font = new Font("宋体", fontsize);
            SizeF sizeF = g.MeasureString(name, font);
            PointF pointF = new PointF((seatPicSizeX - sizeF.Width) / 2, 3);
            //背景
            g.FillRectangle(new SolidBrush(Color.FromArgb(247, 252, 190)), pointF.X, pointF.Y - 1, sizeF.Width - 2, sizeF.Height - 3);
            //名字
            g.DrawString(name, font, Brushes.Black, pointF);
            //g.DrawRectangle(new Pen(Color.Black), pointF.X, pointF.Y - 1, sizeF.Width, sizeF.Height - 1);
            g.Save();
            //dispose
            g.Dispose();
            seatimage.Dispose();
            return bitmap;
        }

        private void cboFloor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbo = sender as ComboBox;
            if (cbo.DroppedDown)
            {
                return;
            }
            cbo.Enabled = false;
            cboFloorItem cfi = cbo.SelectedItem as cboFloorItem;
            //RefreshMap(picBaseMap, cfi.Id);
            RefreshMapOnTime(picBaseMap, cfi.Id);
            ResizeAndCenterMap();
            _fl.ComboFloorIndex = (sender as ComboBox).SelectedIndex;
            cbo.Enabled = true;
        }



        private void picBaseMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }
            Point mousePoint = pnlBaseMap.PointToClient(Cursor.Position);
            mousePoint.X = mousePoint.X - pnlBaseMap.AutoScrollPosition.X;
            mousePoint.Y = mousePoint.Y - pnlBaseMap.AutoScrollPosition.Y;
            foreach (DataSet_HRSeat.Hr_EP_SeatRow r in seatTable)
            {
                if (mousePoint.X > r.PosX - seatPicSizeX / 2 && mousePoint.Y > r.PosY - seatPicSizeY / 2 && mousePoint.X < r.PosX + seatPicSizeX / 2 && mousePoint.Y < r.PosY + seatPicSizeY / 2)
                {
                    Form frmseatview = new frmSeatView(this, r.ID, _employeeid);
                    frmseatview.ShowDialog();
                    break;
                }
            }
        }
        private Point _StartPoint;
        private void picBaseMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left != e.Button) return;
            _StartPoint = e.Location;
            PictureBox pic = sender as PictureBox;
            Panel p = pic.Parent as Panel;
            if (pic.Cursor != Cursors.Hand && p.AutoScroll == true)
            {
                SetCursor(pic, (Bitmap)Bitmap.FromFile(Application.StartupPath + "\\Images\\drag.png"));
            }
        }
        private void SetCursor(PictureBox pic, Bitmap cursor)
        {
            Point hotPoint = new Point(0, 0);
            try
            {
                if (myNewCursor == null || gCursor == null || handCursor == null)
                {
                    myNewCursor = new Bitmap(cursor.Width * 2 - hotPoint.X, cursor.Height * 2 - hotPoint.Y);
                    gCursor = Graphics.FromImage(myNewCursor);
                    gCursor.Clear(Color.FromArgb(0, 0, 0, 0));
                    gCursor.DrawImage(cursor, cursor.Width - hotPoint.X, cursor.Height - hotPoint.Y, cursor.Width,
                     cursor.Height);
                    handCursor = new Cursor(myNewCursor.GetHicon());

                }
                pic.Cursor = handCursor;
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        private void picBaseMap_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic.Image == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Left && pnlBaseMap.AutoScroll == true)
            {
                Point changePoint = new Point(e.Location.X - _StartPoint.X,
                                              e.Location.Y - _StartPoint.Y);
                pnlBaseMap.AutoScrollPosition = new Point(-pnlBaseMap.AutoScrollPosition.X - changePoint.X,
                                                      -pnlBaseMap.AutoScrollPosition.Y - changePoint.Y);
                //pic.Cursor = Cursors.SizeAll;
                SetCursor(pic, (Bitmap)Bitmap.FromFile(Application.StartupPath + "\\Images\\drag.png"));
            }
            else
            {
                pic.Cursor = Cursors.Arrow;
            }


            Point mousePoint = pnlBaseMap.PointToClient(Cursor.Position);
            mousePoint.X = mousePoint.X - pnlBaseMap.AutoScrollPosition.X;
            mousePoint.Y = mousePoint.Y - pnlBaseMap.AutoScrollPosition.Y;
            if (seatTable.Count != 0)
            {
                foreach (DataSet_HRSeat.Hr_EP_SeatRow r in seatTable)
                {
                    if (mousePoint.X > r.PosX - seatPicSizeX / 2 && mousePoint.Y > r.PosY - seatPicSizeY / 2 && mousePoint.X < r.PosX + seatPicSizeX / 2 && mousePoint.Y < r.PosY + seatPicSizeY / 2)
                    {
                        (sender as PictureBox).Cursor = Cursors.Hand;
                        break;
                    }
                }
            }

        }


        private void dgrdFloorView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            floorEditFloorID = (int)dgrdFloorView.Rows[e.RowIndex].Cells["ID"].Value;
            if (dgrdFloorView.Columns[e.ColumnIndex].Name == "btnView")
            {
                floorEditMode = FloorEditMode.View;
                tctlEditFloor.SelectedTab = tpgAddFloor;
            }
            else if (dgrdFloorView.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                DialogResult dr = MessageBox.Show("确实要删除吗？删除后该楼层的所有房间、座位信息会全部删除！", "删除确认", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        if (client.DeleteFloor(floorEditFloorID))
                        {
                            LoadMap();
                            MessageBox.Show("删除成功！");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        log.Error(ex.Message, ex);
                        this.Close();

                    }
                }
                else
                {
                    return;
                }
            }
            else if (dgrdFloorView.Columns[e.ColumnIndex].Name == "btnEdit")
            {
                floorEditMode = FloorEditMode.Edit;
                tctlEditFloor.SelectedTab = tpgAddFloor;
            }
            else if (dgrdFloorView.Columns[e.ColumnIndex].Name == "btnEditSeat")
            {
                floorEditMode = FloorEditMode.Edit;
                tctlEditFloor.SelectedTab = tpgSetSeat;
            }
        }

        private void btnAddFloor_Click(object sender, EventArgs e)
        {
            floorEditMode = FloorEditMode.New;
            txtFloorName.Text = "";
            txtFloorCode.Text = "";
            txtPicName.Text = "";
            picBaseMapPreview.Image = null;
            tctlEditFloor.SelectedTab = tpgAddFloor;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            odlgUpload.Filter = "Jpg文件|*.jpg|Bmp文件|*.bmp|Png文件|*.png";
            odlgUpload.ShowDialog();
        }

        private void odlgUpload_FileOk(object sender, CancelEventArgs e)
        {
            //上传图片到pictureBox1
            FileStream fs = null;
            Bitmap bmp = null;
            long fileLength = 0;
            try
            {
                fs = new FileStream(odlgUpload.FileName, FileMode.Open, FileAccess.Read);
                bmp = new Bitmap(fs);
                fileLength = fs.Length;
                fs.Close();
            }
            catch
            {
                MessageBox.Show("图片格式错误，请重新选择");
                e.Cancel = true;
                return;
            }
            if (bmp.Size.Width * bmp.Size.Height > 8000000 || fileLength > 1024 * 1024 * 100)
            {
                MessageBox.Show("图片过大，请重新选择");
                e.Cancel = true;
                return;
            }
            if (!(bmp.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg) ||
                bmp.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png) ||
                bmp.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp)))
            {
                MessageBox.Show("图片格式错误，请重新选择");
                e.Cancel = true;
                return;
            }
            picBaseMapPreview.Image = bmp;
            ResizePanel(pnlBaseMapPreview, picBaseMapPreview);
            CenterPic(pnlBaseMapPreview, picBaseMapPreview);
            //图像适合图片框的大小
            //this.picBaseMapPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            txtPicName.Text = odlgUpload.FileName.ToString();
        }

        private Point _StartPointPreview;
        private void picBaseMapPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _StartPointPreview = e.Location;
            PictureBox pic = sender as PictureBox;
            Panel p = pic.Parent as Panel;
            if (pic.Cursor != Cursors.Hand && p.AutoScroll == true)
            {
                //pic.Cursor = Cursors.SizeAll;
                SetCursor(pic, (Bitmap)Bitmap.FromFile(Application.StartupPath + "\\Images\\drag.png"));
            }
        }

        private void picBaseMapPreview_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic.Image == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Left && pnlBaseMapPreview.AutoScroll == true)
            {
                Point changePoint = new Point(e.Location.X - _StartPointPreview.X,
                                              e.Location.Y - _StartPointPreview.Y);
                pnlBaseMapPreview.AutoScrollPosition = new Point(-pnlBaseMapPreview.AutoScrollPosition.X - changePoint.X,
                                                      -pnlBaseMapPreview.AutoScrollPosition.Y - changePoint.Y);
                //pic.Cursor = Cursors.SizeAll;
                SetCursor(pic, (Bitmap)Bitmap.FromFile(Application.StartupPath + "\\Images\\drag.png"));
            }
            else
            {
                pic.Cursor = Cursors.Arrow;
            }

        }



        private void tctlEditFloor_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblUpload.Visible = true;
            txtPicName.Visible = true;
            btnUpload.Visible = true;

            TabControl tc = sender as TabControl;


            #region 添加平面图
            if (tc.SelectedTab == tpgAddFloor)
            {
                //新增
                if (floorEditMode == FloorEditMode.New)
                {
                    txtFloorName.ReadOnly = false;
                    txtFloorCode.ReadOnly = false;
                    gbxSaveSeats.Visible = false;
                }
                //查看
                else if (floorEditMode == FloorEditMode.View)
                {
                    picBaseMapPreview.Image = ByteToImage(floorTable.FindByID(floorEditFloorID).Image);
                    ResizePanel(pnlBaseMapPreview, picBaseMapPreview);
                    CenterPic(pnlBaseMapPreview, picBaseMapPreview);
                    txtFloorName.Text = floorTable.FindByID(floorEditFloorID).Name;
                    txtFloorCode.Text = floorTable.FindByID(floorEditFloorID).Code;

                    txtFloorName.ReadOnly = true;
                    txtFloorCode.ReadOnly = true;
                    lblUpload.Visible = false;
                    txtPicName.Visible = false;
                    btnUpload.Visible = false;
                    gbxSaveSeats.Visible = false;
                }
                //编辑
                else if (floorEditMode == FloorEditMode.Edit)
                {
                    picBaseMapPreview.Image = ByteToImage(floorTable.FindByID(floorEditFloorID).Image);
                    ResizePanel(pnlBaseMapPreview, picBaseMapPreview);
                    txtFloorName.Text = floorTable.FindByID(floorEditFloorID).Name;
                    txtFloorCode.Text = floorTable.FindByID(floorEditFloorID).Code;
                    txtPicName.Text = "";

                    txtFloorName.ReadOnly = false;
                    txtFloorCode.ReadOnly = false;
                    gbxSaveSeats.Visible = true;
                }
            }
            #endregion

            #region 房间设置
            else if (tc.SelectedTab == tpgAddRoom)
            {
                rbtnSaveSeats.Checked = true;
                try
                {
                    //frmLoading.ShowSplashScreen();
                    roomTable = client.GetRoomByFloorID(floorEditFloorID);
                    //frmLoading.CloseForm();
                }
                catch /*(System.Exception ex)*/
                {
                    this.Close();
                }
                RefreshdgrdRoomSettings(roomTable, floorEditMode);
                if (floorEditMode == FloorEditMode.View)
                {
                    btnAddRoom.Visible = false;
                    btnTpgAddRoomSave.Visible = false;
                }
                else
                {
                    btnAddRoom.Visible = true;
                    btnTpgAddRoomSave.Visible = true;
                }
            }
            #endregion
            #region 座位设置
            else if (tc.SelectedTab == tpgSetSeat)
            {
                try
                {
                    //frmLoading.ShowSplashScreen();
                    roomTable = client.GetRoomByFloorID(floorEditFloorID);
                    //frmLoading.CloseForm();
                }
                catch /*(System.Exception ex)*/
                {
                    this.Close();
                }
                //将seatTable=null强制在编辑的时候重绘编辑的Map 即强制调用RefreshMap
                seatTable = null;
                RefreshMap(picSetSeatMap, floorEditFloorID);
                ResizeAndCenterMap();
            }
            #endregion
        }

        private void btnTpgAddFloorCancel_Click(object sender, EventArgs e)
        {
            tctlEditFloor.SelectedTab = tpgViewFloor;
        }

        private void btnTpgAddFloorNext_Click(object sender, EventArgs e)
        {
            if (txtFloorCode.Text.Trim() == "")
            {
                ttFloorCode.Show("", txtFloorCode, 0);
                ttFloorCode.Show("请输入楼层编号", txtFloorCode, 0, txtFloorCode.Height, 2000);
                return;
            }
            else if (txtFloorName.Text.Trim() == "")
            {
                ttFloorName.Show("", txtFloorName, 0);
                ttFloorName.Show("请输入楼层名称", txtFloorName, 0, txtFloorName.Height, 2000);
                return;
            }


            if (txtFloorCode.Text.Trim() == "" || txtFloorName.Text.Trim() == "")
            {
                MessageBox.Show("请将信息补全!");
                return;
            }
            if (floorEditMode == FloorEditMode.Edit)
            {
                //检测重复
                var tmp = from n in floorTable where n.Code == txtFloorCode.Text.Trim() where n.ID != floorEditFloorID select n;
                if (tmp.ToList().Count != 0)
                {
                    ttFloorCode.Show("", txtFloorCode, 0);
                    ttFloorCode.ToolTipTitle = "填写有误";
                    ttFloorCode.Show("编号已存在，请重新输入", txtFloorCode, 0, txtFloorCode.Height, 2000);
                    return;
                }
                tmp = from n in floorTable where n.Name == txtFloorName.Text.Trim() where n.ID != floorEditFloorID select n;
                if (tmp.ToList().Count != 0)
                {
                    ttFloorName.Show("", txtFloorName, 0);
                    ttFloorName.ToolTipTitle = "填写有误";
                    ttFloorName.Show("名称已存在，请重新输入", txtFloorName, 0, txtFloorName.Height, 2000);
                    return;
                }
                if (rbtnCleanSeats.Checked)
                {
                    DialogResult dr = MessageBox.Show("确实要清空座位吗？清空后该楼层的所有座位信息会全部删除！", "清空确认", MessageBoxButtons.YesNo);
                    if (dr != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                }
                DataSet_HRSeat.Hr_EP_FloorRow row = floorTable.FindByID(floorEditFloorID);
                row.Image = ImageToByte(picBaseMapPreview.Image);
                row.ImageHeight = picBaseMapPreview.Image.Height;
                row.ImageWidth = picBaseMapPreview.Image.Width;
                row.Name = txtFloorName.Text.Trim();
                row.Code = txtFloorCode.Text.Trim();

                try
                {
                    Splasher.Show(typeof(frmLoading));
                    DataSet_HRSeat.Hr_EP_FloorDataTable updateTable = new DataSet_HRSeat.Hr_EP_FloorDataTable();
                    updateTable.ImportRow(row);
                    client.UpdateFloor(updateTable);
                    Splasher.Close(this);
                }
                catch /*(System.Exception ex)*/
                {
                    Splasher.Close(this);
                    MessageBox.Show("更新失败！");
                }
                //如果更换了底图 则清空原有的房间和座位
                if (rbtnCleanSeats.Checked)
                {
                    //client.DeleteRoomsAndSeats(floorEditFloorID);
                    Splasher.Show(typeof(frmLoading));
                    client.DeleteSeatsByFloorID(floorEditFloorID);
                    Splasher.Close(this);
                }
                LoadMap();
                //MessageBox.Show("更新成功!");
                RefreshMap(picBaseMap, (cboFloor.SelectedItem as cboFloorItem).Id);
            }
            else if (floorEditMode == FloorEditMode.New)
            {
                //检测重复
                var tmp = from n in floorTable where n.Code == txtFloorCode.Text.Trim() select n;
                if (tmp.ToList().Count != 0)
                {
                    ttFloorCode.Show("", txtFloorCode, 0);
                    ttFloorCode.ToolTipTitle = "填写有误";
                    ttFloorCode.Show("编号已存在，请重新输入", txtFloorCode, 0, txtFloorCode.Height, 2000);
                    return;
                }
                tmp = from n in floorTable where n.Name == txtFloorName.Text.Trim() select n;
                if (tmp.ToList().Count != 0)
                {
                    ttFloorName.Show("", txtFloorName, 0);
                    ttFloorName.ToolTipTitle = "填写有误";
                    ttFloorName.Show("名称已存在，请重新输入", txtFloorName, 0, txtFloorName.Height, 2000);
                    return;
                }
                if (txtPicName.Text.Trim() == "")
                {
                    //ttUserName.SetToolTip(txtUserName, "请输入帐号后再登录");
                    ttFloorPic.Show("", btnUpload, 0);
                    ttFloorPic.Show("请上传楼层图片", btnUpload, 2000);
                    return;
                }
                DataSet_HRSeat.Hr_EP_FloorRow row = floorTable.NewHr_EP_FloorRow();
                row.Image = ImageToByte(picBaseMapPreview.Image);
                row.ImageHeight = picBaseMapPreview.Image.Height;
                row.ImageWidth = picBaseMapPreview.Image.Width;
                row.Name = txtFloorName.Text.Trim();
                row.Code = txtFloorCode.Text.Trim();
                floorTable.Rows.Add(row);
                try
                {
                    Splasher.Show(typeof(frmLoading));
                    DataSet_HRSeat.Hr_EP_FloorDataTable updateTable = new DataSet_HRSeat.Hr_EP_FloorDataTable();
                    updateTable.ImportRow(row);
                    client.UpdateFloor(updateTable);
                    Splasher.Close(this);
                }
                catch /*(System.Exception ex)*/
                {
                    MessageBox.Show("添加失败！");
                    //this.Close();
                }
                finally
                {
                    Splasher.Close(this);
                }
                floorTable.AcceptChanges();
                LoadMap();

                floorEditFloorID = Convert.ToInt32(floorTable.Compute("max([ID])", ""));
                //MessageBox.Show("添加成功!");
                RefreshMap(picBaseMap, (cboFloor.SelectedItem as cboFloorItem).Id);
                floorEditMode = FloorEditMode.Edit;
            }
            tctlEditFloor.SelectedTab = tpgAddRoom;

        }

        private void tctlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tc = sender as TabControl;
            if (tc.SelectedTab == tpgSettings)
            {
                tctlEditFloor.SelectedTab = tpgViewFloor;
                RefreshdgrdFloorView(floorTable);
            }
            else if (tc.SelectedTab == tpgView)
            {
                cboFloorItem cfi = cboFloor.SelectedItem as cboFloorItem;
                if (cfi != null)
                {
                    RefreshMap(picBaseMap, cfi.Id);
                    ResizeAndCenterMap();
                    //CenterPic(pnlBaseMap, picBaseMap);
                }
            }

        }
        /// <summary>
        /// 刷新房间datagridview
        /// </summary>
        /// <param name="dt">房间datatable</param>
        /// <param name="fem">楼层编辑模式</param>
        private void RefreshdgrdRoomSettings(DataSet_HRSeat.Hr_EP_RoomDataTable dt, FloorEditMode fem)
        {
            dgrdRoomSettings.RowTemplate.Height = 30;
            dgrdRoomSettings.DataSource = dt;

            //if (dgrdRoomSettings.Columns.Count != dt.Columns.Count)
            //{
            //    return;
            //}
            dgrdRoomSettings.Columns["FloorID"].Visible = false;
            dgrdRoomSettings.Columns["ID"].Visible = false; ;
            dgrdRoomSettings.Columns["ID"].Width = 26;
            dgrdRoomSettings.Columns["Code"].HeaderText = "编号";
            dgrdRoomSettings.Columns["Code"].Width = 80;
            dgrdRoomSettings.Columns["Name"].HeaderText = "名称";
            //dgrdRoomSettings.Columns["Name"].Width = 500;
            dgrdRoomSettings.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgrdRoomSettings.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            if (fem != FloorEditMode.View)
            {
                if (dgrdRoomSettings.Columns.Contains("btnDelete"))
                {
                    return;
                }
                DataGridViewColumn dgvc = new DataGridViewColumn();
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.HeaderText = "操作";
                btnDelete.Name = "btnDelete";
                btnDelete.Text = "删除";
                btnDelete.Width = 51;

                btnDelete.FlatStyle = FlatStyle.Popup;
                btnDelete.DefaultCellStyle.BackColor = Color.FromArgb(227, 240, 252);
                btnDelete.UseColumnTextForButtonValue = true;


                dgrdRoomSettings.Columns.Add(btnDelete);
            }
            else
            {
                if (dgrdRoomSettings.Columns.Contains("btnDelete"))
                {
                    dgrdRoomSettings.Columns.Remove("btnDelete");
                }

            }
        }
        private bool SaveRoomSettings()
        {

            foreach (DataGridViewRow row in dgrdRoomSettings.Rows)
            {
                if (row.Cells["Code"].EditedFormattedValue.ToString() == "")
                {
                    MessageBox.Show("请填完数据!");
                    dgrdRoomSettings.CurrentCell = dgrdRoomSettings.Rows[row.Index].Cells["Code"];
                    dgrdRoomSettings.BeginEdit(true);
                    return false;
                }
                else if (row.Cells["Name"].EditedFormattedValue.ToString() == "")
                {
                    MessageBox.Show("请填完数据!");
                    dgrdRoomSettings.CurrentCell = dgrdRoomSettings.Rows[row.Index].Cells["Name"];
                    dgrdRoomSettings.BeginEdit(true);
                    return false;
                }
            }
            try
            {
                if (client.UpdateRooms(roomTable))
                {
                    MessageBox.Show("保存成功");
                }
                roomTable.AcceptChanges();
                roomTable = client.GetRoomByFloorID(floorEditFloorID);
            }
            catch /*(System.Exception ex)*/
            {
                this.Close();

            }
            RefreshdgrdRoomSettings(roomTable, floorEditMode);
            return true;
        }

        private void btnTpgAddRoomSave_Click(object sender, EventArgs e)
        {

            SaveRoomSettings();
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            //client.UpdateRooms(roomTable);
            //roomTable.AcceptChanges();
            DataSet_HRSeat.Hr_EP_RoomRow newrow = roomTable.NewHr_EP_RoomRow();
            newrow.FloorID = floorEditFloorID;
            roomTable.Rows.Add(newrow);
            dgrdRoomSettings.CurrentCell = dgrdRoomSettings.Rows[dgrdRoomSettings.Rows.Count - 1].Cells["Code"];
            dgrdRoomSettings.BeginEdit(true);
        }


        private void dgrdRoomSettings_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

            DataGridView dgv = sender as DataGridView;
            if (dgv.Columns[e.ColumnIndex].Name == "Code" || dgv.Columns[e.ColumnIndex].Name == "Name")
            {
                if (e.FormattedValue.ToString() == "")
                {
                    MessageBox.Show("请填完数据!");
                    dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    e.Cancel = true;
                }
            }
        }

        private void dgrdRoomSettings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int roomid = (int)dgrdRoomSettings.Rows[e.RowIndex].Cells["ID"].Value;
            if (dgrdRoomSettings.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                DialogResult dr = MessageBox.Show("确实要删除吗？删除后该房间的所有座位信息会全部删除！", "删除确认", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    dgrdRoomSettings.Rows.RemoveAt(e.RowIndex);
                    if (roomid < 1)
                    {
                        return;
                    }
                    try
                    {
                        Splasher.Show(typeof(frmLoading));
                        client.DeleteSeatByRoomID(roomid);
                        client.UpdateRooms(roomTable);
                        Splasher.Close(this);
                    }
                    catch /*(System.Exception ex)*/
                    {
                        Splasher.Close(this);
                        this.Close();
                    }
                    roomTable.AcceptChanges();
                }
                else
                {
                    return;
                }
            }
        }

        private void btnTpgAddRoomPrev_Click(object sender, EventArgs e)
        {
            tctlEditFloor.SelectedTab = tpgAddFloor;
        }

        private void btnTpgAddRoomNext_Click(object sender, EventArgs e)
        {
            if (dgrdRoomSettings.Rows.Count == 0)
            {
                MessageBox.Show("请设置房间!");
                return;
            }
            bool saveOK = SaveRoomSettings();
            if (!saveOK)
            {
                return;
            }
            tctlEditFloor.SelectedTab = tpgSetSeat;

        }
        private Point _StartPointSetSeat;

        private void picSetSeatMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left != e.Button) return;
            _StartPointSetSeat = e.Location;
            PictureBox pic = sender as PictureBox;
            Panel p = pic.Parent as Panel;
            if (pic.Cursor != Cursors.Hand && p.AutoScroll == true)
            {
                //pic.Cursor = Cursors.SizeAll;
                SetCursor(pic, (Bitmap)Bitmap.FromFile(Application.StartupPath + "\\Images\\drag.png"));
            }
        }

        private void picSetSeatMap_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic.Image == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Left && pnlSetSeatMap.AutoScroll == true)
            {
                Point changePoint = new Point(e.Location.X - _StartPointSetSeat.X,
                                              e.Location.Y - _StartPointSetSeat.Y);
                pnlSetSeatMap.AutoScrollPosition = new Point(-pnlSetSeatMap.AutoScrollPosition.X - changePoint.X,
                                                      -pnlSetSeatMap.AutoScrollPosition.Y - changePoint.Y);
                //pic.Cursor = Cursors.SizeAll;
                SetCursor(pic, (Bitmap)Bitmap.FromFile(Application.StartupPath + "\\Images\\drag.png"));
            }
            else
            {
                pic.Cursor = Cursors.Arrow;
            }

            Point mousePoint = pnlSetSeatMap.PointToClient(Cursor.Position);
            mousePoint.X = mousePoint.X - pnlSetSeatMap.AutoScrollPosition.X;
            mousePoint.Y = mousePoint.Y - pnlSetSeatMap.AutoScrollPosition.Y;
            if (seatTable.Count != 0)
            {
                foreach (DataSet_HRSeat.Hr_EP_SeatRow r in seatTable)
                {
                    if (mousePoint.X > r.PosX - seatPicSizeX / 2 && mousePoint.Y > r.PosY - seatPicSizeY / 2 && mousePoint.X < r.PosX + seatPicSizeX / 2 && mousePoint.Y < r.PosY + seatPicSizeY / 2)
                    {
                        (sender as PictureBox).Cursor = Cursors.Hand;
                        break;
                    }
                }
            }

        }

        private void picSetSeatMap_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePoint = pnlSetSeatMap.PointToClient(Cursor.Position);
            mousePoint.X = mousePoint.X - pnlSetSeatMap.AutoScrollPosition.X;
            mousePoint.Y = mousePoint.Y - pnlSetSeatMap.AutoScrollPosition.Y;
            //单击左键查看
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {

                foreach (DataSet_HRSeat.Hr_EP_SeatRow r in seatTable)
                {
                    if (mousePoint.X > r.PosX - seatPicSizeX / 2 && mousePoint.Y > r.PosY - seatPicSizeY / 2 && mousePoint.X < r.PosX + seatPicSizeX / 2 && mousePoint.Y < r.PosY + seatPicSizeY / 2)
                    {
                        Form frmseatview = new frmSeatView(this, r.ID, _employeeid);
                        frmseatview.ShowDialog();
                        break;
                    }
                }
            }
            //单击右键编辑
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (floorEditMode == FloorEditMode.View)
                {
                    return;
                }

                foreach (DataSet_HRSeat.Hr_EP_SeatRow r in seatTable)
                {
                    if (mousePoint.X > r.PosX - seatPicSizeX / 2 && mousePoint.Y > r.PosY - seatPicSizeY / 2 && mousePoint.X < r.PosX + seatPicSizeX / 2 && mousePoint.Y < r.PosY + seatPicSizeY / 2)
                    {
                        Form frmseatedit = new frmSeatEdit(this, r.ID, picSetSeatMap.Image.Size);
                        frmseatedit.ShowDialog();
                        //RefreshSeatTable();
                        RefreshMap(picSetSeatMap, floorEditFloorID);
                        return;
                    }
                }
                Form frmseatedit_add = new frmSeatEdit(this, mousePoint, picSetSeatMap.Image.Size);
                frmseatedit_add.ShowDialog();
                RefreshMap(picSetSeatMap, floorEditFloorID);
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("编辑完成，将返回楼层列表界面。");
            tctlEditFloor.SelectedTab = tpgViewFloor;
        }

        private void btnTpgSetSeatPrev_Click(object sender, EventArgs e)
        {
            tctlEditFloor.SelectedTab = tpgAddRoom;

        }

        private void llblEmployeeStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEmployeeStatus fes = new frmEmployeeStatus();
            fes.ShowDialog();
        }

        /// <summary>
        /// 图片居中
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pb"></param>
        private void CenterPic(Panel p, PictureBox pb)
        {
            if (pb.Image == null)
            {
                return;
            }
            Point autoScrollPosion = new Point();
            autoScrollPosion.X = p.AutoScrollPosition.X;
            autoScrollPosion.Y = p.AutoScrollPosition.Y;

            if (pb.Image.Width > p.Width)
            {
                autoScrollPosion.X = (pb.Image.Width - p.Width) / 2;
            }
            else
            {
                p.HorizontalScroll.Visible = false;
            }
            if (pb.Image.Height > p.Height)
            {
                autoScrollPosion.Y = (pb.Image.Height - p.Height) / 2;
            }
            else
            {
                p.VerticalScroll.Visible = false;
            }
            p.AutoScrollPosition = autoScrollPosion;
        }
        //还需要调校
        /// <summary>
        /// 根据图片大小调整图片panel的位置
        /// </summary>
        /// <param name="p">图片panel</param>
        /// <param name="pb">图片picturebox控件</param>
        private void ResizePanel(Panel p, PictureBox pb)
        {
            if (pb.Image == null)
            {
                return;
            }
            try
            {
                Point startLocation;
                Point endLocation;
                Size curSize = new Size();
                //scroll bug
                bool isautoscrollx = false;
                bool isautoscrolly = false;
                if (p == pnlBaseMap)
                {
                    startLocation = new Point(10, 48);
                    endLocation = new Point(this.Width - 32, this.Height - 138);
                    curSize = new Size(endLocation.X - startLocation.X, endLocation.Y - startLocation.Y);
                }
                else if (p == pnlBaseMapPreview)
                {
                    startLocation = new Point(2, 152);
                    endLocation = new Point(this.Width - 43, this.Height - 213);
                    curSize = new Size(endLocation.X - startLocation.X, endLocation.Y - startLocation.Y);
                }
                else
                {
                    startLocation = new Point(2, 62);
                    endLocation = new Point(this.Width - 42, this.Height - 196);
                    curSize = new Size(endLocation.X - startLocation.X, endLocation.Y - startLocation.Y);
                }

                if (pb.Image.Size.Width < (endLocation.X - startLocation.X))
                {
                    isautoscrollx = false;
                    startLocation = new Point(startLocation.X + (curSize.Width - pb.Size.Width) / 2, startLocation.Y);
                    curSize = new Size(pb.Image.Size.Width, curSize.Height);
                }
                else
                {
                    isautoscrollx = true;
                    p.HorizontalScroll.Visible = true;
                }
                if (pb.Image.Size.Height < (endLocation.Y - startLocation.Y))
                {
                    isautoscrolly = false;
                    startLocation = new Point(startLocation.X, startLocation.Y + (curSize.Height - pb.Size.Height) / 2);
                    curSize = new Size(curSize.Width, pb.Image.Size.Height);
                }
                else
                {
                    isautoscrolly = true;
                    p.VerticalScroll.Visible = true;
                }
                p.Size = curSize;
                p.Location = startLocation;
                p.AutoScroll = isautoscrollx || isautoscrolly;
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message, ex);
                return;
            }
        }

        private void btnAddRoomCancel_Click(object sender, EventArgs e)
        {
            tctlEditFloor.SelectedTab = tpgViewFloor;
        }

        private void llblContractsEXP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExportContracts();
        }
        /// <summary>
        /// 导出通讯录
        /// </summary>
        private void ExportContracts()
        {
            frmEXPSet frmExp = new frmEXPSet();
            frmExp.ShowDialog();
            if (frmExp.Tag == null)
            {
                return;
            }
            Enums.ExpType exptype = (Enums.ExpType)frmExp.Tag;
            string expTypeStr = string.Empty;
            switch (exptype)
            {
                case Enums.ExpType.Name:
                    expTypeStr = "（按照姓名）";

                    break;
                case Enums.ExpType.Dept:
                    expTypeStr = "（按照部门）";
                    break;
                default:
                    break;
            }

            sfdContractsEXP.Filter = "Excel 文档|*.xls";
            sfdContractsEXP.Title = "保存通讯录";
            sfdContractsEXP.FileName = "通讯录" + expTypeStr;
            DialogResult dr = sfdContractsEXP.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                return;
            }




            DataTable dtContracts = null;
            try
            {
                Splasher.Show(typeof(frmLoading));
                switch (exptype)
                {
                    case Enums.ExpType.Name:
                        dtContracts = client.ExportContractsByName();
                        break;
                    case Enums.ExpType.Dept:
                        dtContracts = client.ExportContracts();
                        break;
                    default:
                        break;
                }
                Splasher.Close(this);
            }
            catch /*(System.Exception ex)*/
            {
                Splasher.Close(this);
                this.Close();
            }
            try
            {
                //
                //ExcelOperater eo = new ExcelOperater();
                //eo.CreateExcelSheet("北京交通发展研究中心电话号码表");
                //eo.DataTableToExcelSheet(0, 1, "北京交通发展研究中心电话号码表", dtContracts, "北京交通发展研究中心电话号码表", exptype);
                //eo.Save(sfdContractsEXP.FileName);
                //MessageBox.Show("导出成功!");
                //
                Splasher.Show(typeof(frmLoading));
                Export(exptype, expTypeStr, dtContracts);
                Splasher.Close(this);
                MessageBox.Show("导出成功!");
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message, ex);
                Splasher.Close(this);
                MessageBox.Show("导出失败:" + ex.Message.ToString());
            }
        }

        private void Export(Enums.ExpType exptype, string expTypeStr, DataTable dtContracts)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ICellStyle style = hssfworkbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "通讯录" + expTypeStr;
            hssfworkbook.SummaryInformation = si;
            ISheet sheet = hssfworkbook.CreateSheet("通讯录");
            IRow row = sheet.CreateRow(0);
            for (int j = 0; j < dtContracts.Columns.Count + 1; j++)
            {
                ICell cell = row.CreateCell(j);
                if (j == 0)
                {
                    cell.SetCellValue("序号");
                }
                else
                {
                    cell.SetCellValue(dtContracts.Columns[j - 1].ColumnName);
                }
                IFont font = hssfworkbook.CreateFont();
                font.FontHeight = 15 * 15;
                ICellStyle styleHeader = hssfworkbook.CreateCellStyle();
                styleHeader.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                styleHeader.SetFont(font);
                styleHeader.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                styleHeader.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                styleHeader.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                styleHeader.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                styleHeader.TopBorderColor = HSSFColor.BLACK.index;
                styleHeader.BottomBorderColor = HSSFColor.BLACK.index;
                styleHeader.LeftBorderColor = HSSFColor.BLACK.index;
                styleHeader.RightBorderColor = HSSFColor.BLACK.index;
                cell.CellStyle = styleHeader;

            }
            for (int i = 1; i < dtContracts.Rows.Count + 1; i++)
            {
                row = sheet.CreateRow(i);
                for (int j = 0; j < dtContracts.Columns.Count + 1; j++)
                {
                    ICell cell = row.CreateCell(j);
                    IFont font = hssfworkbook.CreateFont();
                    font.FontHeight = 15 * 15;
                    ICellStyle deptstyle = hssfworkbook.CreateCellStyle();
                    deptstyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                    deptstyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
                    deptstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                    deptstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
                    deptstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
                    deptstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
                    deptstyle.TopBorderColor = HSSFColor.BLACK.index;
                    deptstyle.BottomBorderColor = HSSFColor.BLACK.index;
                    deptstyle.LeftBorderColor = HSSFColor.BLACK.index;
                    deptstyle.RightBorderColor = HSSFColor.BLACK.index;
                    deptstyle.SetFont(font);
                    if (exptype == Enums.ExpType.Dept && dtContracts.Rows[i - 1]["部门"].ToString() != "" && j > 2)
                    {
                        deptstyle.FillForegroundColor = HSSFColor.GREY_25_PERCENT.index;
                        deptstyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
                    }
                    cell.CellStyle = deptstyle;
                    if (j == 0)
                    {
                        cell.SetCellValue(i.ToString());
                    }
                    else
                    {

                        cell.SetCellValue(dtContracts.Rows[i - 1][j - 1].ToString());
                    }
                }
            }
            for (int i = 0; i < dtContracts.Columns.Count + 1; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            if (sheet.GetRow(0).GetCell(2).ToString() == "房间")
            {
                int i = 1, j = 1;
                while (i < sheet.LastRowNum && j < sheet.LastRowNum)
                {
                    j = i + 1;
                    while (sheet.GetRow(i).GetCell(2).ToString() != "" && sheet.GetRow(j).GetCell(1).ToString() == "" && sheet.GetRow(i).GetCell(2).ToString() == sheet.GetRow(j).GetCell(2).ToString())
                    {
                        j++;
                    }
                    if (j - 1 - i >= 1)
                    {
                        NPOI.SS.Util.CellRangeAddress region = new NPOI.SS.Util.CellRangeAddress(i, j - 1, 2, 2);
                        sheet.AddMergedRegion(region);
                        ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.THIN, NPOI.HSSF.Util.HSSFColor.BLACK.index);
                    }
                    i = j;
                }
            }
            if (sheet.GetRow(0).GetCell(1).ToString() == "部门")
            {
                int i = 1, j = 1;
                while (i < sheet.LastRowNum && j < sheet.LastRowNum)
                {
                    j = i + 1;
                    while (sheet.GetRow(j).GetCell(1).ToString() == "")
                    {
                        j++;
                        if (j > sheet.LastRowNum)
                        {
                            break;
                        }
                    }
                    if (j - 1 - i >= 1)
                    {
                        NPOI.SS.Util.CellRangeAddress region = new NPOI.SS.Util.CellRangeAddress(i, j - 1, 1, 1);
                        sheet.AddMergedRegion(region);
                        ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.THIN, NPOI.HSSF.Util.HSSFColor.BLACK.index);
                    }
                    i = j;
                }
            }
            FileStream file = new FileStream(sfdContractsEXP.FileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }

        private void ResizeAndCenterMap()
        {
            if (tctlMain.SelectedTab == tpgView)
            {
                ResizePanel(pnlBaseMap, picBaseMap);
                CenterPic(pnlBaseMap, picBaseMap);
            }
            else
            {
                if (tctlEditFloor.SelectedTab == tpgAddFloor)
                {
                    ResizePanel(pnlBaseMapPreview, picBaseMapPreview);
                    CenterPic(pnlBaseMapPreview, picBaseMapPreview);
                }
                else if (tctlEditFloor.SelectedTab == tpgSetSeat)
                {
                    ResizePanel(pnlSetSeatMap, picSetSeatMap);
                    CenterPic(pnlSetSeatMap, picSetSeatMap);
                }
                else if (tctlEditFloor.SelectedTab == tpgViewFloor)
                {
                    RefreshdgrdFloorView(floorTable);
                }
            }


        }
        private void frmView_Resize(object sender, EventArgs e)
        {
            ResizeAndCenterMap();
        }

        private void txtFloorCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunction.InputEngNumber(sender, e);
        }

        private void picOnline_MouseHover(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            ttOnlinePic.Show("", pic, 0);
            ttOnlinePic.Show("在线", pic, 0, pic.Height * -1, 2000);

        }

        private void picOnline2_MouseHover(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            ttOnlinePic.Show("", pic, 0);
            ttOnlinePic.Show("在线（非本人）", pic, 0, pic.Height * -1, 2000);

        }

        private void picOffline_MouseHover(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            ttOnlinePic.Show("", pic, 0);
            ttOnlinePic.Show("离线", pic, 0, pic.Height * -1, 2000);
        }

        private void picContractsEXP_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.export_BUTTON_down;

        }

        private void picContractsEXP_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.export_BUTTON_up;

        }

        private void picContractsEXP_MouseHover(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.export_BUTTON_over;
            ttOnlinePic.Show("", pic, 0);
            ttOnlinePic.Show("导出通讯录", pic, 0, pic.Height * -1, 2000);
        }

        private void picContractsEXP_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.export_BUTTON_up;
        }

        private void picContractsEXP_Click(object sender, EventArgs e)
        {
            ExportContracts();
        }

        private void picExit_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.Exit_down;
        }

        private void picExit_MouseHover(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.Exit_over;
            ttOnlinePic.Show("", pic, 0);
            ttOnlinePic.Show("退出", pic, 0, pic.Height * -1, 2000);

        }

        private void picExit_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.Exit_up;

        }

        private void picExit_MouseUp(object sender, MouseEventArgs e)
        {

            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.Exit_up;
        }

        private void SaveFloorConfig()
        {
            IniFile inifile = new IniFile("config.ini");
            inifile.WriteInt("LOCAL_CONFIG", "cboFloorIndex", _fl.ComboFloorIndex);
        }
        private void picExit_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否退出？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            try
            {
                Splasher.Show(typeof(frmLoading));
                client.UserLogout(CommonFunction.GetIPAddress());
                Splasher.Close(this);
                if (_fl.notifyIconSeat != null)
                {
                    _fl.notifyIconSeat.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            finally
            {
                SaveFloorConfig();
                Splasher.Close(this);
                Environment.Exit(0);
            }
        }

        private void picHisQuery_Click(object sender, EventArgs e)
        {
            frmEmployeeStatus fes = new frmEmployeeStatus();
            fes.ShowDialog();
        }

        private void picHisQuery_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.UserInline_down;
        }

        private void picHisQuery_MouseHover(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.UserInline_over;
            ttOnlinePic.Show("", pic, 0);
            ttOnlinePic.Show("员工历史记录查询", pic, 0, pic.Height * -1, 2000);

        }

        private void picHisQuery_MouseLeave(object sender, EventArgs e)
        {

            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.UserInline_up;
        }

        private void picHisQuery_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            pic.Image = HRSeat.Properties.Resources.UserInline_up;

        }

        private void btnTpgSetSeatCancel_Click(object sender, EventArgs e)
        {
            tctlEditFloor.SelectedTab = tpgViewFloor;
        }

        #region Hide TabControl tabs
        private void tctlEditFloor_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pn = new Pen(Color.White, 10);
            g.DrawRectangle(pn, tctlEditFloor.SelectedTab.Bounds);
        }

        private void tpgViewFloor_Paint(object sender, PaintEventArgs e)
        {
            this.tctlEditFloor.Region = new Region(new RectangleF(this.tctlEditFloor.Left, this.tctlEditFloor.Top + 1, this.tctlEditFloor.Width, this.tctlEditFloor.Height));
        }
        #endregion Hide TabControl tabs

        private void frmView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Dispose
                RefreshTimer.Dispose();
                Splasher.Close(this);
                dicFloorImages.Clear();
                client.Close();
                if (floorTable != null)
                {
                    floorTable.Dispose();
                }
                if (roomTable != null)
                {
                    roomTable.Dispose();
                }
                if (seatTable != null)
                {
                    seatTable.Dispose();
                }
                if (employeeTable != null)
                {
                    employeeTable.Dispose();
                }
                if (internTable != null)
                {
                    internTable.Dispose();
                }
                if (employeeTmpTable != null)
                {
                    employeeTmpTable.Dispose();
                }
            }
            catch
            {
            }
            finally
            {
                if (_fl != null)
                {
                    _fl.DisposeFrmView();
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
