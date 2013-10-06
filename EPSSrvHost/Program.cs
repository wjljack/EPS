using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using log4net;
using System.Reflection;
using HRSeatServer;
using System.Diagnostics;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace HRSeatHost
{
    static class Program
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool ret;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
            if (ret)
            {
                log.Info("----------------------Begin-------------------------");
                try
                {   //处理未捕获的异常
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    //处理UI线程异常
                    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                    //处理非UI线程异常
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new HRSeatHost());
                    mutex.ReleaseMutex();
                }
                catch (Exception ex)
                {
                    ExceptionExit();
                    string str = "";
                    string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";

                    if (ex != null)
                    {
                        str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                        ex.GetType().Name, ex.Message, ex.StackTrace);
                    }
                    else
                    {
                        str = string.Format("应用程序线程错误:{0}", ex);
                    }

                    MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //LogManager.WriteLog(str);
                    log.Fatal(str);
                    log.Info("----------------------End-------------------------");
                    try
                    {
                        //run the program again and close this one
                        Process.Start(Application.StartupPath + "\\EPSServer.exe");
                        //or you can use Application.ExecutablePath

                        //close this one
                        Process.GetCurrentProcess().Kill();
                    }
                    catch
                    { }
                }
            }
            else
            {
                Environment.Exit(0);//退出程序
            }
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = "";
            string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
            Exception error = e.Exception as Exception;
            if (error != null)
            {
                str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                error.GetType().Name, error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("应用程序线程错误:{0}", e);
            }

            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //LogManager.WriteLog(str);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = "";
            Exception error = e.ExceptionObject as Exception;
            string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
            if (error != null)
            {
                str = string.Format(strDateInfo + "Application UnhandledException:{0};\n\r堆栈信息:{1}", error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("Application UnhandledError:{0}", e);
            }

            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //LogManager.WriteLog(str);
        }
        static void ExceptionExit()
        {
            try
            {
                HRSeatService service = new HRSeatService(); ;
                service.UpdateOnlineStatusAllOffline();
                service.UpdateOnlineHisAllOffline();
            }
            catch
            {

            }
        }
    }
}
