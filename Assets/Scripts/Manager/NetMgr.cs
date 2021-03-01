using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

public class NetMgr : Obj
{
    Socket conn;// 连接
    private static byte[] bytes = new byte[1024];// 消息字节组

    public override void Awake()
    {
        base.Awake();
        Connect();
    }
    protected override void RegEvents()
    {
        RegEventHandler<Events.Net.SendMessage>(SendMessage);// 发送消息事件
    }

    /// <summary>
    /// 建立连接
    /// </summary>
    public void Connect()
    {
        conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress Host = IPAddress.Parse("119.29.65.81");
        int Post = 522;
        // 把IP和端口号集成在一个网络端点中
        IPEndPoint endpoint = new IPEndPoint(Host, Post);
        try
        {
            // 连接
            conn.Connect(endpoint);

            // 创建接收消息线程
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }
        catch (SocketException e)
        {
            Log.Format("连接错误:{0}", e.ToString());
        }
        catch (Exception e)
        {
            Log.Format("错误:{0}", e.ToString());
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public void SendMessage(Obj sender, Events.Net.SendMessage e)
    {
        Log.Format("发送：{0}", e.Con);
        conn.Send(Encoding.UTF8.GetBytes(e.Con));
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    public void ReceiveMessages()
    {
        while (true)
        {
            int receiveNumber = conn.Receive(bytes);
            string strContent = Encoding.UTF8.GetString(bytes, 0, receiveNumber);
            Log.Format("接收：{0}", strContent);
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void CloseNet()
    {
        FireEvent(new Events.Net.SendMessage("exit"));
        conn.Close();
    }
}