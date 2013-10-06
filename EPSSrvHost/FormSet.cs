using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace HRSeat.CommonClass
{
    static class FormSet
    {
        /**/
        /// <summary>
        /// 页面居中
        /// </summary>
        public static void SetMid(Form form)
        {
            // Center the Form on the user's screen everytime it requires a Layout.
            form.SetBounds((Screen.GetBounds(form).Width / 2) - (form.Width / 2),
                (Screen.GetBounds(form).Height / 2) - (form.Height / 2),
                form.Width, form.Height, BoundsSpecified.Location);
        }
        public const Int32 AW_HOR_POSITIVE = 0x00000001; // 从左到右打开窗口
        public const Int32 AW_HOR_NEGATIVE = 0x00000002; // 从右到左打开窗口
        public const Int32 AW_VER_POSITIVE = 0x00000004; // 从上到下打开窗口
        public const Int32 AW_VER_NEGATIVE = 0x00000008; // 从下到上打开窗口
        public const Int32 AW_CENTER = 0x00000010; //若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展。
        public const Int32 AW_HIDE = 0x00010000; //隐藏窗口，缺省则显示窗口。
        public const Int32 AW_ACTIVATE = 0x00020000; //激活窗口。在使用了AW_HIDE标志后不要使用这个标志。
        public const Int32 AW_SLIDE = 0x00040000; //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略。
        public const Int32 AW_BLEND = 0x00080000; //使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志。
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AnimateWindow(
          IntPtr hwnd, // handle to window
          int dwTime, // duration of animation
          int dwFlags // animation type
          );

        public static void Paint(this object sender, PaintEventArgs e)
        {
            Form form = ((Form)sender);
            List<Point> list = new List<Point>();
            int width = form.Width;
            int height = form.Height;

            //左上
            list.Add(new Point(0, 5));
            list.Add(new Point(1, 5));
            list.Add(new Point(1, 3));
            list.Add(new Point(2, 3));
            list.Add(new Point(2, 2));
            list.Add(new Point(3, 2));
            list.Add(new Point(3, 1));
            list.Add(new Point(5, 1));
            list.Add(new Point(5, 0));
            //右上
            list.Add(new Point(width - 5, 0));
            list.Add(new Point(width - 5, 1));
            list.Add(new Point(width - 3, 1));
            list.Add(new Point(width - 3, 2));
            list.Add(new Point(width - 2, 2));
            list.Add(new Point(width - 2, 3));
            list.Add(new Point(width - 1, 3));
            list.Add(new Point(width - 1, 5));
            list.Add(new Point(width - 0, 5));
            //右下
            list.Add(new Point(width - 0, height - 5));
            list.Add(new Point(width - 1, height - 5));
            list.Add(new Point(width - 1, height - 3));
            list.Add(new Point(width - 2, height - 3));
            list.Add(new Point(width - 2, height - 2));
            list.Add(new Point(width - 3, height - 2));
            list.Add(new Point(width - 3, height - 1));
            list.Add(new Point(width - 5, height - 1));
            list.Add(new Point(width - 5, height - 0));
            //左下
            list.Add(new Point(5, height - 0));
            list.Add(new Point(5, height - 1));
            list.Add(new Point(3, height - 1));
            list.Add(new Point(3, height - 2));
            list.Add(new Point(2, height - 2));
            list.Add(new Point(2, height - 3));
            list.Add(new Point(1, height - 3));
            list.Add(new Point(1, height - 5));
            list.Add(new Point(0, height - 5));

            Point[] points = list.ToArray();

            GraphicsPath shape = new GraphicsPath();
            shape.AddPolygon(points);

            //将窗体的显示区域设为GraphicsPath的实例 
            form.Region = new System.Drawing.Region(shape);
            //Color TColor = Color.FromArgb(200, 200, 200);
            //Color FColor = Color.FromArgb(255, 255, 255);
            //Brush b = new LinearGradientBrush((sender as Form).ClientRectangle, FColor, TColor, LinearGradientMode.ForwardDiagonal);
            Graphics g = e.Graphics;
            //g.FillRectangle(b, (sender as Form).ClientRectangle);
            //g.DrawLine(pen, new Point(10, 10), new Point(100, 100));
            Pen pen = new Pen(Color.FromArgb(200, 200, 200));
            g.DrawPolygon(pen, points);
        }

        #region 窗体边框阴影效果变量申明
        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        #endregion
    }
}
