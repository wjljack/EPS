using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MySchedule
{
    public partial class FormDetail : Form
    {
        public FormDetail()
        {
            InitializeComponent();
        }

        public FormDetail(string date,string startDate,string endDate,string message ,string memo)
        {
            InitializeComponent();
            txtDate.Text = date;
            txtStartTime.Text = startDate;
            txtEndTime.Text = endDate;
            txtMessage.Text = message;
            txtMemo.Text = memo;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 窗口可以拖动效果
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        private void FormDetail_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        #endregion



    }
}
