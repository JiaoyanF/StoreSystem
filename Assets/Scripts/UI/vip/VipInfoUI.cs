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
/// 会员：增加、修改面板
/// </summary> 
public class VipInfoUI : UI
{
    public override UILayer Layer { get { return UILayer.Popup; } }
    InputField index_input;// id
    InputField name_input;// 名称
    InputField point_input;// 积分
    InputField year_input;// 生日年
    InputField month_input;// 生日月
    InputField day_input;// 生日日
    Dropdown gender_drop;// 性别
    Member UpdateData = null;
    protected override void Initialize()
    {
        index_input = GetControl<InputField>(this, "index_input");
        name_input = GetControl<InputField>(this, "name_input");
        point_input = GetControl<InputField>(this, "point_input");
        year_input = GetControl<InputField>(this, "year_input");
        month_input = GetControl<InputField>(this, "month_input");
        day_input = GetControl<InputField>(this, "day_input");
        gender_drop = GetControl<Dropdown>(this, "gender_drop");
    }
    protected override void RegEvents()
    {
        SetBtnEvent(Get(this, "mask"), delegate () { Close(); });
        SetBtnEvent(Get(this, "Ensure"), SureBtnClick);
        SetBtnEvent(Get(this, "Reset"), ResetBtnClick);
        RegEventHandler<Events.Vip.Add>(GetResult);
        RegEventHandler<Events.Vip.Update>(GetResult);
    }
    private void GetResult(Obj sender, Events.Vip.Add e)
    {
        if (e.Result == false)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
        }
        FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADD_WIN")));
        Close();
    }
    private void GetResult(Obj sender, Events.Vip.Update e)
    {
        if (e.Result == false)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UPDATE_WIN")));
        Close();
    }
    private void SureBtnClick()
    {
        if (index_input.text == "")
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDVIP_INDEX_TIPS")));
            return;
        }
        if (name_input.text == "")
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDVIP_NAME_TIPS")));
            return;
        }
        if (point_input.text != string.Empty && int.Parse(point_input.text) < 0)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDVIP_POINT_TIPS")));
            return;
        }
        Member vip = new Member();
        vip.Id = index_input.text;
        vip.Name = name_input.text;
        vip.Point = point_input.text != string.Empty ? int.Parse(point_input.text) : 0;
        vip.Gender = gender_drop.value;
        vip.Birth = Tool.DateTimeChange(year_input.text, month_input.text, day_input.text);
        if (UpdateData != null)
        {
            NetMgr.SendMessage(NetTag.Vip.UpdateVip, vip);
            return;
        }
        NetMgr.SendMessage(NetTag.Vip.AddVip, vip);
    }
    private void ResetBtnClick()
    {
        if (UpdateData != null)
        {
            RefreshData();
            return;
        }
        index_input.text = "";
        name_input.text = "";
        point_input.text = "";
        year_input.text = "";
        month_input.text = "";
        day_input.text = "";
        gender_drop.value = 0;
    }
    /// <summary>
    /// 刷新初始数据
    /// </summary>
    private void RefreshData()
    {
        index_input.readOnly = true;
        index_input.text = UpdateData.Id.ToString();
        name_input.text = UpdateData.Name;
        point_input.text = UpdateData.Point.ToString();
        string[] birth = Tool.DateTimeChange(UpdateData.Birth);
        year_input.text = birth[0];
        month_input.text = birth[1];
        day_input.text = birth[2];
        gender_drop.value = UpdateData.Gender;
    }
    protected override void OnEnable()
    {
        // 修改数据
        if (context != null && context.Length > 0)
        {
            UpdateData = context[0] as Member;
            RefreshData();
        }
    }
    protected override void OnUpdate() { }
    protected override void OnDisable()
    {
        context = null;
        UpdateData = null;
    }
    protected override void OnDestroy() { }
}