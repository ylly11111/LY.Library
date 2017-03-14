using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LY.Communication
{
   public class TcpServer
    {
        Thread thThreadRead; 
        static Socket serverSocket;
        Dictionary<string, List<Thread>> _ThreadPool = new Dictionary<string, List<Thread>>();
        private static byte[] result = new byte[1024];
        private string _IP;
        private string _Port;

        public TcpServer(string IP,string Port)
        {
            _IP = IP;
            _Port = Port;
        }

        public void Start()
        {
            thThreadRead = new Thread(new ThreadStart(Listen));
            thThreadRead.Start();
        }

        public void Stop()
        {
            thThreadRead.Abort();
        }

       private void Listen()
       {
           IPAddress ip = IPAddress.Parse(_IP);//txtIP.Text
           serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
           serverSocket.Bind(new IPEndPoint(ip, Convert.ToInt32(_Port)));  //绑定IP地址：端口  
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

                   string remote = (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                   if (_ThreadPool.Keys.Contains(remote))
                   {
                       _ThreadPool[remote].ForEach(p => p.Abort());
                       _ThreadPool.Remove(remote);
                   }

                   _ThreadPool.Add(remote, new List<Thread>());
                   _ThreadPool[remote].Add(receiveThread);
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
                   //
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


    }
}
