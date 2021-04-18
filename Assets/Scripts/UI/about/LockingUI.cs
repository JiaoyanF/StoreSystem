using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;
using LitJson;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 锁定面板
/// </summary> 
public class LockingUI : UI
{
    public override UILayer Layer { get { return UILayer.Full; } }
    InputField un_input;
    InputField ps_input;
    protected override void Initialize()
    {
        un_input = GetControl<InputField>(this, "un_input");
        ps_input = GetControl<InputField>(this, "ps_input");
    }
    protected override void RegEvents()
    {
        SetBtnEvent(Get(this, "Unlock"), UnlockFunc);
        SetBtnEvent(Get(this, "Exit"), ui_mgr.Loom.ExitSystem);
        RegEventHandler<Events.Login.Confirm>(UnlockResult);
    }
    /// <summary>
    /// 解锁
    /// </summary>
    private void UnlockFunc()
    {
        if (ps_input.text == string.Empty)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("PASSWORD_NULL_TIPS")));
            return;
        }
        JsonData data = new JsonData();
        data["username"] = un_input.text;
        data["password"] = ps_input.text;
        NetMgr.SendMessage(NetTag.Login.LoginVerify, data);
    }
    private void UnlockResult(Obj sender, Events.Login.Confirm e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("LOGIN_FAIL", e.Reason)));
            return;
        }
        FireEvent(new Events.UI.OpenUI("MainForm"));
        Close();
    }
    protected override void OnEnable()
    {
        un_input.text = ui_mgr.Loom.MainUser.Id.ToString();
    }
    protected override void OnUpdate() { }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}