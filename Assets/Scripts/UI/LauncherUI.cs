﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 启动面板
/// </summary> 
public class LauncherUI : MonoBehaviour
{
    private GameObject title;// 标题对象
    private GameObject center;// 中间按钮组
    private GameObject login_btn;// 登录按钮
    private GameObject close_btn;// 关闭
    private GameObject login_group;// 登录面板
    private GameObject bg;// 背景
    private LoginPanel loginPanel;// 登录面板类

    void Awake()
    {
        print("Awake");
        title = Tool.FindChild(this.transform, "title").gameObject;
        center = Tool.FindChild(this.transform, "center").gameObject;
        login_btn = Tool.FindChild(this.transform, "login_btn").gameObject;
        close_btn = Tool.FindChild(this.transform, "close_btn").gameObject;
        login_group = Tool.FindChild(this.transform, "login_group").gameObject;
        bg = Tool.FindChild(this.transform, "bg").gameObject;
    }
    void Start()
    {
        print("Start");
        // Text title_text = title.GetComponent<Text>();
        // title_text.text = "这是一个管理系统";

        loginPanel = new LoginPanel(login_group, center);

        title.GetComponent<Text>().text = "这是一个管理系统";
        login_btn.GetComponent<Button>().onClick.AddListener(LoginBtnClick);
        close_btn.GetComponent<Button>().onClick.AddListener(CloseBtnClick);
    }

    void Update()
    {

    }

    void LoginBtnClick()
    {
        if (login_group != null && login_group.activeSelf == false)
        {
            login_btn.SetActive(false);
            login_group.SetActive(true);
        }
    }
    void CloseBtnClick()
    {
        print("退出");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

/// <summary>
/// 登录面板
/// </summary> 
public class LoginPanel
{
    GameObject obj;// 自身
    GameObject mutex;// 互斥对象
    InputField un_input;
    InputField ps_input;

    /// <summary>
    /// 构造
    /// </summary> 
    /// <param name="obj">自身</param>
    /// <param name="mutex">互斥对象</param>
    public LoginPanel(GameObject obj, GameObject mutex)
    {
        this.obj = obj;
        this.mutex = mutex;
        Init();
    }
    private void Init()
    {
        Tool.FindChild(this.obj.transform, "mask").gameObject.GetComponent<Button>().onClick.AddListener(MaskClick);
        Tool.FindChild(this.obj.transform, "login").gameObject.GetComponent<Button>().onClick.AddListener(LoadData);
        Tool.FindChild(this.obj.transform, "reset").gameObject.GetComponent<Button>().onClick.AddListener(ResetData);
        un_input = Tool.FindChild(this.obj.transform, "un_input").gameObject.GetComponent<InputField>();
        ps_input = Tool.FindChild(this.obj.transform, "ps_input").gameObject.GetComponent<InputField>();
    }
    /// <summary>
    /// 登录加载
    /// </summary> 
    private void LoadData()
    {
        un_input.text = "000";
        ps_input.text = "000";
    }
    /// <summary>
    /// 重置
    /// </summary> 
    private void ResetData()
    {
        un_input.text = "";
        ps_input.text = "";
    }
    private void MaskClick()
    {
        ResetData();
        this.obj.SetActive(false);
        this.mutex.SetActive(true);
    }
}