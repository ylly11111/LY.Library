using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LY.Communication
{
    public class TcpClientClass
    {
        #region 字段
        public TcpClient tcp;
        NetworkStream ns;
        Thread _trReceive;
        Thread _trSend;
        public delegate void AddTextHandle(string msg);
        public AddTextHandle AddText;
        #endregion

        #region 属性
        public string IPAddress1 { get; set; }

        public int Port { get; set; }
        #endregion


        #region 实现
        public TcpClientClass(string ip, int port)
        {
            IPAddress1 = ip;
            Port = port;
            _trReceive = new Thread(Receive);
            _trSend = new Thread(SendInThread);

        }

        public void Start()
        {
            byte[] bytes = new Byte[1024];
            tcp = new TcpClient();

            tcp.Connect(IPAddress1, Port);
            ns = tcp.GetStream();
            ns.ReadTimeout = 300;

            _trReceive.Start();
        }

        public void Stop()
        {
            try
            {
                ns.Close();
                tcp.Close();

                _trReceive.Abort();
            }
            catch
            {
            }
        }



        public void Send(byte[] b)
        {
            SendInThread(b);
        }

        private void SendInThread(object o)
        {

            try
            {
                byte[] b = (byte[])o;
                ns.Write(b, 0, b.Length);

            }
            catch
            {

                try
                {
                    ns.Close();
                }
                catch
                {
                }

                tcp.Close();

                tcp = new TcpClient();
                tcp.Connect(IPAddress1, Port);
                ns = tcp.GetStream();
            }
        }


        List<byte> bList = new List<byte>();
        private void Receive()
        {

            while (true)
            {
                Thread.Sleep(1);
                Application.DoEvents();
                byte[] bytes = new Byte[1024];
                string data = string.Empty;

                try
                {

                    int length = ns.Read(bytes, 0, bytes.Length);


                    if (length > 0)
                    {

                        byte[] result = new byte[length];
                        Array.Copy(bytes, 0, result, 0, result.Length);

                        string msg = Encoding.Default.GetString(result);

                        AddText(msg);
                    }
                }
                catch
                {
                }
            }
        }

        public void Send(string msg)
        {
            byte[] bs = Encoding.Default.GetBytes(msg);
            Send(bs);
        }
        #endregion
    }
}
