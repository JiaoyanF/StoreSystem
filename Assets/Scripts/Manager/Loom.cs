using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 协调子线程和主线程等等...杂事
/// </summary>
public class Loom : MonoBehaviour
{
    public SystemMgr system_mgr;
    private List<string> net_buffer = new List<string>();
    public Staff MainUser{get; private set;}// 当前用户
    
    void Update()
    {
        ExecuteNet();
    }

    /// <summary>
    /// 添加网络消息
    /// </summary>
    /// <param name="str"></param>
    public void AddNetWork(string str)
    {
        net_buffer.Add(str);
    }
    /// <summary>
    /// 处理网络消息
    /// </summary>
    public void ExecuteNet()
    {
        if (net_buffer.Count <= 0) return;
        string[] strs = Regex.Split(net_buffer[0], "#");
        
        system_mgr.GetSingleT<NetMgr>().FireEvent(strs[0], strs[1]);
        net_buffer.RemoveAt(0);
    }
    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="s"></param>
    public void LoginUser(Staff s)
    {
        MainUser = s;
    }
    /// <summary>
    /// 退出系统
    /// </summary>
    public void ExitSystem()
    {
        system_mgr.GetSingleT<NetMgr>().CloseNet();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}