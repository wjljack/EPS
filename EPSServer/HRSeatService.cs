using DWMSCrpyt;
using HRSeatServer.DataSet_HRSeatTableAdapters;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Caching;

namespace HRSeatServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class HRSeatService : IHRSeat
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static List<string> forceOfflineClients = new List<string>();
        public static List<string> logoutList = new List<string>();

        //WCF cache
        private static ObjectCache cache = MemoryCache.Default;

        //强制下线栈 利用 CheckOnline可以使客户端强制下线
        //onlineClinets string->employeeid string->ip
        private static Dictionary<string, string> onlineClients = new Dictionary<string, string>();

        private static DateTime seatRefreshTimeStamp = DateTime.Now;

        /// <summary>
        /// 检测在线状态
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="onlineStatus"></param>
        /// <returns>1在线 0下线 2强制下线</returns>
        public int CheckOnline(string ip, int onlineStatus)
        {
            //不在在线列表 且需要强制下线
            //挤下线情况
            if (HRSeatService.forceOfflineClients.Contains(ip) && !onlineClients.ContainsValue(ip))
            {
                //发出强制下线请求 从强制下线列表移除
                HRSeatService.forceOfflineClients.Remove(ip);
                UserLogout(ip);
                return 2;
            }
            //不在线且登陆为在线状态 强制下线
            if (!onlineClients.ContainsValue(ip) && onlineStatus != 0)
            {
                HRSeatService.forceOfflineClients.Remove(ip);
                UserLogout(ip);
                return 0;
            }
            DataSet_HRSeat.Hr_EP_SeatDataTable dt = new Hr_EP_SeatTableAdapter().GetSeatByIP(ip);
            //如果没有座位 登陆的时候却坐在自己的座位
            //修改ip后的强制下线
            if (dt.Count == 0 && onlineStatus == 1)
            {
                HRSeatService.forceOfflineClients.Remove(ip);
                UserLogout(ip);
                return 0;
            }
            //如果有座位 且数据库为掉线状态 但是登陆的时候不是掉线的
            //bug mark
            if (dt.Count == 1 && dt[0].OnlineStatus == 0 && onlineStatus != 0)
            {
                forceOfflineClients.Remove(ip);
                UserLogout(ip);
                return 0;
            }
            return 1;
        }

        public bool DeleteFloor(int floorid)
        {
            UpdateTableAdapter _adapter = new UpdateTableAdapter();
            try
            {
                _adapter.DeleteSeatsByFloorID(floorid);
                _adapter.DeleteRoomsByFloorID(floorid);
                _adapter.DeleteFloorByFloorID(floorid);
                return true;
            }
            catch /*(System.Exception ex)*/
            {
                return false;
            }
        }

        public bool DeleteRoomsAndSeats(int floorid)
        {
            UpdateTableAdapter _adapter = new UpdateTableAdapter();
            try
            {
                _adapter.DeleteSeatsByFloorID(floorid);
                _adapter.DeleteRoomsByFloorID(floorid);
                return true;
            }
            catch /*(System.Exception ex)*/
            {
                return false;
            }
        }

        public bool DeleteSeatByRoomID(int roomid)
        {
            UpdateTableAdapter _adapter = new UpdateTableAdapter();
            try
            {
                _adapter.DeleteSeatsByRoomID(roomid);
                return true;
            }
            catch /*(System.Exception ex)*/
            {
                return false;
            }
        }

        public bool DeleteSeatBySeatID(int seatid)
        {
            UpdateTableAdapter _adapter = new UpdateTableAdapter();
            try
            {
                _adapter.DeleteOnlineHisBySeatID(seatid);
                _adapter.DeleteSeatBySeatID(seatid);
                return true;
            }
            catch /*(System.Exception ex)*/
            {
                return false;
            }
        }

        public bool DeleteSeatsByFloorID(int floorid)
        {
            UpdateTableAdapter _adapter = new UpdateTableAdapter();
            try
            {
                _adapter.DeleteSeatsByFloorID(floorid);
                return true;
            }
            catch /*(System.Exception ex)*/
            {
                return false;
            }
        }

        public DataSet_HRSeat.ContactsEXPByNameDataTable ExportContractsByName()
        {
            ContactsEXPByNameTableAdapter _adapter = new ContactsEXPByNameTableAdapter();
            return _adapter.GetData();
        }
        public DataSet_HRSeat.ContactsEXPDataTable ExportContracts()
        {
            ContactsEXPTableAdapter _adapter = new ContactsEXPTableAdapter();
            return _adapter.GetData();
        }

        public DataSet_HRSeat.Hr_Employment_DivisionDataTable GetAllDivisions()
        {
            Hr_Employment_DivisionTableAdapter _adapter = new Hr_Employment_DivisionTableAdapter();
            return _adapter.GetData();
        }

        public DataSet_HRSeat.Hr_EP_FloorDataTable GetAllFloors()
        {
            Hr_EP_FloorTableAdapter _adapter = new Hr_EP_FloorTableAdapter();
            return _adapter.GetData();
        }

        public DataSet_HRSeat.Hr_EP_FloorNoImgDataTable GetAllFloorsNoImg()
        {
            Hr_EP_FloorNoImgTableAdapter _adapter = new Hr_EP_FloorNoImgTableAdapter();
            return _adapter.GetData();
        }

        //...
        public DataSet_HRSeat.Hr_EP_SeatDataTable GetAllSeats()
        {
            Hr_EP_SeatTableAdapter _adapter = new Hr_EP_SeatTableAdapter();
            return _adapter.GetData();
        }

        public DataSet_HRSeat.Hr_Employment_DivisionDataTable GetDivisionByEmployeeID(int employeeid)
        {
            Hr_Employment_PositionTableAdapter _adapter = new Hr_Employment_PositionTableAdapter();
            DataSet_HRSeat.Hr_Employment_PositionDataTable posTable = _adapter.GetPositionByEmployeeID(employeeid);
            if (posTable.Count != 0)
            {
                Hr_Employment_DivisionTableAdapter _divadapter = new Hr_Employment_DivisionTableAdapter();
                return _divadapter.GetDivisionByID(posTable[0].DivisionID);
            }
            return new DataSet_HRSeat.Hr_Employment_DivisionDataTable();
        }

        public DataSet_HRSeat.DivisionEmployeeDataTable GetDivisionEmployee()
        {
            DivisionEmployeeTableAdapter _adapter = new DivisionEmployeeTableAdapter();
            return _adapter.GetData();
        }

        public DataSet_HRSeat.DivisionEmployeeDataTable GetDivisionEmployeeNoSeat()
        {
            DivisionEmployeeTableAdapter _adapter = new DivisionEmployeeTableAdapter();
            return _adapter.GetEmployeeByNoSeat();
        }

        public DataSet_HRSeat.Hr_EmployeeDataTable GetEmployeeByFloorID(int floorid)
        {
            Hr_EmployeeTableAdapter _adapter = new Hr_EmployeeTableAdapter();
            return _adapter.GetEmployeeByFloorID(floorid);
        }

        public DataSet_HRSeat.Hr_EmployeeDataTable GetEmployeeByID(int id)
        {
            Hr_EmployeeTableAdapter _adapter = new Hr_EmployeeTableAdapter();
            return _adapter.GetEmployeeByID(id);
        }

        public DataSet_HRSeat.Hr_EmployeeDataTable GetEmployeeByNoSeat()
        {
            Hr_EmployeeTableAdapter _adapter = new Hr_EmployeeTableAdapter();
            return _adapter.GetEmployeeByNoSeat();
        }

        public int GetEmployeeIDByUserName(string username)
        {
            Hr_RBAC_UserTableAdapter _adapter = new Hr_RBAC_UserTableAdapter();
            DataSet_HRSeat.Hr_RBAC_UserDataTable _dt = _adapter.GetDataByUserName(username);
            if (_dt.Count == 1)
            {
                if (_dt[0]["EmployeeID"].ToString() == "")
                {
                    return -1;
                }
                else
                {

                    return _dt[0].EmployeeID;
                }
            }
            else
            {
                return -1;
            }
        }
        public string GetEmployeeNameByIDEmpType(int employeeid, int employeetype)
        {
            switch (employeetype)
            {
                //正式员工
                case 0:
                    Hr_EmployeeTableAdapter _employeeAdapter = new Hr_EmployeeTableAdapter();
                    DataSet_HRSeat.Hr_EmployeeDataTable employeeTable = _employeeAdapter.GetEmployeeByID(employeeid);
                    if (employeeTable.Count != 0)
                    {
                        return employeeTable[0].FullName;
                    }
                    else return "";
                    break;
                //实习生
                case 1:
                    Hr_InternTableAdapter _internAdapter = new Hr_InternTableAdapter();
                    DataSet_HRSeat.Hr_InternDataTable internTable = _internAdapter.GetInternByID(employeeid);
                    if (internTable.Count != 0)
                    {
                        return internTable[0].FullName;
                    }
                    else return "";
                    break;
                //临时人员
                case 2:
                    Hr_Employee_TemporaryTableAdapter _employeeTmpAdapter = new Hr_Employee_TemporaryTableAdapter();
                    DataSet_HRSeat.Hr_Employee_TemporaryDataTable employeeTmpTable = _employeeTmpAdapter.GetEmployeeTmpByID(employeeid);
                    if (employeeTmpTable.Count != 0)
                    {
                        return employeeTmpTable[0].FullName;
                    }
                    else return "";
                    break;
                default:
                    return "";
                    break;
            }
        }

        public string GetEmployeeNameByID(int employeeid)
        {
            Hr_EmployeeTableAdapter _employeeAdapter = new Hr_EmployeeTableAdapter();
            Hr_Employee_TemporaryTableAdapter _employeeTmpAdapter = new Hr_Employee_TemporaryTableAdapter();
            DataSet_HRSeat.Hr_EmployeeDataTable employeeTable = _employeeAdapter.GetEmployeeByID(employeeid);
            DataSet_HRSeat.Hr_Employee_TemporaryDataTable employeeTmpTable = _employeeTmpAdapter.GetEmployeeTmpByID(employeeid);
            if (employeeTable.Count != 0)
            {
                return employeeTable[0].FullName;
            }
            else if (employeeTmpTable.Count != 0)
            {
                return employeeTmpTable[0].FullName;
            }
            else return "";
        }

        //public SeatMap gettest()
        //{
        //    return new SeatMap();
        //}
        public string GetEmployeeNameBySeatID(int seatid)
        {
            Hr_EP_SeatTableAdapter _adapater = new Hr_EP_SeatTableAdapter();
            DataSet_HRSeat.Hr_EP_SeatDataTable seatTable = null;
            seatTable = _adapater.GetSeatByID(seatid);
            if (seatTable.Count == 0)
            {
                return "";
            }
            int employeeid = seatTable[0].EmployeeID;
            return GetEmployeeNameByID(employeeid);
        }

        public DataSet_HRSeat.EmployeeStatusDataTable GetEmployeeStatus(int employeeid, int employeetype, DateTime begin, DateTime end, ref DateTime serverTime)
        {
            serverTime = DateTime.Now;
            EmployeeStatusTableAdapter _adapter = new EmployeeStatusTableAdapter();
            return _adapter.GetData(employeeid, employeetype, begin, end);
        }

        public DataSet_HRSeat.Hr_Employee_TemporaryDataTable GetEmployeeTmpByFloorID(int floorid)
        {
            Hr_Employee_TemporaryTableAdapter _adapter = new Hr_Employee_TemporaryTableAdapter();
            return _adapter.GetEmployeeTmpByFloorID(floorid);
        }

        public DataSet_HRSeat.Hr_Employee_TemporaryDataTable GetEmployeeTmpByID(int id)
        {
            Hr_Employee_TemporaryTableAdapter _adapter = new Hr_Employee_TemporaryTableAdapter();
            return _adapter.GetEmployeeTmpByID(id);
        }
        public long GetEmployeeTmpNewID()
        {
            Hr_Employee_TemporaryTableAdapter _adapter = new Hr_Employee_TemporaryTableAdapter();
            DataSet_HRSeat.Hr_Employee_TemporaryDataTable dt = _adapter.GetNewIDData();
            if (dt.Count == 1)
            {
                return dt[0].ID;
            }
            else
            {
                return -1;
            }
            return _adapter.GetNewIDData()[0].ID;
        }
        public DataSet_HRSeat.Hr_Employee_TemporaryDataTable GetEmployeeTmp()
        {
            Hr_Employee_TemporaryTableAdapter _adapter = new Hr_Employee_TemporaryTableAdapter();
            return _adapter.GetData();
        }

        public DataSet_HRSeat.Hr_Employee_TemporaryDataTable GetEmployeeTmpByNoSeat()
        {
            Hr_Employee_TemporaryTableAdapter _adapter = new Hr_Employee_TemporaryTableAdapter();
            return _adapter.GetEmployeeTmpByNoSeat();
        }

        public DataSet_HRSeat.Hr_EP_FloorDataTable GetFloorByID(int id)
        {
            Hr_EP_FloorTableAdapter _adapter = new Hr_EP_FloorTableAdapter();
            return _adapter.GetFloorByID(id);
        }

        public DataSet_HRSeat.Hr_InternDataTable GetInternByID(int id)
        {
            Hr_InternTableAdapter _adapter = new Hr_InternTableAdapter();
            return _adapter.GetInternByID(id);
        }

        public DataSet_HRSeat.Hr_InternDataTable GetInternByFloorID(int floorid)
        {
            Hr_InternTableAdapter _adapter = new Hr_InternTableAdapter();
            return _adapter.GetInternByFloorID(floorid);
        }

        public DataSet_HRSeat.Hr_InternDataTable GetIntern()
        {
            Hr_InternTableAdapter _adapter = new Hr_InternTableAdapter();
            return _adapter.GetDataByUnidentified();
        }

        public DataSet_HRSeat.Hr_InternDataTable GetInternByNoSeat()
        {
            Hr_InternTableAdapter _adapter = new Hr_InternTableAdapter();
            return _adapter.GetInternByNoSeat();
        }

        public DataSet_HRSeat.Hr_EP_RoomDataTable GetRoomByFloorID(int floorid)
        {
            Hr_EP_RoomTableAdapter _adapter = new Hr_EP_RoomTableAdapter();
            return _adapter.GeRoomsByFloorID(floorid);
        }

        public DataSet_HRSeat.Hr_EP_RoomDataTable GetRoomByID(int id)
        {
            Hr_EP_RoomTableAdapter _adapter = new Hr_EP_RoomTableAdapter();
            return _adapter.GetRoomByID(id);
        }

        public DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatAllHistory(string ipAddress)
        {
            Hr_EP_Online_HistoryTableAdapter _historyAdapter = new Hr_EP_Online_HistoryTableAdapter();
            return _historyAdapter.GetData();
        }

        public DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatHistoryByIP(string ipAddress, ref DateTime serverTime)
        {
            serverTime = DateTime.Now;
            Hr_EP_Online_HistoryTableAdapter _historyAdapter = new Hr_EP_Online_HistoryTableAdapter();
            return _historyAdapter.GetDataByIP(ipAddress);
        }

        public DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatHistoryByIPDay(string ipAddress, uint day, ref DateTime serverTime)
        {
            serverTime = DateTime.Now;
            DateTime queryDate = serverTime.AddDays(day * -1);
            Hr_EP_Online_HistoryTableAdapter _historyAdapter = new Hr_EP_Online_HistoryTableAdapter();
            return _historyAdapter.GetDataByIPDay(ipAddress, queryDate);
        }
        public DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatHistoryByIDType(int employeeid, int employeetype)
        {
            Hr_EP_Online_HistoryTableAdapter _historyAdapter = new Hr_EP_Online_HistoryTableAdapter();
            return _historyAdapter.GetDataByIDEmpType(employeeid, employeetype);
        }

        public DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatHistoryByIPType(string ipAddress, int employeetype)
        {
            Hr_EP_Online_HistoryTableAdapter _historyAdapter = new Hr_EP_Online_HistoryTableAdapter();
            return _historyAdapter.GetDataByIPEmpType(ipAddress, employeetype);
        }

        public DataSet_HRSeat.Hr_EP_SeatDataTable GetSeatsByFloorID(int floorid)
        {
            Hr_EP_SeatTableAdapter _adapter = new Hr_EP_SeatTableAdapter();
            return _adapter.GetSeatsByFloorID(floorid);
        }
        public DataSet_HRSeat.Hr_EP_SeatDataTable GetSeatByIP(string ip)
        {
            Hr_EP_SeatTableAdapter _adapter = new Hr_EP_SeatTableAdapter();
            return _adapter.GetSeatByIP(ip);
        }
        public DataSet_HRSeat.Hr_EP_SeatDataTable GetSeatsByFloorIDRefresh(int floorid)
        {
            string CacheKey = "GetSeatsByFloorIDRefresh" + floorid.ToString();
            if (cache.Contains(CacheKey))
                return (DataSet_HRSeat.Hr_EP_SeatDataTable)cache.Get(CacheKey);
            else
            {
                Hr_EP_SeatTableAdapter _adapter = new Hr_EP_SeatTableAdapter();
                DataSet_HRSeat.Hr_EP_SeatDataTable seatTableRefresh = _adapter.GetSeatsByFloorID(floorid);
                // Store data in the cache
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddSeconds(5);
                cache.Add(CacheKey, seatTableRefresh, cacheItemPolicy);

                return seatTableRefresh;
            }
        }

        public bool UpdateEmployee(DataSet_HRSeat.Hr_EmployeeDataTable employeeTable)
        {
            if (employeeTable.Count != 1)
            {
                return false;
            }
            try
            {
                DataSet_HRSeat.Hr_EmployeeRow tmpRow = employeeTable[0];

                Hr_EmployeeTableAdapter _apdater = new Hr_EmployeeTableAdapter();
                DataSet_HRSeat.Hr_EmployeeDataTable _table = _apdater.GetEmployeeByID(employeeTable[0].ID);
                DataSet_HRSeat.Hr_EmployeeRow _row = _table[0];
                _row.WorkEmail = tmpRow.WorkEmail;
                _row.Mobile = tmpRow.Mobile;
                _row.WorkTelephone = tmpRow.WorkTelephone;
                if (_apdater.Update(_row) == 1)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public bool UpdateEmployeeTmp(DataSet_HRSeat.Hr_Employee_TemporaryDataTable employeeTmpTable)
        {
            if (employeeTmpTable.Count != 1)
            {
                return false;
            }
            try
            {
                DataSet_HRSeat.Hr_Employee_TemporaryRow tmpRow = employeeTmpTable[0];

                Hr_Employee_TemporaryTableAdapter _apdater = new Hr_Employee_TemporaryTableAdapter();
                DataSet_HRSeat.Hr_Employee_TemporaryDataTable _table = _apdater.GetEmployeeTmpByID(employeeTmpTable[0].ID);
                if (_table.Count == 1)
                {
                    DataSet_HRSeat.Hr_Employee_TemporaryRow _row = _table[0];
                    _row.FullName = tmpRow.FullName;
                    _row.WorkEmail = tmpRow.WorkEmail;
                    _row.Mobile = tmpRow.Mobile;
                    _row.WorkTelephone = tmpRow.WorkTelephone;
                    if (_apdater.Update(_row) == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    int count = _apdater.Insert(string.Empty, employeeTmpTable[0].FullName, string.Empty, DateTime.MinValue, string.Empty, string.Empty, 3, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, employeeTmpTable[0].Mobile, string.Empty, string.Empty, employeeTmpTable[0].WorkTelephone, string.Empty, string.Empty, 4, 2, 1, false, DateTime.Now, DateTime.MinValue, string.Empty, employeeTmpTable[0].WorkTelephone, employeeTmpTable[0].WorkEmail, string.Empty, 0, string.Empty);
                    if (count == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public bool UpdateFloor(DataSet_HRSeat.Hr_EP_FloorDataTable floorTable)
        {
            Hr_EP_FloorTableAdapter _adapter = new Hr_EP_FloorTableAdapter();

            if (floorTable.Count == 1)
            {
                DataSet_HRSeat.Hr_EP_FloorDataTable dt = _adapter.GetFloorByID(floorTable[0].ID);
                if (dt.Count == 1)
                {
                    dt[0].Code = floorTable[0].Code;
                    dt[0].Name = floorTable[0].Name;
                    dt[0].Image = floorTable[0].Image;
                    dt[0].ImageHeight = floorTable[0].ImageHeight;
                    dt[0].ImageWidth = floorTable[0].ImageWidth;
                    //DataSet_HRSeat.Hr_EP_FloorRow row = floorTable[0];
                    int count = _adapter.Update(dt);
                    if (count == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    int count = _adapter.Insert(floorTable[0].Code, floorTable[0].Name, floorTable[0].ImageHeight, floorTable[0].ImageWidth, floorTable[0].Image);
                    if (count == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                int count = _adapter.Update(floorTable);
                if (count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdateIntern(DataSet_HRSeat.Hr_InternDataTable internTable)
        {
            if (internTable.Count != 1)
            {
                return false;
            }
            try
            {
                DataSet_HRSeat.Hr_InternRow tmpRow = internTable[0];

                Hr_InternTableAdapter _apdater = new Hr_InternTableAdapter();
                DataSet_HRSeat.Hr_InternDataTable _table = _apdater.GetInternByID(internTable[0].ID);
                DataSet_HRSeat.Hr_InternRow _row = _table[0];
                //_row.email = tmpRow.WorkEmail;
                _row.Mobile = tmpRow.Mobile;
                _row.HomeTelephone = tmpRow.HomeTelephone;
                if (_apdater.Update(_row) == 1)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public void UpdateOnlineHisAllOffline()
        {
            UpdateTableAdapter _adapter = new UpdateTableAdapter();
            _adapter.UpdateOnlineHisAllOffline();
        }

        public void UpdateOnlineStatusAllOffline()
        {
            UpdateTableAdapter _adapter = new UpdateTableAdapter();
            _adapter.UpdateOnlineStatusAllOffline();
        }

        public bool UpdateRooms(DataSet_HRSeat.Hr_EP_RoomDataTable roomTable)
        {
            Hr_EP_RoomTableAdapter _adapter = new Hr_EP_RoomTableAdapter();

            int count = _adapter.Update(roomTable);
            if (count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateSeats(DataSet_HRSeat.Hr_EP_SeatDataTable seatTable)
        {
            Hr_EP_SeatTableAdapter _adapter = new Hr_EP_SeatTableAdapter();
            int count = 0;
            //if (_adapter.GetData().Count == 0)
            //{
            //    DataSet_HRSeat.Hr_EP_SeatRow row = seatTable[0];
            //    count = _adapter.Insert(row.Code, row.PosX, row.PosY, row.OnlineStatus, row.EmployeeID, row.EmployeeType, row.IP, row.RoomID);
            //    seatTable.Rows.RemoveAt(0);
            //    seatTable.AcceptChanges();
            //}
            count += _adapter.Update(seatTable);
            if (count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 正式人员登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ip">登陆机器ip地址</param>
        /// <param name="isadmin">是否管理员登陆</param>
        /// <returns></returns>
        public LoginReturnStatus UserLogin(string username, string password, string ip, out bool isadmin)
        {
            forceOfflineClients.Remove(ip);
            isadmin = false;

            Hr_RBAC_UserTableAdapter _userAdapter = new Hr_RBAC_UserTableAdapter();
            DataSet_HRSeat.Hr_RBAC_UserDataTable _dtUser = _userAdapter.GetDataByUserName(username);
            //没有此用户
            if (_dtUser.Count == 0)
            {
                return LoginReturnStatus.NOUSER;
            }
            //密码错误
            if (!CryptHelp.CompareHash(password, _dtUser[0].Password))
            {
                return LoginReturnStatus.PWDERROR;
            }

            string employeeid = _dtUser[0]["EmployeeID"].ToString();
            int iemployeeid;
            if (employeeid == "")
            {
                iemployeeid = -1;
            }
            else
            {
                iemployeeid = _dtUser[0].EmployeeID;
            }
            long userId = _dtUser[0].ID;

            Hr_RBAC_GetRolesTableAdapter _rolesAdapter = new Hr_RBAC_GetRolesTableAdapter();
            DataSet_HRSeat.Hr_RBAC_GetRolesDataTable _dtRoles = _rolesAdapter.GetRolesByUserID(userId);
            //检测登陆的是否为管理员
            foreach (DataSet_HRSeat.Hr_RBAC_GetRolesRow rowRole in _dtRoles)
            {
                if (rowRole.rolename == "座位管理员")
                {
                    isadmin = true;
                    break;
                }
            }

            UpdateTableAdapter _updateAdapter = new UpdateTableAdapter();
            Hr_EP_SeatTableAdapter _adapater = new Hr_EP_SeatTableAdapter();
            Hr_EmployeeTableAdapter _employeeAdapter = new Hr_EmployeeTableAdapter();
            DataSet_HRSeat.Hr_EmployeeDataTable employeeTable = _employeeAdapter.GetEmployeeByID(iemployeeid);
            DataSet_HRSeat.Hr_EP_SeatDataTable seatTable = null;
            //如果有此用户
            if (employeeTable.Count == 1)
            {
                Hr_EP_Online_HistoryTableAdapter _historyAdapter = new Hr_EP_Online_HistoryTableAdapter();

                //如果账号已经登录 且不是本机登陆 则将登录的机器强制下线
                if (onlineClients.ContainsKey(employeeid) && onlineClients[employeeid] != ip)
                {
                    if (!forceOfflineClients.Contains(onlineClients[employeeid]))
                    {
                        forceOfflineClients.Add(onlineClients[employeeid]);
                    }
                    onlineClients[employeeid] = ip;
                }

                //将用户加入到在线列表
                if (!onlineClients.ContainsKey(employeeid))
                {
                    onlineClients.Add(employeeid, ip);
                }

                //获取此用户的座位
                seatTable = _adapater.GetSeatByIP(ip);
                //没有设置此人的座位
                if (seatTable.Count == 0)
                {
                    _historyAdapter.Insert(null, DateTime.Now, null, ip, int.Parse(employeeid), 0);
                    return LoginReturnStatus.NOUSERSEAT;
                    ///
                }

                ////之前登陆的没下线
                //if (seatTable[0].OnlineStatus != 0)
                //{
                //    //如果有别人在座 挤下来
                //    if (seatTable[0].EmployeeID != int.Parse(employeeid))
                //    {
                //        //先下线
                //        UserLogout(ip);
                //        // _updateAdapter.UpdateOnlineStatusByIP(1, ip);
                //        //自己登陆历史
                //        //_historyAdapter.Insert(seatTable[0].ID, DateTime.Now, null, ip, int.Parse(employeeid));
                //        //return LoginReturnStatus.OK;
                //    }
                //    //return LoginReturnStatus.ALREADYOK;
                //    ///
                //}

                //没人坐上去
                //自己坐在自己的位置
                //bugfixed 还需要比对雇员类型
                if (seatTable[0]["EmployeeID"].ToString() == employeeid && seatTable[0]["EmployeeType"].ToString() == "0")
                {
                    //状态改为自己坐在自己的位置
                    _updateAdapter.UpdateOnlineStatusByIP(1, ip);
                    DataSet_HRSeat.Hr_EP_Online_HistoryDataTable hisTable = _historyAdapter.GetDataByIP(ip);
                    //添加登陆历史
                    if (hisTable.Count == 0 || hisTable.Count != 0 && hisTable[0]["OfflineTime"] != DBNull.Value)
                    {
                        _historyAdapter.Insert(seatTable[0].ID, DateTime.Now, null, ip, int.Parse(employeeid), 0);
                    }
                    return LoginReturnStatus.OK;
                }
                //其他人坐在这个座位
                else
                {
                    //他原来的位置
                    DataSet_HRSeat.Hr_EP_SeatDataTable seatTableOthers = _adapater.GetSeatsByEmpIDType(int.Parse(employeeid), 0);
                    _updateAdapter.UpdateOnlineStatusByIP(2, ip);
                    //这样id和ip不匹配 表示是其他人坐上去了 通过在历史表中查id 可以查到这个机器是被哪个“其他人”使用过
                    if (seatTableOthers.Count != 0)
                    {
                        DataSet_HRSeat.Hr_EP_Online_HistoryDataTable hisTable = _historyAdapter.GetDataByIP(ip);
                        if (hisTable.Count == 0 || hisTable.Count != 0 && hisTable[0]["OfflineTime"] != DBNull.Value)
                        {
                            _historyAdapter.Insert(seatTableOthers[0].ID, DateTime.Now, null, ip, int.Parse(employeeid), 0);
                        }
                        return LoginReturnStatus.WRONGSEAT;
                    }
                    _historyAdapter.Insert(null, DateTime.Now, null, ip, int.Parse(employeeid), 0);
                    return LoginReturnStatus.NOUSERSEAT;
                }
            }
            else
            {
                //用户不存在
                return LoginReturnStatus.NOUSER;
            }
        }

        /// <summary>
        /// 非正式人员登陆
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public LoginReturnStatus UserLoginAnonymous(string ip, string name)
        {
            forceOfflineClients.Remove(ip);

            //检测用户是否存在

            Hr_EP_SeatTableAdapter _seatAdapter = new Hr_EP_SeatTableAdapter();
            DataSet_HRSeat.Hr_EP_SeatDataTable _seatTable = _seatAdapter.GetSeatByIP(ip);
            //没有分配座位
            if (_seatTable.Count == 0)
            {
                return LoginReturnStatus.NOUSERSEAT;
            }
            //获取座位id
            int iemployeeid = _seatTable[0].EmployeeID;
            int employeeType = _seatTable[0].EmployeeType;
            string employeeid = iemployeeid.ToString();

            string employeeOthersKey = employeeid.ToString() + "|" + employeeType.ToString();

            switch (employeeType)
            {
                case 0:
                    return LoginReturnStatus.NOUSER;
                case 1:
                    Hr_InternTableAdapter _internAdapter = new Hr_InternTableAdapter();
                    DataSet_HRSeat.Hr_InternDataTable _internTable = _internAdapter.GetInternByID(iemployeeid);
                    if (_internTable.Count == 0)
                    {
                        return LoginReturnStatus.NOUSER;
                    }
                    if (_internTable[0].FullName != name)
                    {
                        return LoginReturnStatus.NOUSER;
                    }
                    break;
                case 2:
                    Hr_Employee_TemporaryTableAdapter _empTmpAdapter = new Hr_Employee_TemporaryTableAdapter();
                    DataSet_HRSeat.Hr_Employee_TemporaryDataTable _empTmpTable = _empTmpAdapter.GetEmployeeTmpByID(iemployeeid);
                    if (_empTmpTable.Count == 0)
                    {
                        return LoginReturnStatus.NOUSER;
                    }
                    if (_empTmpTable[0].FullName != name)
                    {
                        return LoginReturnStatus.NOUSER;
                    }
                    break;
                default:
                    break;
            }
            //有此用户

            // 非正式员工判断在线状态逻辑


            UpdateTableAdapter _updateAdapter = new UpdateTableAdapter();
            Hr_EP_SeatTableAdapter _adapater = new Hr_EP_SeatTableAdapter();
            Hr_EmployeeTableAdapter _employeeAdapter = new Hr_EmployeeTableAdapter();
            Hr_Employee_TemporaryTableAdapter _employeeTmpAdapter = new Hr_Employee_TemporaryTableAdapter();
            //DataSet_HRSeat.Hr_EmployeeDataTable employeeTable = _employeeAdapter.GetEmployeeByID(id);
            //DataSet_HRSeat.Hr_Employee_TemporaryDataTable employeeTmpTable = _employeeTmpAdapter.GetEmployeeTmpByID(id);
            Hr_EP_Online_HistoryTableAdapter _historyAdapter = new Hr_EP_Online_HistoryTableAdapter();

            //如果账号已经登录 且不是本机登陆 则将登录的机器强制下线
            if (onlineClients.ContainsKey(employeeOthersKey) && onlineClients[employeeOthersKey] != ip)
            {
                if (!forceOfflineClients.Contains(onlineClients[employeeOthersKey]))
                {
                    forceOfflineClients.Add(onlineClients[employeeOthersKey]);
                }
                onlineClients[employeeOthersKey] = ip;
            }

            //将用户加入到在线列表
            if (!onlineClients.ContainsKey(employeeOthersKey))
            {
                onlineClients.Add(employeeOthersKey, ip);
            }

            //获取此用户的座位
            ////已经有人坐上去了
            //if (_seatTable[0].OnlineStatus != 0)
            //{
            //    //如果有别人在座 挤下来
            //    if (_seatTable[0].EmployeeID + "|" + _seatTable[0].EmployeeType != employeeOthersKey)
            //    {
            //        UserLogout(ip);
            //    }
            //    //return LoginReturnStatus.ALREADYOK;
            //    ///
            //}

            //没人坐上去
            //自己坐在自己的位置
            if (_seatTable[0].EmployeeID + "|" + employeeType == employeeOthersKey)
            {
                //状态改为自己坐在自己的位置
                _updateAdapter.UpdateOnlineStatusByIP(1, ip);
                DataSet_HRSeat.Hr_EP_Online_HistoryDataTable hisTable = _historyAdapter.GetDataByIP(ip);
                //添加登陆历史
                if (hisTable.Count == 0 || hisTable.Count != 0 && hisTable[0]["OfflineTime"] != DBNull.Value)
                {
                    _historyAdapter.Insert(_seatTable[0].ID, DateTime.Now, null, ip, int.Parse(employeeid), employeeType);
                }
                return LoginReturnStatus.OK;
            }
            //其他人坐在这个座位
            else
            {
                //他原来的位置
                DataSet_HRSeat.Hr_EP_SeatDataTable seatTableOthers = _adapater.GetSeatByIDType(iemployeeid, (short)employeeType);
                _updateAdapter.UpdateOnlineStatusByIP(2, ip);
                //这样id和ip不匹配 表示是其他人坐上去了 通过在历史表中查id 可以查到这个机器是被哪个“其他人”使用过
                if (seatTableOthers.Count != 0)
                {
                    DataSet_HRSeat.Hr_EP_Online_HistoryDataTable hisTable = _historyAdapter.GetDataByIP(ip);
                    if (hisTable.Count == 0 || hisTable.Count != 0 && hisTable[0]["OfflineTime"] != DBNull.Value)
                    {
                        _historyAdapter.Insert(seatTableOthers[0].ID, DateTime.Now, null, ip, int.Parse(employeeid), employeeType);
                    }
                    return LoginReturnStatus.WRONGSEAT;
                }
                _historyAdapter.Insert(null, DateTime.Now, null, ip, int.Parse(employeeid), employeeType);
                return LoginReturnStatus.NOUSERSEAT;
            }
        }

        public bool UserLogout(string ip)
        {
            if (logoutList.Contains(ip))
            {
                return false;
            }
            logoutList.Add(ip);
            //将ip从在线列表移除
            if (onlineClients.ContainsValue(ip))
            {
                foreach (KeyValuePair<string, string> client in onlineClients)
                {
                    if (client.Value == ip)
                    {
                        onlineClients.Remove(client.Key);
                        break;
                    }
                }
            }
            Hr_EP_SeatTableAdapter _adapater = new Hr_EP_SeatTableAdapter();
            UpdateTableAdapter _updateAdapter = new UpdateTableAdapter();
            Hr_EP_Online_HistoryTableAdapter _historyAdapter = new Hr_EP_Online_HistoryTableAdapter();
            DataSet_HRSeat.Hr_EP_SeatDataTable seatTable = null;

            seatTable = _adapater.GetSeatByIP(ip);
            //下线
            _updateAdapter.UpdateOnlineStatusByIP(0, ip);
            //更新历史记录
            DataSet_HRSeat.Hr_EP_Online_HistoryDataTable hisTable = _historyAdapter.GetDataByIP(ip);
            if (hisTable.Count != 0)
            {
                if (hisTable[0].OnLineTime > DateTime.Now)
                {
                    hisTable[0].OfflineTime = hisTable[0].OnLineTime + new TimeSpan(0, 0, 0, 0, 1);
                }
                else
                {
                    hisTable[0].OfflineTime = DateTime.Now;
                }
                _historyAdapter.Update(hisTable[0]);
            }
            logoutList.Remove(ip);
            return true;
        }
        public long GetUserIDByUserName(string username)
        {

            Hr_RBAC_UserTableAdapter _adapter = new Hr_RBAC_UserTableAdapter();
            DataSet_HRSeat.Hr_RBAC_UserDataTable _dt = _adapter.GetDataByUserName(username);
            if (_dt.Count == 1)
            {
                return _dt[0].ID;
            }
            else
            {
                return -1;
            }

        }
    }
}