using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.ServiceModel;
using HRSeatServer;

namespace HRSeatHost
{
    public partial class HRSeatHost : Form
    {

        private bool bIsShow = false;
        private byte[] buffer = new byte[1024];
        private System.Timers.Timer checkTimer = new System.Timers.Timer();
        private Dictionary<string, DateTime> clientDic = new Dictionary<string, DateTime>();
        private EndPoint ep;
        private int heartbeatPort;
        private DateTime lastSeatUpdate;
        private DateTime lastUpdate;
        private HRSeatService service = null;
        private Socket udpServerSocket;
        public HRSeatHost()
        {
            InitializeComponent();
        }

        private void checkTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            clientDicUpdate();
            //客户端字典
            Dictionary<string, DateTime> clientDicTmp = new Dictionary<string, DateTime>();
            foreach (KeyValuePair<string, DateTime> item in clientDic)
            {
                clientDicTmp.Add(item.Key, item.Value);
            }
            //遍历客户端
            foreach (KeyValuePair<string, DateTime> item in clientDicTmp)
            {

                TimeSpan timeSinceLastHeartbeat = DateTime.Now.ToUniversalTime() - item.Value;

                // 比对时间戳
                if (timeSinceLastHeartbeat > TimeSpan.FromSeconds(10))
                {
                    // 时间戳过了 下线
                    try
                    {
                        clientDic.Remove(item.Key);
                        service.UserLogout(item.Key);
                    }
                    catch
                    {
                        if (!bIsShow)
                        {
                            bIsShow = true;
                            MessageBox.Show("EPS服务器：数据库访问失败！");
                        }
                        Environment.Exit(0);
                    }
                }
            }
        }

        private void clientDicUpdate()
        {
            if (DateTime.Now.ToUniversalTime() - lastSeatUpdate > TimeSpan.FromSeconds(10))
            {
                HRSeatServer.DataSet_HRSeatTableAdapters.Hr_EP_SeatTableAdapter _adapter = new HRSeatServer.DataSet_HRSeatTableAdapters.Hr_EP_SeatTableAdapter();
                DataSet_HRSeat.Hr_EP_SeatDataTable seatTable = null;
                try
                {
                    seatTable = _adapter.GetSeatsByOnline();
                }
                catch
                {
                    if (!bIsShow)
                    {
                        bIsShow = true;
                        MessageBox.Show("EPS服务器：数据库访问失败！");
                    }
                    Environment.Exit(0);
                }
                if (seatTable.Count == 0)
                {
                    return;
                }
                foreach (DataSet_HRSeat.Hr_EP_SeatRow row in seatTable)
                {
                    //如果有新的座位 则添加到clientDic中
                    if (!clientDic.ContainsKey(row.IP))
                    {
                        clientDic.Add(row.IP, DateTime.Now.ToUniversalTime());
                    }
                }
                lastSeatUpdate = DateTime.Now.ToUniversalTime();
            }
        }
        private void Exit()
        {
            service.UpdateOnlineStatusAllOffline();
            service.UpdateOnlineHisAllOffline();
            if (notifyIconServer != null)
            {
                notifyIconServer.Dispose();
            }
            Environment.Exit(0);
        }


        private void HRSeatHost_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            //init
            try
            {
                heartbeatPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ServerHeartBeatPort"].ToString());
            }
            catch
            {
                MessageBox.Show("配置文件出错！");
                Environment.Exit(0);
            }
            ep = new IPEndPoint(IPAddress.Any, heartbeatPort);
            lastSeatUpdate = DateTime.Now.ToUniversalTime();
            clientDicUpdate();
            udpServerSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            udpServerSocket.Bind(ep);
            udpServerSocket.BeginReceiveFrom(buffer, 0, 1024, SocketFlags.None, ref ep, new AsyncCallback(ReceiveData), udpServerSocket);

            checkTimer.Interval = 1000;
            checkTimer.AutoReset = true;
            checkTimer.Elapsed += new System.Timers.ElapsedEventHandler(checkTimer_Elapsed);
            checkTimer.Start();
            //init

            ServiceHost hostHRSeatService = new ServiceHost(typeof(HRSeatService));
            ServiceHost hostSchedule = new ServiceHost(typeof(Schedule));
            service = new HRSeatService();
            //seat init
            try
            {
                service.UpdateOnlineStatusAllOffline();
                service.UpdateOnlineHisAllOffline();
            }
            catch
            {
                if (!bIsShow)
                {
                    bIsShow = true;
                    MessageBox.Show("EPS服务器：数据库访问失败！");
                }
                Environment.Exit(0);
            }

            hostHRSeatService.Open();
            hostSchedule.Open();
            notifyIconServer.ShowBalloonTip(5000);
            //lblStatus.Text = "服务已经启动";
            //Console.WriteLine("服务已经启动");
            //Console.Read();
        }
        void ReceiveData(IAsyncResult iar)
        {
            // Create temporary remote end Point
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint tempRemoteEP = (EndPoint)sender;

            // 获取socket连接
            Socket remote = (Socket)iar.AsyncState;

            // 接受消息
            int recv = remote.EndReceiveFrom(iar, ref tempRemoteEP);

            // 接受buffer字符串
            string stringData = Encoding.ASCII.GetString(buffer, 0, recv);
            //Console.WriteLine(stringData);
            //更新时间戳
            lastUpdate = DateTime.Now.ToUniversalTime();
            if (clientDic.ContainsKey(stringData))
            {
                clientDic[stringData] = lastUpdate;
            }
            else
            {
                clientDic.Add(stringData, lastUpdate);
            }


            //重新接收
            if (!this.IsDisposed)
            {
                udpServerSocket.BeginReceiveFrom(buffer, 0, 1024, SocketFlags.None, ref ep, new AsyncCallback(ReceiveData), udpServerSocket);
            }
        }
        private void tSMIAbout_Click(object sender, EventArgs e)
        {
            (new HRSeat.frmAbout()).ShowDialog();
        }

        private void tSMIExit_Click(object sender, EventArgs e)
        {
            Exit();
        }
    }

}
