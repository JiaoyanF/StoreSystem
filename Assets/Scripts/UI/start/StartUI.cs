using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 启动面板
/// </summary> 
public class StartUI : UI
{
    private GameObject title;// 标题对象
    private GameObject center;// 中间按钮组
    private GameObject login_btn;// 登录按钮
    private GameObject about_btn;// 关于按钮
    private GameObject close_btn;// 关闭
    private GameObject login_panel;// 登录面板
    private GameObject bg;// 背景
    private LoginPanel loginPanel;// 登录面板类

    public override void Start()
    {
        // base.Start();
        title = Get(this, "title");
        center = Get(this, "center");
        login_btn = Get(this, "login_btn");
        about_btn = Get(this, "about_btn");
        close_btn = Get(this, "close_btn");
        login_panel = Get(this, "login_panel");
        bg = Get(this, "bg");
        
        loginPanel = new LoginPanel(login_panel, center);
        Tool.GetOrAddComponent<LoginPanel>(login_panel);

        SetText<Text>(title, "这是一个管理系统");
        
        SetBtnEvent(login_btn, LoginBtnClick);
        SetBtnEvent(about_btn, AboutClick);
        SetBtnEvent(close_btn, CloseBtnClick);

        Log.Debug("context", context);
    }

    /// <summary>
    /// 登录按钮点击
    /// </summary>
    private void LoginBtnClick()
    {
        if (login_panel != null && login_panel.activeSelf == false)
        {
            center.SetActive(false);
            login_panel.SetActive(true);
        }
    }
    /// <summary>
    /// 关于按钮点击
    /// </summary>
    private void AboutClick()
    {
        print("点击关于");
        center.SetActive(false);
        // about_pane
    }
    private void CloseBtnClick()
    {
        print("退出");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

public class AboutPanel
{

}