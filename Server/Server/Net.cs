using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Net
    {
        public static void Main()
        {
            // 多线程
            Thread ListenThread = new Thread(new ThreadStart(ServerListener));
            ListenThread.Start();
            //============================================================================
            Socket ss = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipa = IPAddress.Parse("127.0.0.1");
            //IPEndPoint是用来把IP地址和端口号集成在一起的一个类型，在C#中叫做“网络端点”
            IPEndPoint iep = new IPEndPoint(ipa, 522);
            //之前创建的Socket与我们本机的IP和所设的端口号绑定
            ss.Bind(iep);
            //进行监听(挂起连接队列的最大长度)
            ss.Listen(50);
            byte[] bMessage = new byte[1024 * 10];
            string sMsg = "Can I help you ?";
            //当ss这个用于监听的socket收到一个连接请求之后，会接受对方请求，并建立一个新的连接
            //而新的这个s就是接下来用于真正通信的socket了。
            Socket s = ss.Accept();
            while (true)
            {
                try
                {
                    //bMessage = System.Text.Encoding.BigEndianUnicode.GetBytes(sMsg.ToCharArray());
                    //s.Send(bMessage);
                    //bMessage = null;
                    //顾名思义啦，这是一个接收信息的方法，把通过网络传过来的流存入byte数组中去。
                    //之所以把它写在这里是因为我的设计之初是，当socket连通成功之后，Client端会首先给Server端发一个信息。
                    s.Receive(bMessage);

                    sMsg = System.Text.Encoding.UTF8.GetString(bMessage);
                    Console.WriteLine("Client(" + DateTime.Now.ToShortTimeString() + "):" + sMsg);
                    Console.WriteLine("000");

                    //接下来就是输入一个字符串，并把其转成byte数组，然后Send出去。
                    //bMessage = System.Text.Encoding.UTF8.GetBytes(Console.ReadLine().ToCharArray());
                    s.Send(bMessage);
                }
                catch (System.Exception ex)
                {

                }
            }
        }

        public static void ServerListener()
        {

        }
        /*
        static TcpListener server = null;
        static TcpClient client = null;
        static NetworkStream stream = null;// 读写流
        public static void Main()
        {
            try
            {
                Int32 port = 522;
                IPAddress localAddres = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddres, port);
                server.Start();

                
                while (true)
                {
                    Console.WriteLine("等待连接ing...");
                    client = server.AcceptTcpClient();
                    Console.WriteLine("已连接！");
                    stream = client.GetStream();

                    ReceiveMessages();
                    //SendMessage();

                    // 关闭客户端连接
                    client.Close();
                }
                // 关闭客户端连接
                //client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("连接异常：{0}", e);
            }
            finally
            {
                // 关闭连接
                server.Stop();
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        public static void ReceiveMessages()
        {
            Byte[] bytes = new byte[256];
            String data = null;

            int i;

            // 循环接收客户端发送的所有数据
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // 字节流转换为字符串
                data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                Console.WriteLine("接收:{0}", data);

                data = "111";

                byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);

                // 发送响应
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("发送:{0}", data);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public static void SendMessage()
        {
            String data;

            data = "111";
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);

            // 发送响应
            stream.Write(msg, 0, msg.Length);
            Console.WriteLine("发送: {0}", data);
        }
        */
    }

    //消息操作类，用于传入线程
    public class MessageHandler
    {
        Socket socket = null;
        int index = 0;
        bool StopFlag = false;
        public MessageHandler(Socket socket, int index)
        {
            this.socket = socket;
            this.index = index;
            Console.WriteLine("线程" + this.index + "号被创建！");
        }
        //接收线程调用的函数
        public void ReceiveMessage()
        {
            byte[] buffer = new byte[1024 * 4];
            string message = null;
            try
            {
                while (!StopFlag)
                {
                    int result = this.socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    if (result < 0)
                    {
                        break;
                    }
                    message = System.Text.Encoding.UTF8.GetString(buffer);
                    Console.Write("Client(" + DateTime.Now.ToShortTimeString() + "):" + message.Trim());
                    Console.WriteLine();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString()); this.socket.Close();
            }
        }
        //发送线程调用的函数
        public void SendMessage()
        {
            byte[] buffer = new byte[1024 * 4];
            string message = null;
            try
            {
                while (!StopFlag)
                {
                    message = Console.ReadLine();
                    if (message.ToLower().Equals("byebye"))
                    {
                        StopFlag = true;
                    }
                    buffer = System.Text.Encoding.UTF8.GetBytes(message);
                    this.socket.Send(buffer);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString()); this.socket.Close();
            }
        }
    }
}