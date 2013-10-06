using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using HRSeatServer.DataSet_HRSeatTableAdapters;

namespace HRSeatServer
{

    [DataContract]
    [Flags]
    public enum LoginReturnStatus
    {
        //管理员
        [EnumMember]
        ADMIN,
        //没有分配座位
        [EnumMember]
        NOUSERSEAT,
        //坐的是自己的位置
        [EnumMember]
        OK,
        //密码错误
        [EnumMember]
        PWDERROR,
        //已经有人坐了
        [EnumMember]
        ALREADYOK,
        //用户不存在
        [EnumMember]
        NOUSER,
        //坐的不是自己的位置
        [EnumMember]
        WRONGSEAT
    }
    [ServiceKnownType(typeof(LoginReturnStatus))]

    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IHRSeat
    {
        [OperationContract]
        int CheckOnline(string ip, int onlinestatus);

        [OperationContract]
        bool DeleteFloor(int floorid);

        [OperationContract]
        bool DeleteRoomsAndSeats(int floorid);

        [OperationContract]
        bool DeleteSeatByRoomID(int roomid);

        [OperationContract]
        bool DeleteSeatBySeatID(int seatid);

        [OperationContract]
        bool DeleteSeatsByFloorID(int floorid);

        [OperationContract]
        DataSet_HRSeat.ContactsEXPDataTable ExportContracts();

        [OperationContract]
        DataSet_HRSeat.ContactsEXPByNameDataTable ExportContractsByName();

        [OperationContract]
        DataSet_HRSeat.Hr_Employment_DivisionDataTable GetAllDivisions();

        [OperationContract]
        DataSet_HRSeat.Hr_EP_FloorDataTable GetAllFloors();

        [OperationContract]
        DataSet_HRSeat.Hr_EP_FloorNoImgDataTable GetAllFloorsNoImg();

        [OperationContract]
        DataSet_HRSeat.Hr_EP_SeatDataTable GetAllSeats();


        [OperationContract]
        DataSet_HRSeat.Hr_Employment_DivisionDataTable GetDivisionByEmployeeID(int employeeid);

        [OperationContract]
        DataSet_HRSeat.DivisionEmployeeDataTable GetDivisionEmployee();

        [OperationContract]
        DataSet_HRSeat.DivisionEmployeeDataTable GetDivisionEmployeeNoSeat();

        [OperationContract]
        DataSet_HRSeat.Hr_EmployeeDataTable GetEmployeeByFloorID(int floorid);

        [OperationContract]
        DataSet_HRSeat.Hr_EmployeeDataTable GetEmployeeByID(int id);

        [OperationContract]
        DataSet_HRSeat.Hr_EmployeeDataTable GetEmployeeByNoSeat();

        [OperationContract]
        int GetEmployeeIDByUserName(string username);

        [OperationContract]
        string GetEmployeeNameByID(int employeeid);

        [OperationContract]
        string GetEmployeeNameByIDEmpType(int employeeid, int employeetype);

        [OperationContract]
        string GetEmployeeNameBySeatID(int seatid);

        [OperationContract]
        DataSet_HRSeat.EmployeeStatusDataTable GetEmployeeStatus(int employeeid, int employeetype, DateTime begin, DateTime end, ref DateTime serverTime);

        [OperationContract]
        DataSet_HRSeat.Hr_Employee_TemporaryDataTable GetEmployeeTmpByFloorID(int floorid);

        [OperationContract]
        DataSet_HRSeat.Hr_Employee_TemporaryDataTable GetEmployeeTmpByID(int id);

        [OperationContract]
        DataSet_HRSeat.Hr_Employee_TemporaryDataTable GetEmployeeTmp();

        [OperationContract]
        DataSet_HRSeat.Hr_Employee_TemporaryDataTable GetEmployeeTmpByNoSeat();

        [OperationContract]
        long GetEmployeeTmpNewID();

        [OperationContract]
        DataSet_HRSeat.Hr_EP_FloorDataTable GetFloorByID(int id);

        [OperationContract]
        DataSet_HRSeat.Hr_InternDataTable GetInternByFloorID(int floorid);

        [OperationContract]
        DataSet_HRSeat.Hr_InternDataTable GetInternByID(int id);

        [OperationContract]
        DataSet_HRSeat.Hr_InternDataTable GetIntern();

        [OperationContract]
        DataSet_HRSeat.Hr_InternDataTable GetInternByNoSeat();

        [OperationContract]
        DataSet_HRSeat.Hr_EP_RoomDataTable GetRoomByFloorID(int floorid);

        [OperationContract]
        DataSet_HRSeat.Hr_EP_RoomDataTable GetRoomByID(int id);

        [OperationContract]
        DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatAllHistory(string ipAddress);

        [OperationContract]
        DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatHistoryByIP(string ipAddress, ref DateTime serverTime);

        [OperationContract]
        DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatHistoryByIPDay(string ipAddress, uint day, ref DateTime serverTime);

        [OperationContract]
        DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatHistoryByIDType(int employeeid, int employeetype);

        [OperationContract]
        DataSet_HRSeat.Hr_EP_Online_HistoryDataTable GetSeatHistoryByIPType(string ipAddress, int employeetype);

        [OperationContract]
        DataSet_HRSeat.Hr_EP_SeatDataTable GetSeatsByFloorID(int floorid);

        [OperationContract]
        DataSet_HRSeat.Hr_EP_SeatDataTable GetSeatByIP(string IP);

        [OperationContract]
        DataSet_HRSeat.Hr_EP_SeatDataTable GetSeatsByFloorIDRefresh(int floorid);
        //更新员工资料
        [OperationContract]
        bool UpdateEmployee(DataSet_HRSeat.Hr_EmployeeDataTable employeeTable);

        [OperationContract]
        bool UpdateEmployeeTmp(DataSet_HRSeat.Hr_Employee_TemporaryDataTable employeeTmpTable);

        [OperationContract]
        bool UpdateFloor(DataSet_HRSeat.Hr_EP_FloorDataTable floorTable);

        [OperationContract]
        bool UpdateIntern(DataSet_HRSeat.Hr_InternDataTable internTable);

        [OperationContract]
        void UpdateOnlineHisAllOffline();

        [OperationContract]
        void UpdateOnlineStatusAllOffline();

        [OperationContract]
        bool UpdateRooms(DataSet_HRSeat.Hr_EP_RoomDataTable roomTable);

        [OperationContract]
        bool UpdateSeats(DataSet_HRSeat.Hr_EP_SeatDataTable seatTable);

        [OperationContract]
        LoginReturnStatus UserLogin(string employeeid, string password, string ip, out bool isadmin);

        [OperationContract]
        LoginReturnStatus UserLoginAnonymous(string ip, string name);

        [OperationContract]
        bool UserLogout(string ip);

        [OperationContract]
        long GetUserIDByUserName(string username);
    }

}
