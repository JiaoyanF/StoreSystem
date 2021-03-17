using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 登录面板
/// </summary> 
public class LoginPanel : UIElement
{
    InputField un_input;
    InputField ps_input;

    protected override void Initialize()
    {
        un_input = GetControl<InputField>(Get(this, "un_input"));
        ps_input = GetControl<InputField>(Get(this, "ps_input"));
    }
    protected override void RegEvents()
    {
        // base.RegEvents();
        SetBtnEvent(Get(this, "mask"), MaskClick);
        SetBtnEvent(Get(this, "login"), LoadData);
        SetBtnEvent(Get(this, "reset"), ResetData);
        Root.NetMgr.RegEvent(NetTag.Login.Resp_Login, LoginResult);
    }
    protected override void OnEnable()
    {
    }
    protected override void OnUpdata()
    {
    }
    protected override void OnDisable()
    {
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
            Log.Debug("数据不完整");
            return;
        }
        Root.NetMgr.SendMessage(NetTag.Login.Req_Login, un_input.text, ps_input.text);
    }
    /// <summary>
    /// 登录结果
    /// </summary>
    /// <param name="con"></param>
    private void LoginResult(Map<string, string> con)
    {
        if (Convert.ToBoolean(con["result"]))
        {
            FireEvent(new Events.UI.OpenUI("MainForm"));
        }else
        {
            Log.Debug(con["reason"]);
        }
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
        (Root as StartUI).ChangeBtnsShow();
        this.gameObject.SetActive(false);
    }
}