using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Pabo.Calendar;
using MySchedule.ScheduleServiceReference;

namespace MySchedule
{
    public partial class FormMain : Form
    {
        private int userID = 0;
        ScheduleClient client = null;
        DataSet_Schedule.CalendarTaskDataTable _monthCalender = null;
        public FormMain(string username)
        {

            InitializeComponent();
            CommonClass.FormSet.SetMid(this);
            Control.CheckForIllegalCrossThreadCalls = false;
            client = new ScheduleClient();
            InitializeCalendarComponent();
            if (username != string.Empty)
            {
                try
                {
                    this.userID = (int)client.GetUserIDByUserName(username);
                }
                catch
                {
                    this.Close();

                }
            }
            Refresh();
            //System.Timers.Timer t = new System.Timers.Timer(10000);//实例化Timer类，设置间隔时间为10000毫秒；
            //t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；
            //t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            //t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        /// <summary>
        /// 定时刷新
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            LoadScheduleForWeekCalendar();
            LoadScheduleForMonthCalendar();
        }
        private void Refresh()
        {
            Splasher.Show(typeof(frmLoading));
            LoadScheduleForWeekCalendar();
            LoadScheduleForMonthCalendar();
            Splasher.Close(this);
        }


        /// <summary>
        /// 加载本周的日程数据
        /// </summary>
        private DataSet_Schedule.CalendarTaskDataTable SplitRowByDateRange(DataSet_Schedule.CalendarTaskDataTable table)
        {
            DataSet_Schedule.CalendarTaskDataTable newTable = table.Clone() as DataSet_Schedule.CalendarTaskDataTable;
            newTable.AcceptChanges();
            foreach (DataSet_Schedule.CalendarTaskRow row in table)
            {
                if (row.StartTime.ToString("yyyy-MM-dd") != row.EndTime.ToString("yyyy-MM-dd"))
                {
                    DataSet_Schedule.CalendarTaskRow oldRow = row;
                    int days = (row.EndTime - row.StartTime).Days;
                    for (int i = 0; i <= days; i++)
                    {
                        DataSet_Schedule.CalendarTaskRow newRow = newTable.NewRow() as DataSet_Schedule.CalendarTaskRow;
                        newRow.UserID = row.UserID;
                        newRow.Task = row.Task;
                        if (i == 0)
                        {
                            newRow.StartTime = row.StartTime;
                        }
                        else
                        {
                            newRow.StartTime = row.StartTime.AddDays(i).Date;
                        }
                        if (i == days)
                        {
                            newRow.EndTime = row.EndTime;
                        }
                        else
                        {
                            newRow.EndTime = row.StartTime.AddDays(i + 1).Date.AddSeconds(-1);
                        }
                        newRow.LastModifyTime = row.LastModifyTime;
                        newRow.Memo = row.Memo;


                        newTable.Rows.Add(newRow);
                    }
                }
                else
                {
                    DataSet_Schedule.CalendarTaskRow newRow = newTable.NewRow() as DataSet_Schedule.CalendarTaskRow;
                    newRow.UserID = row.UserID;
                    newRow.Task = row.Task;
                    newRow.StartTime = row.StartTime;
                    newRow.EndTime = row.EndTime;
                    newRow.LastModifyTime = row.LastModifyTime;
                    newRow.Memo = row.Memo;
                    newTable.Rows.Add(newRow);
                }
            }
            newTable.AcceptChanges();
            return newTable;
        }
        private void LoadScheduleForWeekCalendar()
        {
            if (userID == 0)
            {
                return;
            }
            DateTime today = DateTime.Now;  //当前时间
            DateTime startWeek = today.AddDays(1 - Convert.ToInt32(today.DayOfWeek.ToString("d")));//本周周一的日期                      
            DataSet_Schedule.CalendarTaskDataTable allScheduleData = client.GetMyTaskByRange(userID, startWeek.ToString("yyyy-MM-dd"), startWeek.AddDays(6).ToString("yyyy-MM-dd"));
            //DataSet_Schedule.CalendarTaskDataTable allScheduleData = client.GetMyTaskByRange(userID, "2010-01-01", "2014-01-01");
            allScheduleData = SplitRowByDateRange(allScheduleData);

            TimeSpan tsMorning = new TimeSpan(12, 0, 0);
            //周一
            dgvMonAm.DataSource = (from d in allScheduleData where d.StartTime.ToString("yyyy-MM-dd") == startWeek.ToString("yyyy-MM-dd") where d.StartTime.TimeOfDay <= tsMorning select d).AsDataView();
            dgvMonPm.DataSource = (from d in allScheduleData where d.EndTime.ToString("yyyy-MM-dd") == startWeek.ToString("yyyy-MM-dd") where d.EndTime.TimeOfDay > tsMorning select d).AsDataView();
            lblMon.Text = "周一\r\n" + startWeek.ToString("MM月dd日");
            //周二
            dgvTueAm.DataSource = (from d in allScheduleData where d.StartTime.ToString("yyyy-MM-dd") == startWeek.AddDays(1).ToString("yyyy-MM-dd") where d.StartTime.TimeOfDay <= tsMorning select d).AsDataView();
            dgvTuePm.DataSource = (from d in allScheduleData where d.EndTime.ToString("yyyy-MM-dd") == startWeek.AddDays(1).ToString("yyyy-MM-dd") where d.EndTime.TimeOfDay > tsMorning select d).AsDataView();
            lblTue.Text = "周二\r\n" + startWeek.AddDays(1).ToString("MM月dd日");
            //周三
            dgvWedAm.DataSource = (from d in allScheduleData where d.StartTime.ToString("yyyy-MM-dd") == startWeek.AddDays(2).ToString("yyyy-MM-dd") where d.StartTime.TimeOfDay <= tsMorning select d).AsDataView();
            dgvWedPm.DataSource = (from d in allScheduleData where d.EndTime.ToString("yyyy-MM-dd") == startWeek.AddDays(2).ToString("yyyy-MM-dd") where d.EndTime.TimeOfDay > tsMorning select d).AsDataView();
            lblWed.Text = "周三\r\n" + startWeek.AddDays(2).ToString("MM月dd日");
            //周四
            dgvThuAm.DataSource = (from d in allScheduleData where d.StartTime.ToString("yyyy-MM-dd") == startWeek.AddDays(3).ToString("yyyy-MM-dd") where d.StartTime.TimeOfDay <= tsMorning select d).AsDataView();
            dgvThuPm.DataSource = (from d in allScheduleData where d.EndTime.ToString("yyyy-MM-dd") == startWeek.AddDays(3).ToString("yyyy-MM-dd") where d.EndTime.TimeOfDay > tsMorning select d).AsDataView();
            lblThu.Text = "周四\r\n" + startWeek.AddDays(3).ToString("MM月dd日");
            //周五
            dgvFriAm.DataSource = (from d in allScheduleData where d.StartTime.ToString("yyyy-MM-dd") == startWeek.AddDays(4).ToString("yyyy-MM-dd") where d.StartTime.TimeOfDay <= tsMorning select d).AsDataView();
            dgvFriPm.DataSource = (from d in allScheduleData where d.EndTime.ToString("yyyy-MM-dd") == startWeek.AddDays(4).ToString("yyyy-MM-dd") where d.EndTime.TimeOfDay > tsMorning select d).AsDataView();
            lblFri.Text = "周五\r\n" + startWeek.AddDays(4).ToString("MM月dd日");
            //周六
            dgvSatAm.DataSource = (from d in allScheduleData where d.StartTime.ToString("yyyy-MM-dd") == startWeek.AddDays(5).ToString("yyyy-MM-dd") where d.StartTime.TimeOfDay <= tsMorning select d).AsDataView();
            dgvSatPm.DataSource = (from d in allScheduleData where d.EndTime.ToString("yyyy-MM-dd") == startWeek.AddDays(5).ToString("yyyy-MM-dd") where d.EndTime.TimeOfDay > tsMorning select d).AsDataView();
            lblSat.Text = "周六\r\n" + startWeek.AddDays(5).ToString("MM月dd日");
            //周日
            dgvSunAm.DataSource = (from d in allScheduleData where d.StartTime.ToString("yyyy-MM-dd") == startWeek.AddDays(6).ToString("yyyy-MM-dd") where d.StartTime.TimeOfDay <= tsMorning select d).AsDataView();
            dgvSunPm.DataSource = (from d in allScheduleData where d.EndTime.ToString("yyyy-MM-dd") == startWeek.AddDays(6).ToString("yyyy-MM-dd") where d.EndTime.TimeOfDay > tsMorning select d).AsDataView();
            lblSun.Text = "周日\r\n" + startWeek.AddDays(6).ToString("MM月dd日");
        }
        /// <summary>
        /// 相应月日程中的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mSchedule_DayDoubleClick(object sender, Pabo.Calendar.DayClickEventArgs e)
        {
            if (userID == 0)
            {
                return;
            }
            string strDate = Convert.ToDateTime(e.Date).ToString("yyyy-MM-dd");
            string strWeek = Convert.ToDateTime(e.Date).DayOfWeek.ToString();
            //ScheduleClient client = new ScheduleClient();

            var dayTask = from m in _monthCalender where m.StartTime.ToString("yyyy-MM-dd") == strDate select m;
            DataView dv;
            if (dayTask != null)
            {
                dv = dayTask.AsDataView();
            }
            else
            {
                MessageBox.Show("没有活动。");
                return;
            }
            FormList f = new FormList(dv, Convert.ToDateTime(e.Date));
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }
        #region//响应周日程中单元格双击事件
        private void ShowDetail(DataGridView grid, int SelectRowIndex)
        {
            DataGridViewRow row = grid.Rows[SelectRowIndex];
            string message = row.Cells[2].Value.ToString();
            string memo = row.Cells[6].Value.ToString();
            string startTime = Convert.ToDateTime(row.Cells[3].Value).ToString("HH:mm");
            string endTime = Convert.ToDateTime(row.Cells[4].Value).ToString("HH:mm");
            string date = Convert.ToDateTime(row.Cells[3].Value).ToString("yyyy-MM-dd");
            FormDetail f = new FormDetail(date, startTime, endTime, message, memo);
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }


