using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Newtonsoft.Json;
using LitJson;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <summary>
/// 网络连接管理
/// </summary>
public class NetMgr : Obj
{
    Socket conn;// 连接
    Thread receiveThread;// 接收消息线程
    private static byte[] bytes = new byte[1024];// 消息字节组
    private Map<Type, NetEventHandler> Events = new Map<Type, NetEventHandler>();

    public delegate void NetEventRecv(JsonData context);// 网络事件回调
    bool is_connect;

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
        // IPAddress Host = IPAddress.Parse("119.29.65.81");
        IPAddress Host = IPAddress.Parse("127.0.0.1");
        int Post = 522;
        // 把IP和端口号集成在一个网络端点中
        IPEndPoint endpoint = new IPEndPoint(Host, Post);
        try
        {
            // 连接
            conn.Connect(endpoint);
            is_connect = true;

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
            if (strContent == "" | strContent == null | strContent == "exit")
            {
                is_connect = false;
                CloseNet();
            }
            Log.Format("接收：{0}", strContent);
            if (strContent.Contains("#"))
            {
                system_mgr.Loom.AddNetWork(strContent);
            }
        }
    }
    public void FireEvent(string tag, string context)
    {
        Type type = tag.GetType();
        if (Events.ContainsKey(type))
        {
            Events[type].Call(context);
        }else
        {
            Log.Format("协议【{0}】未定义事件", tag);
        }
    }

    public void RegEvent(string tag, NetEventRecv action)
    {
        Type type = tag.GetType();
        NetEventHandler net_event = new NetEventHandler(type, action);
        Events.Add(type, net_event);
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void CloseNet()
    {
        if (is_connect)
        {
            conn.Send(Encoding.UTF8.GetBytes("exit"));
            Log.Debug("断开连接");
            receiveThread.Abort();
            conn.Close();
        }
    }

    public interface Handler
    {
        Type EventType { get; }
        void Call(string s);
    }

    struct NetEventHandler : Handler
    {
        private Type eventType;
        private NetEventRecv recv;
        public Type EventType { get { return this.eventType; } }

        public NetEventHandler(Type t, NetEventRecv recv)
        {
            this.eventType = t;
            this.recv = recv;
        }
        public void Call(string s)
        {
            if (recv == null)
                return;
            JsonData jsonData = JsonMapper.ToObject(s);
            // Map<string, object> jsonDict = JsonConvert.DeserializeObject<Map<string, object>>(s);
            // foreach (KeyValuePair<string, object> item in jsonDict)
            //     Log.Debug(String.Format("{0}：{1}", item.Key, item.Value));
            recv(jsonData);
        }
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
    public struct Data
    {
        public const string Req_UserData = "user:get_data";// 请求：用户数据
        public const string Resp_UserData = "user:data";// 响应：用户数据
    }
}