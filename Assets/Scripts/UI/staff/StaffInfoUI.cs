
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
/// 员工：增加、修改面板
/// </summary> 
public class StaffInfoUI : UI
{
    public override UILayer Layer { get { return UILayer.Popup; } }
    InputField index_input;// id
    InputField name_input;// 名称
    InputField password_input;// 密码
    InputField tel_input;// 电话
    InputField year_input;// 生日年
    InputField month_input;// 生日月
    InputField day_input;// 生日日
    Dropdown gender_drop;// 性别
    Dropdown power_drop;// 权限
    Staff UpdateData = null;
    protected override void Initialize()
    {
        index_input = GetControl<InputField>(this, "index_input");
        name_input = GetControl<InputField>(this, "name_input");
        password_input = GetControl<InputField>(this, "password_input");
        tel_input = GetControl<InputField>(this, "tel_input");
        year_input = GetControl<InputField>(this, "year_input");
        month_input = GetControl<InputField>(this, "month_input");
        day_input = GetControl<InputField>(this, "day_input");
        gender_drop = GetControl<Dropdown>(this, "gender_drop");
        power_drop = GetControl<Dropdown>(this, "power_drop");
    }
    protected override void RegEvents()
    {
        SetBtnEvent(Get(this, "mask"), delegate () { Close(); });
        SetBtnEvent(Get(this, "Ensure"), SureBtnClick);
        SetBtnEvent(Get(this, "Reset"), ResetBtnClick);
        RegEventHandler<Events.User.Add>(GetResult);
        RegEventHandler<Events.User.Update>(GetResult);
    }
    private void GetResult(Obj sender, Events.User.Add e)
    {
        if (e.Result == false)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADD_WIN")));
        Close();
    }
    private void GetResult(Obj sender, Events.User.Update e)
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
        if (index_input.text == string.Empty)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDSTAFF_INDEX_TIPS")));
            return;
        }
        if (name_input.text == string.Empty)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDSTAFF_NAME_TIPS")));
            return;
        }
        if (password_input.text == string.Empty)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDSTAFF_PS_TIPS")));
            return;
        }
        Staff user = new Staff();
        user.Id = index_input.text;
        user.Name = name_input.text;
        user.Password = password_input.text;
        user.Tel = tel_input.text;
        user.Power = power_drop.value;
        user.Gender = gender_drop.value;
        user.Birth = Tool.DateTimeChange(year_input.text, month_input.text, day_input.text);
        if (UpdateData != null)
        {
            NetMgr.SendMessage(NetTag.Staff.UpdateStaff, user);
            return;
        }
        NetMgr.SendMessage(NetTag.Staff.AddStaff, user);
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
        password_input.text = "";
        tel_input.text = "";
        year_input.text = "";
        month_input.text = "";
        day_input.text = "";
        gender_drop.value = 0;
        power_drop.value = 0;
    }
    /// <summary>
    /// 刷新初始数据
    /// </summary>
    private void RefreshData()
    {
        index_input.readOnly = true;
        index_input.text = UpdateData.Id.ToString();
        name_input.text = UpdateData.Name;
        password_input.text = UpdateData.Password;
        tel_input.text = UpdateData.Tel;
        if (UpdateData.Birth != string.Empty)
        {
            string[] birth = Tool.DateTimeChange(UpdateData.Birth);
            year_input.text = birth[0];
            month_input.text = birth[1];
            day_input.text = birth[2];
        }
        gender_drop.value = UpdateData.Gender;
        power_drop.value = UpdateData.Power;
    }
    protected override void OnEnable()
    {
        // 修改数据
        if (context != null && context.Length > 0)
        {
            UpdateData = context[0] as Staff;
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