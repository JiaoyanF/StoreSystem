using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Tar;
using LitJson;
using System.Linq;

/// <summary>
/// 登录面板
/// </summary> 
public class LoginPanel : UIElement
{
    InputField un_input;
    InputField ps_input;

    protected override void Initialize()
    {
        un_input = Root.GetControl<InputField>(this, "un_input");
        ps_input = Root.GetControl<InputField>(this, "ps_input");
    }
    protected override void RegEvents()
    {
        // base.RegEvents();
        Root.SetBtnEvent(Root.Get(this, "mask"), MaskClick);
        Root.SetBtnEvent(Root.Get(this, "login"), LoadData);
        Root.SetBtnEvent(Root.Get(this, "reset"), ResetData);
        Root.RegEventHandler<Events.Login.Confirm>(LoginResult);
    }
    protected override void OnEnable()
    {
    }
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            LoadData();
        }
    }
    protected override void OnDisable()
    {
        ResetData();
    }
    protected override void OnDestroy()
    {
    }
    /// <summary>
    /// 登录加载
    /// </summary> 
    private void LoadData()
    {
        if (un_input.text == "" || ps_input.text == "")
        {
            ResetData();
            Root.FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("INPUT_IS_NULL_TIPS")));
            return;
        }
        JsonData data = new JsonData();
        data["username"] = un_input.text;
        data["password"] = ps_input.text;
        Root.NetMgr.SendMessage(NetTag.Login.LoginVerify, data);
    }
    /// <summary>
    /// 登录结果
    /// </summary>
    /// <param name="con"></param>
    private void LoginResult(Obj sender, Events.Login.Confirm e)
    {
        if (!e.Result)
        {
            Root.FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("LOGIN_FAIL", e.Reason)));
            return;
        }
        Root.ui_mgr.Loom.LoginUser(e.Staff);
        Root.FireEvent(new Events.UI.OpenUI("MainForm"));
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
        (Root as StartUI).ChangeBtnsShow();
        this.gameObject.SetActive(false);
    }
}