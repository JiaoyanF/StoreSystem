using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using Tar;

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
    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void FireEvent(Obj sender, ref Event e)
    {
        HandlerGroup g;
        if (event_handlers.TryGetValue(e.GetType(), out g))
            g.CallHandler(sender, ref e);
    }
    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="recv"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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
    /// <summary>
    /// 移除注册事件
    /// </summary>
    /// <param name="h"></param>
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
    /// <summary>
    /// 事件信息接口
    /// </summary>
    public interface Handler
    {
        int Key { get; }
        Type EventType { get; }
        void Call(Obj s, ref Event e);
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
    /// <summary>
    /// 事件组
    /// </summary>
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
        public struct CloseUI : Event
        {
            public string UI;
            public CloseUI(string ui)
            {
                this.UI = ui;
            }
        }
    }
    /// <summary>
    /// 登录事件
    /// </summary>
    public struct Login
    {
        /// <summary>
        /// 登录确认
        /// </summary>
        public struct Confirm : Event
        {
            public bool Result;
            public Staff Staff;
            public string Reason;
            public Confirm(JsonData json)
            {
                this.Result = true;
                this.Staff = new Staff(json);
                this.Reason = null;
            }
            public Confirm(string reason)
            {
                this.Result = false;
                this.Staff = null;
                this.Reason = reason;
            }
        }
    }
    /// <summary>
    /// 商品相关事件
    /// </summary>
    public struct GoodsEve
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public struct Get : Event
        {
            public bool Result;
            public List<Goods> Data;
            public string Reason;
            public Get(JsonData json)
            {
                this.Result = true;
                this.Data = new List<Goods>();
                foreach (JsonData item in json)
                {
                    this.Data.Add(new Goods(item));
                }
                this.Reason = null;
            }
            public Get(string reason)
            {
                this.Result = false;
                this.Data = null;
                this.Reason = reason;
            }
        }
        /// <summary>
        /// 添加商品
        /// </summary>
        public struct Add : Event
        {
            public bool Result;
            public Goods NewGoods;
            public string Reason;
            public Add(JsonData json)
            {
                this.Result = true;
                this.NewGoods = new Goods(json);
                this.Reason = null;
            }
            public Add(string reason)
            {
                this.Result = false;
                this.NewGoods = null;
                this.Reason = reason;
            }
        }
        /// <summary>
        /// 修改商品
        /// </summary>
        public struct Update : Event
        {
            public bool Result;
            public Goods NewGoods;
            public string Reason;
            public Update(JsonData json)
            {
                this.Result = true;
                this.NewGoods = new Goods(json);
                this.Reason = null;
            }
            public Update(string reason)
            {
                this.Result = false;
                this.NewGoods = null;
                this.Reason = reason;
            }
        }
        /// <summary>
        /// 删除商品
        /// </summary>
        public struct Delete : Event
        {
            public bool Result;
            public string Id;
            public string Reason;
            public Delete(JsonData id)
            {
                this.Result = true;
                this.Id = id.ToString();
                this.Reason = null;
            }
            public Delete(string reason)
            {
                this.Result = false;
                this.Id = null;
                this.Reason = reason;
            }
        }
        /// <summary>
        /// 添加购物车
        /// </summary>
        public struct AddStop : Event
        {
            public bool Result;
            public Goods Data;
            public int Num;
            public string Reason;
            public AddStop(JsonData json, int num)
            {
                this.Result = true;
                this.Data = new Goods(json);
                this.Num = num;
                this.Reason = null;
            }
            public AddStop(string reason)
            {
                this.Result = false;
                this.Data = null;
                this.Num = -1;
                this.Reason = reason;
            }
        }
        public struct SureSettlement : Event
        {
            public List<Goods> Data;
            public SureSettlement(List<Goods> data)
            {
                this.Data = data;
            }
        }
        /// <summary>
        /// 结账结果
        /// </summary>
        public struct Settlement : Event
        {
            public bool Result;
            public string Reason;
            public Settlement(bool result, string reason = null)
            {
                this.Result = result;
                this.Reason = reason;
            }
        }
    }
    /// <summary>
    /// 会员相关事件
    /// </summary>
    public struct Vip
    {
        public struct Get : Event
        {
            public bool Result;
            public List<Member> Data;
            public string Reason;
            public Get(JsonData json)
            {
                this.Result = true;
                List<Member> list = new List<Member>();
                foreach (JsonData item in json)
                {
                    list.Add(new Member(item));
                }
                this.Data = list;
                this.Reason = null;
            }
            public Get(string reason)
            {
                this.Result = false;
                this.Data = null;
                this.Reason = reason;
            }
        }
        public struct Add : Event
        {
            public bool Result;
            public Member NewVip;
            public string Reason;
            public Add(JsonData json)
            {
                this.Result = true;
                this.NewVip = new Member(json);
                this.Reason = null;
            }
            public Add(string reason)
            {
                this.Result = false;
                this.NewVip = null;
                this.Reason = reason;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        public struct Update : Event
        {
            public bool Result;
            public Member Data;
            public string Reason;
            public Update(JsonData json)
            {
                this.Result = true;
                this.Data = new Member(json);
                this.Reason = null;
            }
            public Update(string reason)
            {
                this.Result = false;
                this.Data = null;
                this.Reason = reason;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public struct Delete : Event
        {
            public bool Result;
            public string Id;
            public string Reason;
            public Delete(JsonData id)
            {
                this.Result = true;
                this.Id = id.ToString();
                this.Reason = null;
            }
            public Delete(string reason)
            {
                this.Result = false;
                this.Id = null;
                this.Reason = reason;
            }
        }
    }
    /// <summary>
    /// 员工相关事件
    /// </summary>
    public struct User
    {
        public struct Get : Event
        {
            public bool Result;
            public List<Staff> Data;
            public string Reason;
            public Get(JsonData json)
            {
                this.Result = true;
                List<Staff> list = new List<Staff>();
                foreach (JsonData item in json)
                {
                    list.Add(new Staff(item));
                }
                this.Data = list;
                this.Reason = null;
            }
            public Get(string reason)
            {
                this.Result = false;
                this.Data = null;
                this.Reason = reason;
            }
        }
        public struct Add : Event
        {
            public bool Result;
            public Staff NewStaff;
            public string Reason;
            public Add(JsonData json)
            {
                this.Result = true;
                this.NewStaff = new Staff(json);
                this.Reason = null;
            }
            public Add(string reason)
            {
                this.Result = false;
                this.NewStaff = null;
                this.Reason = reason;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        public struct Update : Event
        {
            public bool Result;
            public Staff Data;
            public string Reason;
            public Update(JsonData json)
            {
                this.Result = true;
                this.Data = new Staff(json);
                this.Reason = null;
            }
            public Update(string reason)
            {
                this.Result = false;
                this.Data = null;
                this.Reason = reason;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public struct Delete : Event
        {
            public bool Result;
            public string Id;
            public string Reason;
            public Delete(JsonData id)
            {
                this.Result = true;
                this.Id = id.ToString();
                this.Reason = null;
            }
            public Delete(string reason)
            {
                this.Result = false;
                this.Id = null;
                this.Reason = reason;
            }
        }
    }
    /// <summary>
    /// 销售记录
    /// </summary>
    public struct Sales
    {
        public struct GetRecord : Event
        {
            public bool Result;
            public List<Record> Data;
            public string Reason;
            public GetRecord(JsonData json)
            {
                this.Result = true;
                this.Reason = null;
                this.Data = new List<Record>();
                foreach (JsonData item in json)
                {
                    this.Data.Add(new Record(item));
                }
            }
            public GetRecord(string reason)
            {
                this.Result = false;
                this.Data = null;
                this.Reason = reason;
            }
        }
        /// <summary>
        /// 退货
        /// </summary>
        public struct Returns : Event
        {
            public bool Result;
            public Record Data;
            public string Reason;
            public Returns(JsonData json)
            {
                this.Result = true;
                this.Reason = null;
                this.Data = new Record(json);
            }
            public Returns(string reason)
            {
                this.Result = false;
                this.Data = null;
                this.Reason = reason;
            }
        }
    }
    /// <summary>
    /// 网络事件
    /// </summary>
    public struct Net
    {
        // 断开事件、连接事件等
    }
}