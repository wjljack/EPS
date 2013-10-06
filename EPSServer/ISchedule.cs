using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HRSeatServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISchedule”。
    [ServiceContract]
    public interface ISchedule
    {
        [OperationContract]
        DataSet_Schedule.CalendarTaskDataTable GetMyMorningTask(int userID, string date);

        [OperationContract]
        DataSet_Schedule.CalendarTaskDataTable GetMyAfternoonTask(int userID, string date);

        [OperationContract]
        DataSet_Schedule.CalendarTaskDataTable GetMyTaskByDate(int userID, string date);

        [OperationContract]
        DataSet_Schedule.CalendarTaskDataTable GetMyTaskByRange(int userID, string beginDate, string endDate);

        [OperationContract]
        int gettest();
        [OperationContract]
        long GetUserIDByUserName(string username);
    }
}
