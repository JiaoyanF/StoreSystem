using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 主面板
/// </summary> 
public class MainFormUI : UI
{
    public override UILayer Layer { get { return UILayer.Full; } }
    private Button Sales;
    private Button GoodsMan;
    private Button VipMan;
    private Button StaffMan;
    private Button About;
    private Text UserInfo;
    private Text NowTime;
    protected override void Initialize()
    {
        // Top
        Sales = GetControl<Button>(Get(this, "Sales"));
        GoodsMan = GetControl<Button>(Get(this, "GoodsMan"));
        VipMan = GetControl<Button>(Get(this, "VipMan"));
        StaffMan = GetControl<Button>(Get(this, "StaffMan"));
        About = GetControl<Button>(Get(this, "About"));
        // Bottom
        UserInfo = GetControl<Text>(Get(this, "user_info"));
        NowTime = GetControl<Text>(Get(this, "now_time"));

        ShowBG(true);
        ChangeBG("MainBG");
    }
    protected override void RegEvents()
    {
        // 销售子菜单
        GameObject SalesCon = Get(Sales, "Context");
        SetBtnEvent(Sales, delegate () { SetActive(SalesCon, true); });
        Get(Sales, "sales_item").GetComponent<PointerEx>().SetEvent(Cashier, SalesCon);// 收银
        Get(Sales, "salesrecord_item").GetComponent<PointerEx>().SetEvent(SalesRecord, SalesCon);// 销售记录
        // 商品管理子菜单
        GameObject GoodsCon = Get(GoodsMan, "Context");
        SetBtnEvent(GoodsMan, delegate () { SetActive(GoodsCon, true); });
        Get(GoodsMan, "goodadd_item").GetComponent<PointerEx>().SetEvent(AddGoods, GoodsCon);// 添加商品
        Get(GoodsMan, "goodinfo_item").GetComponent<PointerEx>().SetEvent(GoodsManage, GoodsCon);// 商品信息
        // 会员管理菜单
        GameObject VipCon = Get(VipMan, "Context");
        SetBtnEvent(VipMan, delegate () { SetActive(VipCon, true); });
        Get(VipMan, "vipadd_item").GetComponent<PointerEx>().SetEvent(AddVip, VipCon);// 加入会员
        Get(VipMan, "vipinfo_item").GetComponent<PointerEx>().SetEvent(VipManage, VipCon);// 会员信息
        // 员工管理子菜单
        GameObject StaffCon = Get(StaffMan, "Context");
        SetBtnEvent(StaffMan, delegate () { SetActive(StaffCon, true); });
        Get(StaffMan, "staffadd_item").GetComponent<PointerEx>().SetEvent(AddStaff, StaffCon);// 入职
        Get(StaffMan, "staffinfo_item").GetComponent<PointerEx>().SetEvent(StaffManage, StaffCon);// 员工信息
        // 关于子菜单
        GameObject AboutCon = Get(About, "Context");
        SetBtnEvent(About, delegate () { SetActive(AboutCon, true); });
        Get(About, "about_item").GetComponent<PointerEx>().SetEvent(AboutShow, AboutCon);// 关于
        Get(About, "lock_item").GetComponent<PointerEx>().SetEvent(LockingSystem, AboutCon);// 锁定
        Get(About, "logout_item").GetComponent<PointerEx>().SetEvent(Logout, AboutCon);// 退出登录
        Get(About, "exit_item").GetComponent<PointerEx>().SetEvent(ui_mgr.Loom.ExitSystem, AboutCon);// 退出系统
    }
    void Cashier()
    {
        Log.Debug("收银");
        FireEvent(new Events.UI.OpenUI("SettleAccounts"));
    }
    void SalesRecord()
    {
        Log.Debug("销售记录");
        FireEvent(new Events.UI.OpenUI("SalesRecord"));
    }
    void AddGoods()
    {
        Log.Debug("添加商品");
        FireEvent(new Events.UI.OpenUI("GoodsInfo"));
    }
    void GoodsManage()
    {
        Log.Debug("商品信息");
        FireEvent(new Events.UI.OpenUI("GoodsManage"));
    }
    void AddVip()
    {
        Log.Debug("加入会员");
        FireEvent(new Events.UI.OpenUI("VipInfo"));
    }
    void VipManage()
    {
        Log.Debug("会员信息");
        FireEvent(new Events.UI.OpenUI("VipManage"));
    }
    void AddStaff()
    {
        Log.Debug("入职");
        FireEvent(new Events.UI.OpenUI("StaffInfo"));
    }
    void StaffManage()
    {
        Log.Debug("员工信息");
        FireEvent(new Events.UI.OpenUI("StaffManage"));
    }
    void AboutShow()
    {
        Log.Debug("关于显示");
    }
    void LockingSystem()
    {
        Log.Debug("锁定");
        FireEvent(new Events.UI.OpenUI("Locking"));
    }
    void Logout()
    {
        Log.Debug("退出登录");
        SceneManager.LoadScene("Launcher");
    }
    protected override void OnEnable()
    {
        // 设置用户信息
        if (UserInfo != null && ui_mgr.Loom.MainUser != null)
        {
            UserInfo.text = Localization.Format("USER_TITLE", ui_mgr.Loom.MainUser.Name, ui_mgr.Loom.MainUser.Power.ToString());
        }
    }
    protected override void OnUpdate()
    {
        RefreshNowTime();
    }
    /// <summary>
    /// 刷新当前时间
    /// </summary>
    void RefreshNowTime()
    {
        NowTime.text = Localization.Format("TIME_TITLE", DateTime.Now.ToString());
    }

    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}