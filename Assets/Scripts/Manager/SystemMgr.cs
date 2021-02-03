using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 系统管理器
/// </summary>
public class SystemMgr : EventMgr
{
    public SystemMgr own { get; private set; }// 自身单例
    private Map<Type, Obj> Managers  = new Map<Type, Obj>();// 管理器单例们
    public void Launch(SystemMgr own)
    {
        this.own = own;
        own.Start();
    }

    public void Start()
    {
        GetSingleT<ResourcesMgr>();// 创建资源管理器
        GetSingleT<UIMgr>();// 创建ui管理器
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