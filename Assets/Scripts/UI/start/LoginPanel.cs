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
            Log.Debug("数据不完整");
            return;
        }
        Root.NetMgr.SendMessage(NetTag.Login.Req_Login, un_input.text, ps_input.text);
    }
    /// <summary>
    /// 登录结果
    /// </summary>
    /// <param name="con"></param>
    private void LoginResult(JsonData con)
    {
        if (Convert.ToBoolean(con["result"].ToString()))
        {
            // if (con.ContainsKey("user"))// 为啥没有
            // {
            //     // Staff user = new Staff(con["user"].id);
            //     // Root.ui_mgr.system_mgr.Loom.LoginUser(user);
            // }
            FireEvent(new Events.UI.OpenUI("MainForm"));
        }else
        {
            Log.Format("登录失败：{0}", con["reason"].ToString());
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
        (Root as StartUI).ChangeBtnsShow();
        this.gameObject.SetActive(false);
    }
}