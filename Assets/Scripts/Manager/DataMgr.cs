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
        NetMgr.RegEvent(NetTag.Goods.AddShoping, GoodsResult);
        NetMgr.RegEvent(NetTag.Goods.Settlement, GoodsResult);
        // 会员相关
        NetMgr.RegEvent(NetTag.Vip.GetData, VipResult);
        NetMgr.RegEvent(NetTag.Vip.AddVip, VipResult);
        NetMgr.RegEvent(NetTag.Vip.UpdateVip, VipResult);
        NetMgr.RegEvent(NetTag.Vip.DeleteVip, VipResult);
        // 员工相关
        NetMgr.RegEvent(NetTag.Staff.GetData, StaffResult);
        NetMgr.RegEvent(NetTag.Staff.AddStaff, StaffResult);
        NetMgr.RegEvent(NetTag.Staff.UpdateStaff, StaffResult);
        NetMgr.RegEvent(NetTag.Staff.DeleteStaff, StaffResult);
        // 销售清单
        NetMgr.RegEvent(NetTag.SalesRecord.GetRecord, SalesRecord);
        NetMgr.RegEvent(NetTag.SalesRecord.Returns, SalesRecord);
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
            case "shop":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.GoodsEve.AddStop(con["data"], Convert.ToInt32(con["num"].ToString())));
                else
                    FireEvent(new Events.GoodsEve.AddStop(con["reason"].ToString()));
                break;
            case "settle":
                if (Tool.ContainsKey(con, "reason"))
                {
                    FireEvent(new Events.GoodsEve.Settlement(Convert.ToBoolean(con["result"].ToString()), con["reason"].ToString()));
                }else
                {
                    FireEvent(new Events.GoodsEve.Settlement(Convert.ToBoolean(con["result"].ToString())));
                }
                break;
            default:
                Log.Debug("操作类型有误：{0}", con["type"]);
                break;
        }
    }
    /// <summary>
    /// 会员数据
    /// </summary>
    /// <param name="con"></param>
    private void VipResult(JsonData con)
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
                    FireEvent(new Events.Vip.Get(con["data"]));
                else
                    FireEvent(new Events.Vip.Get(con["reason"].ToString()));
                break;
            case "add":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.Vip.Add(con["data"]));
                else
                    FireEvent(new Events.Vip.Add(con["reason"].ToString()));
                break;
            case "update":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.Vip.Update(con["data"]));
                else
                    FireEvent(new Events.Vip.Update(con["reason"].ToString()));
                break;
            case "delete":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.Vip.Delete(Convert.ToInt32(con["id"].ToString())));
                else
                    FireEvent(new Events.Vip.Delete(con["reason"].ToString()));
                break;
            default:
                Log.Debug("操作类型有误：{0}", con["type"]);
                break;
        }
    }
    /// <summary>
    /// 员工数据
    /// </summary>
    /// <param name="con"></param>
    private void StaffResult(JsonData con)
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
                    FireEvent(new Events.User.Get(con["data"]));
                else
                    FireEvent(new Events.User.Get(con["reason"].ToString()));
                break;
            case "add":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.User.Add(con["data"]));
                else
                    FireEvent(new Events.User.Add(con["reason"].ToString()));
                break;
            case "update":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.User.Update(con["data"]));
                else
                    FireEvent(new Events.User.Update(con["reason"].ToString()));
                break;
            case "delete":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.User.Delete(Convert.ToInt32(con["id"].ToString())));
                else
                    FireEvent(new Events.User.Delete(con["reason"].ToString()));
                break;
            default:
                Log.Debug("操作类型有误：{0}", con["type"]);
                break;
        }
    }
    /// <summary>
    /// 销售清单数据
    /// </summary>
    /// <param name="con"></param>
    private void SalesRecord(JsonData con)
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
                    FireEvent(new Events.Sales.GetRecord(con["data"]));
                else
                    FireEvent(new Events.Sales.GetRecord(con["reason"].ToString()));
                break;
            case "returns":
                if (Convert.ToBoolean(con["result"].ToString()))
                    FireEvent(new Events.Sales.Returns(con["data"]));
                else
                    FireEvent(new Events.Sales.Returns(con["reason"].ToString()));
                break;
            default:
                Log.Debug("操作类型有误：{0}", con["type"]);
                break;
        }
        
    }
}