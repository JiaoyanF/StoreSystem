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
/// 协调子线程和主线程
/// </summary>
public class Loom : MonoBehaviour
{
    public SystemMgr system_mgr;
    private List<string> net_buffer = new List<string>();
    public void AddNetWork(string str)
    {
        net_buffer.Add(str);
    }
    public void InitData()
    {
        
    }
    void Update()
    {
        if (net_buffer.Count > 0)
        {
            string[] strs = Regex.Split(net_buffer[0], "#");
            system_mgr.GetSingleT<NetMgr>().FireEvent(strs[0], strs[1]);
            net_buffer.RemoveAt(0);
        }
    }
}