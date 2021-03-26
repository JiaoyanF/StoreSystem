using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;
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
    Map<Button, List<Button>> Btns = new Map<Button, List<Button>>();
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
        GetBtns();
    }
    protected override void RegEvents()
    {
        SubBtnClickEvent();
    }
    /// <summary>
    /// 获取标题按钮组
    /// </summary>
    void GetBtns()
    {
        foreach (Button title_btn in GetComponentChild<Button>(Get(this, "Top")))
        {
            GameObject con = Get(title_btn.gameObject, "Context");
            Btns.Add(title_btn, GetComponentChild<Button>(con));
            // 设置点击展开菜单事件
            SetBtnEvent(title_btn, delegate (){
                SetActive(con, true);
            });
        }
    }
    void SubBtnClickEvent()
    {
        // 销售子菜单
        List<Button> sales_btns = Btns[Sales];
        SetBtnEvent(sales_btns[0], Cashier);// 收银
        SetBtnEvent(sales_btns[1], SalesRecord);// 销售记录
        // 商品管理子菜单
        List<Button> goods_btns = Btns[GoodsMan];
        SetBtnEvent(goods_btns[0], AddGoods);// 添加商品
        SetBtnEvent(goods_btns[1], GoodsInfo);// 商品信息
        // 会员管理菜单
        List<Button> vip_btns = Btns[VipMan];
        SetBtnEvent(vip_btns[0], AddVip);// 加入会员
        SetBtnEvent(vip_btns[1], VipInfo);// 会员信息
        // 员工管理子菜单
        List<Button> staff_btns = Btns[StaffMan];
        SetBtnEvent(staff_btns[0], AddStaff);// 入职
        SetBtnEvent(staff_btns[1], StaffInfo);// 员工信息
        // 关于子菜单
        List<Button> about_btns = Btns[About];
        SetBtnEvent(about_btns[0], AboutShow);// 关于
        SetBtnEvent(about_btns[1], LockingSystem);// 锁定
        SetBtnEvent(about_btns[2], Logout);// 退出登录
        SetBtnEvent(about_btns[3], ui_mgr.Loom.ExitSystem);// 退出系统
    }
    void Cashier()
    {
        Log.Debug("收银");
    }
    void SalesRecord()
    {
        Log.Debug("销售记录");
    }
    void AddGoods()
    {
        Log.Debug("添加商品");
    }
    void GoodsInfo()
    {
        Log.Debug("商品信息");
    }
    void AddVip()
    {
        Log.Debug("加入会员");
    }
    void VipInfo()
    {
        Log.Debug("会员信息");
    }
    void AddStaff()
    {
        Log.Debug("入职");
    }
    void StaffInfo()
    {
        Log.Debug("员工信息");
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
    }
    protected override void OnEnable()
    {
        if (UserInfo != null && ui_mgr.Loom.MainUser != null)
        {
            UserInfo.text = Localization.Format("USER_TITLE", ui_mgr.Loom.MainUser.Name);
        }
    }
    protected override void OnUpdate()
    {
        RefreshNowTime();
        if (Input.GetMouseButtonDown(0))
        {
            HideSubpanel();
        }
    }
    /// <summary>
    /// 刷新当前时间
    /// </summary>
    void RefreshNowTime()
    {
        NowTime.text = Localization.Format("TIME_TITLE", DateTime.Now.ToString());
    }
    /// <summary>
    /// 隐藏展开菜单
    /// </summary>
    void HideSubpanel()
    {
        foreach (var item in Btns)
        {
            GameObject con = Get(item.Key.gameObject, "Context");
            SetActive(con, false);
        }
    }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}