using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LY.Communication
{
    /// <summary>
    /// 串口服务器
    /// </summary>
    public partial class SerialPortService
    {
        SerialPort _SerialPort = null;
        bool IsStart = false;
        bool IsZf = false;
        Thread thThreadRead;
        String RecvDataText = null;
        private static byte[] result = new byte[1024];
        static Socket serverSocket;
        Dictionary<string, List<Thread>> _ThreadPool = new Dictionary<string, List<Thread>>();
        string cmbCom;
        string cmbBound;
        string cmbDataBit;
        string cmbStop;
        string cmbEven;
        string txtIP;
        string txtPort;

        #region 通讯
        public void Start()
        {
            LoadConfig();
            StartMethod();
        }

        public void Stop()
        {
            try
            {
                _SerialPort.Close();
                thThreadRead.Abort();
            }
            catch
            {

            }

        }
        #endregion

        #region 事件
        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (_SerialPort.BytesToRead > 0)
            {
                RecvDataText = "";

                byte[] packet = new byte[_SerialPort.BytesToRead];
                _SerialPort.Read(packet, 0, packet.Length);

                for (int i = 0; i <= packet.Length - 1; i++)
                {
                    RecvDataText += packet[i].ToString("X2");
                }

                IsZf = true;
            }
        }
        #endregion

        #region 方法
        private void LoadConfig()
        {
            cmbCom = ConfigurationManager.AppSettings["Port"];
            cmbBound = ConfigurationManager.AppSettings["Bound"];
            cmbDataBit = ConfigurationManager.AppSettings["DataBits"];
            cmbStop = ConfigurationManager.AppSettings["StopBits"];
            cmbEven = ConfigurationManager.AppSettings["Odd"];
            txtIP = ConfigurationManager.AppSettings["IP"];
            txtPort = ConfigurationManager.AppSettings["IPPort"];
        }

        private void InitSerialPort()
        {
            _SerialPort = new SerialPort();

            _SerialPort.DataReceived += serialPort_DataReceived;
            _SerialPort.ReadTimeout = 200;
            _SerialPort.WriteTimeout = 300;
            _SerialPort.BaudRate = Convert.ToInt32(cmbBound);

            _SerialPort.DataBits = Convert.ToInt32(cmbDataBit);
            _SerialPort.PortName = cmbCom;
            switch (cmbStop)
            {
                case "None": _SerialPort.StopBits = System.IO.Ports.StopBits.None;
                    break;
                case "One": _SerialPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case "OnePointFive": _SerialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
                case "Two": _SerialPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default: _SerialPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
            }

            switch (cmbEven)
            {
                case "Even": _SerialPort.Parity = System.IO.Ports.Parity.Even;
                    break;
                case "Mark": _SerialPort.Parity = System.IO.Ports.Parity.Mark;
                    break;
                case "None": _SerialPort.Parity = System.IO.Ports.Parity.None;
                    break;
                case "Odd": _SerialPort.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case "Space": _SerialPort.Parity = System.IO.Ports.Parity.Space;
                    break;
                default: _SerialPort.Parity = System.IO.Ports.Parity.None;
                    break;

            }

        }
     
        private void StartMethod()
        {
            if (!IsStart)
            {
                // SaveConfig();
                InitSerialPort();
                _SerialPort.Open();
                thThreadRead = new Thread(new ThreadStart(Listen));
                thThreadRead.Start();
                IsStart = true;
                //btnStart.Text = "停止";


            }
            else
            {
                _SerialPort.Close();
                thThreadRead.Abort();
                IsStart = false;
                //btnStart.Text = "开始";
            }
        }

        private byte[] GetByte(string cmd)
        {
            byte[] result = new byte[cmd.Length / 2];

            try
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = Convert.ToByte(cmd.Substring(i * 2, 2), 16);
                }
            }
            catch
            {
            }


            return result;
        }

        private void Listen()
        {
            IPAddress ip = IPAddress.Parse(txtIP);//txtIP.Text
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, Convert.ToInt32(txtPort)));  //绑定IP地址：端口  
            serverSocket.Listen(0);

            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
        }

        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private void ListenClientConnect()
        {
            while (true)
            {
                try
                {

                    Socket clientSocket = serverSocket.Accept();

                    Thread receiveThread = new Thread(ReceiveMessage);
                    receiveThread.Start(clientSocket);

                    Thread sendThread = new Thread(SendMessage);
                    sendThread.Start(clientSocket);

                    string remote = (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                    if (_ThreadPool.Keys.Contains(remote))
                    {
                        _ThreadPool[remote].ForEach(p => p.Abort());
                        _ThreadPool.Remove(remote);
                    }

                    _ThreadPool.Add(remote, new List<Thread>());
                    _ThreadPool[remote].Add(receiveThread);
                    _ThreadPool[remote].Add(sendThread);
                }
                catch
                {
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                Thread.Sleep(1);
                try
                {
                    //通过clientSocket接收数据  
                    int receiveNumber = myClientSocket.Receive(result);
                    if (receiveNumber == 0)
                    {
                        return;
                    }
                    byte[] newB = new byte[receiveNumber];
                    for (int i = 0; i < receiveNumber; i++)
                    {
                        newB[i] = result[i];
                    }
                    _SerialPort.Write(newB, 0, receiveNumber);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    try
                    {
                        myClientSocket.Shutdown(SocketShutdown.Both);
                    }
                    catch
                    {
                    }
                    try
                    {
                        myClientSocket.Close();
                    }
                    catch
                    {
                    }
                    return;
                }
                //Application.DoEvents();

            }
        }

        private void SendMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    if (IsZf)
                    {
                        if (RecvDataText != "")
                        {
                            byte[] msg = GetByte(RecvDataText);
                            myClientSocket.Send(msg);
                            RecvDataText = "";
                        }
                        IsZf = false;
                    }
                }
                catch
                {

                    return;
                }
                //Application.DoEvents();
                Thread.Sleep(1);
            }
        }

          
        #endregion

    }
}
