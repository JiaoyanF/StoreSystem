using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;

/// <summary>
/// 启动面板
/// </summary> 
public class StartUI : UI
{
    public override UILayer Layer { get { return UILayer.Normal; } }
    private GameObject title;// 标题对象
    private GameObject center;// 中间按钮组
    private GameObject login_btn;// 登录按钮
    private GameObject about_btn;// 关于按钮
    private GameObject close_btn;// 关闭
    private GameObject login_panel;// 登录面板
    private GameObject bg;// 背景
    private LoginPanel loginPanel;// 登录面板实例

    protected override void Initialize()
    {
        title = Get(this, "title");
        center = Get(this, "center");
        login_btn = Get(this, "login_btn");
        about_btn = Get(this, "about_btn");
        close_btn = Get(this, "close_btn");
        login_panel = Get(this, "LoginPanel");
        bg = Get(this, "bg");

        loginPanel = NewElement<LoginPanel>(this, login_panel);// 实例化登录面板
    }
    protected override void RegEvents()
    {
        SetBtnEvent(login_btn, LoginBtnClick);
        SetBtnEvent(about_btn, AboutClick);
        SetBtnEvent(close_btn, CloseBtnClick);
    }
    protected override void OnEnable()
    {
        SetText<Text>(title, Localization.Format("SYSTEM_NAME"));
    }
    protected override void OnUpdate() { }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
    /// <summary>
    /// 登录按钮点击
    /// </summary>
    private void LoginBtnClick()
    {
        if (login_panel != null && login_panel.activeSelf == false)
        {
            ChangeBtnsShow();
            login_panel.SetActive(true);
        }
    }
    /// <summary>
    /// 关于按钮点击
    /// </summary>
    private void AboutClick()
    {
        // ChangeBtnsShow();
        // print("显示关于面板");
        FireEvent(new Events.UI.OpenUI("About"));
    }
    /// <summary>
    /// 退出按钮
    /// </summary>
    private void CloseBtnClick()
    {
        ui_mgr.Loom.ExitSystem();
    }
    /// <summary>
    /// 按钮组显示状态改变
    /// </summary>
    public void ChangeBtnsShow()
    {
        center.SetActive(center.activeSelf == false);
    }
}