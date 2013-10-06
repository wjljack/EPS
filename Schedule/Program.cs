using System;
using System.Windows.Forms;

namespace MySchedule
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            bool ret;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
            if (ret)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain(""));
                mutex.ReleaseMutex();
            }
            else
            {
                Application.Exit();//退出程序
            }
        }
    }
}