using System;
using System.Collections.Generic;
using UnityEngine;

/*
    基于Object基类的子类，继承该类的对象在不使用时必须调用Dispose
*/
public class Obj : IDisposable
{
    public SystemMgr system_mgr;
    private Map<Type, EventMgr.Handler> event_handlers = new Map<Type, EventMgr.Handler>();
    public int ID
    {
        get;
    }
    public string Name
    {
        get;
    }
    public virtual void Awake()
    {
        RegEvents();
    }
    protected virtual void RegEvents() { }
    protected virtual void Dispose(bool disposed) { }
    public void Dispose() { }

    public T GetSingleT<T>() where T : Obj, new()
    {
        if (this.system_mgr == null) { return null; }
        return this.system_mgr.GetSingleT<T>();
    }

    #region 基于事件系统相关

    public void FireEvent(Event e)
    {
        if (this.system_mgr == null | e == null)
            return;
        this.system_mgr.FireEvent(this, ref e);
    }

    protected EventMgr.Handler RegEventHandler<T>(EventMgr.OnEventRecv<T> recv) where T : Event
    {
        if (this.system_mgr == null)
        {
            return null;
        }

        if (recv == null)
            return null;

        Type etype = typeof(T);
        if (this.event_handlers.ContainsKey(etype))
            return event_handlers[etype];

        EventMgr.Handler h = this.system_mgr.RegEventHandler<T>(recv);
        if (h != null)
            this.event_handlers.Add(etype, h);
        return h;
    }

    #endregion
}