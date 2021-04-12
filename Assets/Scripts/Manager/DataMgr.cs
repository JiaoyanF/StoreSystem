using UnityEngine;
using System;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using Def;

/// <summary>
/// 数据管理器：分发服务器发来的数据
/// </summary>
public class DataMgr : Obj
{
    NetMgr NetMgr { get { return system_mgr.GetSingleT<NetMgr>(); } }
    public override void Awake()
    {
        base.Awake();
    }
    protected override void RegEvents()
    {
        // 登录相关
        NetMgr.RegEvent(NetTag.Login.LoginVerify, LoginResult);
        // 商品相关
        NetMgr.RegEvent(NetTag.Goods.AddGoods, GoodsResult);
        NetMgr.RegEvent(NetTag.Goods.UpdateGoods, GoodsResult);
        NetMgr.RegEvent(NetTag.Goods.GetData, GoodsResult);
        NetMgr.RegEvent(NetTag.Goods.DeleteGoods, GoodsResult);
    }
    /// <summary>
    /// 登录数据
    /// </summary>
    /// <param name="con"></param>
    private void LoginResult(JsonData con)
    {
        if (Convert.ToBoolean(con["result"].ToString()))
            FireEvent(new Events.Login.Confirm(con["user"]));
        else
            FireEvent(new Events.Login.Confirm(con["reason"].ToString()));
    }
    /// <summary>
    /// 商品相关数据
    /// </summary>
    /// <param name="con"></param>
    private void GoodsResult(JsonData con)
    {
        if (!Tool.ContainsKey(con, "result") || !Tool.ContainsKey(con, "type"))
        {
            Log.Debug("缺少返回结果或操作类型：{0}", con.ToJson());
            return;
        }
        switch (con["type"].ToString())
        {
            case "get":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.GoodsEve.Get(con["data"]));
                else
                    FireEvent(new Events.GoodsEve.Get(con["reason"].ToString()));
                break;
            case "add":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.GoodsEve.Add(con["data"]));
                else
                    FireEvent(new Events.GoodsEve.Add(con["reason"].ToString()));
                break;
            case "update":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.GoodsEve.Update(con["data"]));
                else
                    FireEvent(new Events.GoodsEve.Update(con["reason"].ToString()));
                break;
            case "delete":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.GoodsEve.Delete(Convert.ToInt32(con["id"].ToString())));
                else
                    FireEvent(new Events.GoodsEve.Delete(con["reason"].ToString()));
                break;
            default:
                Log.Debug("操作类型有误：{0}", con["type"]);
                break;
        }
    }
}