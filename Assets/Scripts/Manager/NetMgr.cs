using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

/// <summary>
/// 网络连接管理
/// </summary>
public class NetMgr : Obj
{
    Socket conn;// 连接
    Thread receiveThread;// 接收消息线程
    private static byte[] bytes = new byte[1024];// 消息字节组

    public override void Awake()
    {
        base.Awake();
        Connect();
    }
    protected override void RegEvents()
    {
    }

    /// <summary>
    /// 建立连接
    /// </summary>
    private void Connect()
    {
        conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress Host = IPAddress.Parse("119.29.65.81");
        // IPAddress Host = IPAddress.Parse("127.0.0.1");
        int Post = 522;
        // 把IP和端口号集成在一个网络端点中
        IPEndPoint endpoint = new IPEndPoint(Host, Post);
        try
        {
            // 连接
            conn.Connect(endpoint);

            // 创建接收消息线程
            receiveThread = new Thread(ReceiveMessages);
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

    public void SendMessage(string tag, params string[] args)
    {
        string str = tag + "#" + string.Join(",", args);
        Log.Format("发送：{0}", str);
        conn.Send(Encoding.UTF8.GetBytes(str));
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    private void ReceiveMessages()
    {
        while (true)
        {
            int receiveNumber = conn.Receive(bytes);
            string strContent = Encoding.UTF8.GetString(bytes, 0, receiveNumber);
            if (strContent == "" | strContent == null)
            {
                CloseNet();
            }
            Log.Format("接收：{0}", strContent);
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void CloseNet()
    {
        conn.Send(Encoding.UTF8.GetBytes("exit"));
        Log.Debug("断开连接");
        receiveThread.Abort();
        conn.Close();
    }
}

/// <summary>
/// 网络事件相关
/// </summary>
public struct NetTag
{
    public struct Login
    {
        public const string Req_Login = "login:request";// 登录请求
        public const string Resp_Login = "login:response";// 登录响应
    }
}