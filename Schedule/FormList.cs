using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySchedule.ScheduleServiceReference;

namespace MySchedule
{
    public partial class FormList : Form
    {
        public FormList(DataView dv, DateTime date)
        {
            InitializeComponent();
            dgvTask.DataSource = dv;
            this.Text = date.ToString("yyyy年MM月dd日") + " " + GetDayName(date);
        }
        private string GetDayName(DateTime date)
        {
            string result = "";
            if (date.DayOfWeek == DayOfWeek.Monday)
            {
                result = "星期一";
            }
            else if (date.DayOfWeek == DayOfWeek.Tuesday)
            {
                result = "星期二";
            }
            else if (date.DayOfWeek == DayOfWeek.Wednesday)
            {
                result = "星期三";
            }
            else if (date.DayOfWeek == DayOfWeek.Thursday)
            {
                result = "星期四";
            }
            else if (date.DayOfWeek == DayOfWeek.Friday)
            {
                result = "星期五";
            }
            else if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                result = "星期六";
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                result = "星期日";
            }
            return result;
        }
        private void dgvTask_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //获得选中行的数据
            DataGridViewRow row = dgvTask.Rows[e.RowIndex];
            string message = row.Cells["Task"].Value.ToString();
            string memo = row.Cells["Memo"].Value.ToString();
            string startTime = Convert.ToDateTime(row.Cells["StartTime"].Value).ToString("HH:mm");
            string endTime = Convert.ToDateTime(row.Cells["EndTime"].Value).ToString("HH:mm");
            string date = Convert.ToDateTime(row.Cells["StartTime"].Value).ToString("yyyy-MM-dd");
            FormDetail f = new FormDetail(date, startTime, endTime, message, memo);
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        private void dgvTask_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridViewTextBoxColumn dgv_Text = new DataGridViewTextBoxColumn();
            for (int i = 0; i < dgvTask.Rows.Count; i++)
            {
                int j = i + 1;
                dgvTask.Rows[i].HeaderCell.Value = j.ToString();
            }
        }

    }
}
