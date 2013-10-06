using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using HRSeatServer.DataSet_ScheduleTableAdapters;
using HRSeatServer.DataSet_HRSeatTableAdapters;

namespace HRSeatServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Schedule”。
    public class Schedule : ISchedule
    {
        public DataSet_Schedule.CalendarTaskDataTable GetMyMorningTask(int userID, string date)
        {
            CalendarTaskTableAdapter _adapter = new CalendarTaskTableAdapter();
            return _adapter.GetDataForMorning(date, userID);
        }

        public DataSet_Schedule.CalendarTaskDataTable GetMyAfternoonTask(int userID, string date)
        {
            CalendarTaskTableAdapter _adapter = new CalendarTaskTableAdapter();
            return _adapter.GetDataForAfternoon(date, userID);
        }

        public DataSet_Schedule.CalendarTaskDataTable GetMyTaskByDate(int userID, string date)
        {
            CalendarTaskTableAdapter _adapter = new CalendarTaskTableAdapter();
            return _adapter.GetDataByDate(date, userID);
        }
        public DataSet_Schedule.CalendarTaskDataTable GetMyTaskByRange(int userID, string beginDate, string endDate)
        {
            CalendarTaskTableAdapter _adapter = new CalendarTaskTableAdapter();
            long useridlong = userID;
            DateTime begin = DateTime.Parse(beginDate);
            DateTime end = DateTime.Parse(endDate);
            return _adapter.GetDataByRange(useridlong, begin, end);
        }
        public int gettest()
        {
            return 1;
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