        private void dgvMonAm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvTueAm, e.RowIndex);
        }

        private void dgvMonPm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvTuePm, e.RowIndex);
        }

        private void dgvTueAm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvWedAm, e.RowIndex);
        }

        private void dgvTuePm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvWedPm, e.RowIndex);
        }

        private void dgvWedAm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvThuAm, e.RowIndex);
        }

        private void dgvWedPm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvThuPm, e.RowIndex);
        }

        private void dgvThuAm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvFriAm, e.RowIndex);
        }

        private void dgvThuPm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvFriPm, e.RowIndex);
        }

        private void dgvFriAm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvSatAm, e.RowIndex);
        }

        private void dgvFriPm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvSatPm, e.RowIndex);
        }

        private void dgvSatAm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvSunAm, e.RowIndex);
        }

        private void dgvSatPm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvSunPm, e.RowIndex);
        }

        private void dgvSunAm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvMonAm, e.RowIndex);
        }

        private void dgvSunPm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetail(dgvMonPm, e.RowIndex);
        }
        #endregion
        #region//窗口可以拖动效果
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        private void monthCalender_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        #endregion

        /// <summary>
        /// 获得指定月的天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private int GetMonthDays(int year, int month)
        {
            int[][] LeapMonth = new int[][]
            {
                new int[]{31,28,31,30,31,30,31,31,30,31,30,31},
                new int[]{31,29,31,30,31,30,31,31,30,31,30,31}
            };  // 每月天数
            int[] monthdays = null;
            if (year % 400 == 0 ||
                (year % 4 == 0 && year % 100 != 0))
            {
                monthdays = LeapMonth[1];
            }
            else monthdays = LeapMonth[0];
            return monthdays[month - 1];
        }

        /// <summary>
        /// 加载本月的日程数据
        /// </summary>
        private void LoadScheduleForMonthCalendar()
        {
            mSchedule.Dates.Clear();
            int year = System.DateTime.Now.Year;
            int month = System.DateTime.Now.Month;
            int mdays = GetMonthDays(year, month);
            string beginDate = new System.DateTime(year, month, 1).ToString("yyyy-MM-dd");
            string endDate = new System.DateTime(year, month, mdays).ToString("yyyy-MM-dd");
            //DataSet_Schedule.CalendarTaskDataTable dt = client.GetMyTaskByRange(userID, beginDate, endDate);
            //_monthCalender = client.GetMyTaskByRange(userID, "2010-01-01", "2014-01-01");
            _monthCalender = client.GetMyTaskByRange(userID, beginDate, endDate);
            _monthCalender = SplitRowByDateRange(_monthCalender);
            for (int i = 1; i <= mdays; i++)
            {
                DateItem dItem = new DateItem();
                dItem.Date = new System.DateTime(year, month, i);
                // 读取日程
                DataView dv = (from day in _monthCalender where day.StartTime.ToString("yyyy-MM-dd") == dItem.Date.ToString("yyyy-MM-dd") select day).AsDataView();
                if (dv.Count != 0)
                {
                    dItem.Text = dv.Count.ToString();
                    dItem.TextColor = Color.Red;
                    dItem.BoldedDate = true;
                    mSchedule.AddDateInfo(dItem);
                }
            }
        }

        /// <summary>
        /// 初始化日历的样式
        /// </summary>
        private void InitializeCalendarComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
            //本月的日历样式
            mSchedule.ActiveMonth.Month = System.DateTime.Now.Month;
            mSchedule.ActiveMonth.Year = System.DateTime.Now.Year;
            mSchedule.BorderColor = System.Drawing.Color.Transparent;
            mSchedule.BorderStyle = System.Windows.Forms.ButtonBorderStyle.None;
            mSchedule.Footer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            mSchedule.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            mSchedule.Header.TextColor = System.Drawing.Color.White;
            mSchedule.MaxDate = new System.DateTime(2020, 2, 21, 19, 26, 22, 502);
            mSchedule.MinDate = new System.DateTime(2000, 2, 21, 19, 26, 22, 502);
            mSchedule.Month.BackgroundImage = null;
            mSchedule.Month.DateFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            mSchedule.Month.TextFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            mSchedule.SelectionMode = Pabo.Calendar.mcSelectionMode.One;
            mSchedule.SelectTrailingDates = false;
            mSchedule.ShowFooter = false;
            mSchedule.ShowHeader = true;
            mSchedule.Header.YearSelectors = false;
            mSchedule.Header.MonthSelectors = false;
            mSchedule.ShowToday = false;
            mSchedule.Weekdays.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(93)))), ((int)(((byte)(136)))));
            mSchedule.Weekdays.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            mSchedule.Weekdays.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            mSchedule.Weekdays.GradientMode = Pabo.Calendar.mcGradientMode.BackwardDiagonal;
            mSchedule.Weekdays.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            mSchedule.Weeknumbers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            //本周的日历样式
            Font fontStyle = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lblMon.Font = fontStyle;
            lblTue.Font = fontStyle;
            lblWed.Font = fontStyle;
            lblThu.Font = fontStyle;
            lblFri.Font = fontStyle;
            lblSat.Font = fontStyle;
            lblSun.Font = fontStyle;
            mSchedule.ResumeLayout(false);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            Refresh();
        }

    }
}
