using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using MySql.Data.MySqlClient;

/// <summary>
/// 系统管理器
/// </summary>
public class SystemMgr : EventMgr
{
    public SystemMgr own { get; private set; }// 自身单例
    private Map<Type, Obj> Managers = new Map<Type, Obj>();// 管理器单例们
    public void Launch(SystemMgr own)
    {
        this.own = own;
        own.Start();
    }

    public void Start()
    {
        // LoadDatabase();
        GetSingleT<NetMgr>();// 网络管理器
        GetSingleT<ResourcesMgr>();// 创建资源管理器
        GetSingleT<UIMgr>();// 创建ui管理器
    }

    /// <summary>
    /// 加载数据库
    /// </summary>
    public void LoadDatabase()
    {
        string connetStr = "server=119.29.65.81;port=3306;user=root;password=0522; database=StoreSystem;";
        // server=127.0.0.1/localhost 代表本机，端口号port默认是3306可以不写
        MySqlConnection conn = new MySqlConnection(connetStr);
        try
        {
            conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
            Log.Debug("连接数据库成功");
            //在这里使用代码对数据库进行增删查改
        }
        catch (MySqlException ex)
        {
            Log.Debug("报错：" + ex.Message);
        }
        finally
        {
            Log.Debug("关闭数据库连接");
            conn.Close();
        }

        // string MyconnecString = "Server=119.29.65.81;Database=han;Uid=root;Pwd=0522";
        // // string mysqlcon = "Database=StoreSystem;Data Source=localhost;User id=root;password=0522;charset=utf8;port=3306";
        // MySqlConnection con = new MySqlConnection(MyconnecString);
        // MySqlCommand cmd = new MySqlCommand();
        // con.Open();
        // cmd = con.CreateCommand();
        // // cmd.CommandType = CommandType.Text;
        // cmd.CommandText = "INSERT INTO stu VALUES (NULL,'Abcdd','Xuanwumen 10',122,'Abcdd','Xuanwumen 10')";
        // cmd.ExecuteNonQuery();
        // con.Close();
    }

    /// <summary>
    /// 获取单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetSingleT<T>() where T : Obj, new()
    {
        Type type = typeof(T);
        if (Managers.ContainsKey(type))
        {
            T mgr = Managers[type] as T;
            return mgr;
        }
        T obj = new T();
        if (obj == null)
            return null;
        obj.system_mgr = this;// 传递系统管理器
        Managers.Add(type, obj);
        obj.Awake();
        return obj;
    }
}