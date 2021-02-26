using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//内部事件接口
public interface Event { }

/// <summary>
/// 事件管理器
/// </summary>
public class EventMgr
{
    private Map<Type, HandlerGroup> event_handlers = new Map<Type, HandlerGroup>();

    public delegate void OnEventRecv(Obj sender, Event e);
    public delegate void OnEventRecv<T>(Obj sender, T e);

    public void FireEvent(Obj sender, ref Event e)
    {
        HandlerGroup g;
        if (event_handlers.TryGetValue(e.GetType(), out g))
            g.CallHandler(sender, ref e);
    }

    public Handler RegEventHandler<T>(OnEventRecv<T> recv) where T : Event
    {
        HandlerGroup g;
        if (!event_handlers.TryGetValue(typeof(T), out g))
        {
            g = new HandlerGroup(typeof(T));
            event_handlers.Add(typeof(T), g);
        }
        return g.Create<T>(recv);
    }

    public Handler RegEventHandler(OnEventRecv recv, Type t)
    {
        HandlerGroup g;
        if (!event_handlers.TryGetValue(t, out g))
        {
            g = new HandlerGroup(t);
            event_handlers.Add(t, g);
        }
        return g.Create(recv, t);
    }

    public void UnregEventHandler(Handler h)
    {
        UnregEventHandler(h.EventType, h);
    }

    public void UnregEventHandler<T>(Handler h)
    {
        UnregEventHandler(typeof(T), h);
    }

    public void UnregEventHandler(Type type, Handler h)
    {
        HandlerGroup g;
        if (event_handlers.TryGetValue(type, out g))
            g.Remove(h);
    }



    public interface Handler
    {
        int Key { get; }
        Type EventType { get; }
        void Call(Obj s, ref Event e);
    }

    struct EventHandler : Handler
    {
        private int key;
        private Type eventType;
        private OnEventRecv recv;

        public Type EventType { get { return this.eventType; } }
        public int Key { get { return this.key; } }

        public EventHandler(int key, OnEventRecv recv, Type t)
        {
            this.key = key;
            this.recv = recv;
            this.eventType = t;
        }

        public void Call(Obj s, ref Event e)
        {

            if (recv != null)
                recv(s, e);
        }
    }

    struct RealHandler<T> : Handler where T : Event
    {
        private int key;
        private OnEventRecv<T> recv;
        public int Key { get { return this.key; } }
        public Type EventType { get { return typeof(T); } }

        public RealHandler(int key, OnEventRecv<T> recv)
        {
            this.key = key;
            this.recv = recv;
        }

        public void Call(Obj s, ref Event e)
        {
            if (recv != null)
            {
                recv(s, (T)e);
            }
        }
    }

    internal class HandlerGroup
    {
        private UniqueIndex index_pool = new UniqueIndex(10);
        private Handler[] handlers = new Handler[10];
        internal HandlerGroup(Type event_type)
        {
            // this.type = event_type; 
        }

        public Handler Create<T>(OnEventRecv<T> recv) where T : Event
        {
            if (!index_pool.CanAlloc())
            {
                index_pool.Grow(5);
                Array.Resize(ref handlers, handlers.Length + 5);
            }

            int idx = index_pool.Alloc();
            RealHandler<T> h = new RealHandler<T>(idx, recv);
            handlers[idx] = h;
            return h;
        }

        public Handler Create(OnEventRecv recv, Type t)
        {
            if (!index_pool.CanAlloc())
            {
                index_pool.Grow(5);
                Array.Resize(ref handlers, handlers.Length + 5);
            }

            int idx = index_pool.Alloc();
            EventHandler h = new EventHandler(idx, recv, t);
            handlers[idx] = h;
            return h;
        }

        public void Remove(Handler h)
        {
            if (h == null) return;
            handlers[h.Key] = null;
            index_pool.Free(h.Key);
        }

        public void CallHandler(Obj s, ref Event e)
        {
            for (int i = 0; i < handlers.Length; ++i)
            {
                if (handlers[i] == null)
                    continue;
                handlers[i].Call(s, ref e);
            }
        }
    }
}

/// <summary>
/// 事件类
/// </summary>
public static class Events
{
    /// <summary>
    /// UI事件
    /// </summary>
    public struct UI
    {
        /// <summary>
        /// 打开UI
        /// </summary>
        public struct OpenUI : Event
        {
            public string UI;
            public object[] Args;
            public OpenUI(string ui, params object[] args)
            {
                this.UI = ui;
                this.Args = args;
            }
        }
    }

    /// <summary>
    /// 网络事件
    /// </summary>
    public struct Net
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        public struct SendMessage : Event
        {
            public string Con;
            public SendMessage(string con)
            {
                this.Con = con;
            }
        }
    }
}