using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetMgr : Obj
{
    TcpClient client = null;
    NetworkStream stream = null;

    public override void Awake()
    {
        base.Awake();
        Connect();
    }
    protected override void RegEvents()
    {
        RegEventHandler<Events.Net.SendMessage>(SendMessage);
    }

    /// <summary>
    /// 建立连接
    /// </summary>
    public void Connect()
    {
        Socket ss = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress myIP = IPAddress.Parse("127.0.0.1");
        // 把IP和端口号集成在一个网络端点中
        IPEndPoint ipe = new IPEndPoint(myIP, 522);
        byte[] bMessage = null;
        string sMsg = "连接成功！";
        try
        {
            //这是客户端的一个方法，表示连接的对象就是参数的网络端点中的IP地址和端口号
            //但是注意这里不需要返回一个新的socket作为通信socket
            //而是进行连接的这个ss就是将来一直维持此次连接的socket，直到该通道关闭或断开
            ss.Connect(ipe);

            bMessage = System.Text.Encoding.UTF8.GetBytes(sMsg.ToCharArray());
            //send方法的返回值表示已发送到socket的字节数，就像我在server端说的那样
            //这个Demo的设计思路就是连通后，客户端先向服务器端发送一个信息
            int count = ss.Send(bMessage);

            while (true)
            {
                //bMessage = null;
                ss.Receive(bMessage);
                sMsg = System.Text.Encoding.UTF8.GetString(bMessage);
                Log.Debug("Server(" + DateTime.Now.ToShortTimeString() + "):" + sMsg);
                bMessage = System.Text.Encoding.UTF8.GetBytes(Console.ReadLine().ToCharArray());
                ss.Send(bMessage);
            }
        }
        catch (ArgumentNullException ae)
        {

        }
        catch (SocketException se)
        {
            Log.Debug("SocketException:{0}", se.ToString());
        }
        catch (Exception e)
        {
        }



        // try
        // {
        //     Int32 port = 522;
        //     TcpClient client = new TcpClient("127.0.0.1", port);

        //     stream = client.GetStream();

        //     // while (true)
        //     // {
        //     //     ReceiveMessages();
        //     // }
        // }
        // catch (ArgumentNullException e)
        // {
        //     Log.Format("ArgumentNullException: {0}", e);
        // }
        // catch (SocketException e)
        // {
        //     Log.Format("SocketException: {0}", e);
        // }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public void SendMessage(Obj sender, Events.Net.SendMessage e)
    {
        string message = e.Con;
        // 将传输数据转换为二进制数组
        Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

        stream.Write(data, 0, data.Length);
        Log.Format("发送: {0}", message);
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    public void ReceiveMessages()
    {
        Byte[] data = new Byte[256];

        String responseData = String.Empty;

        Int32 bytes = stream.Read(data, 0, data.Length);
        responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
        Log.Format("接受: {0}", responseData);
    }

    public void CloseNet()
    {
        stream.Close();
        client.Close();
    }
}