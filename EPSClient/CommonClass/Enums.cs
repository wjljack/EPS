using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRSeat.CommonClass
{
    public class Enums
    {
        public enum EmployeeType
        {
            Employee = 0,
            Intern = 1,
            Employee_temporary = 2,
        }

        public enum SelectType
        {
            AllEmployee,
            NoSeatEmployee
        }
        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }
        public enum OnlineType
        {
            Online = 1,
            Online2 = 2,
            Offline = 0,
            ForceOffline = 3
        }
        public enum ExpType
        {
            Name = 1,
            Dept = 2
        }
    }
}
