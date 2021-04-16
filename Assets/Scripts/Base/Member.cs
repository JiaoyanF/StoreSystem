using Tar;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 会员
/// </summary>
public class Member : BaseData
{
    public int Gender;// 性别
    public int Point;// 积分
    public string Birth;// 出生日期
    public string Enter;// 入会日期
    public Member() { }
    public Member(JsonData json)
    {
        this.Tag = Convert.ToInt32(Def.DataList.SalesRecord);
        this.Id = json["id"] != null ? json["id"].ToString() : string.Empty;
        this.Name = json["name"] != null ? json["name"].ToString() : string.Empty;
        this.Gender = json["gender"] != null ? Convert.ToInt32(json["gender"].ToString()) : 0;
        this.Birth = json["birth"] != null ? json["birth"].ToString() : string.Empty;
        this.Enter = json["enter"] != null ? json["enter"].ToString() : string.Empty;
        this.Point = json["point"] != null ? Convert.ToInt32(json["point"].ToString()) : 0;
    }
}

/// <summary>
/// 会员列表项
/// </summary>
public class VipItem : UIElement
{
    GameObject select;// 选中高亮
    Text index;
    Text vip_name;
    Text gender;
    Text point;
    Text birth;
    Text enter;
    public Member data;// 数据
    public delegate void OnItemClick(VipItem item);
    public OnItemClick ClickFunc;
    protected override void Initialize()
    {
        select = Root.Get(this, "select");
        index = Root.GetControl<Text>(this, "tel");
        vip_name = Root.GetControl<Text>(this, "name");
        gender = Root.GetControl<Text>(this, "gender");
        point = Root.GetControl<Text>(this, "point");
        birth = Root.GetControl<Text>(this, "birth");
        enter = Root.GetControl<Text>(this, "enter");
    }
    protected override void RegEvents()
    {
        Root.SetBtnEvent(this.gameObject, OnClick);
    }
    public void RefreshData(Member data)
    {
        this.data = data;
        this.gameObject.name = data.Id.ToString();
        index.text = data.Id.ToString();
        vip_name.text = data.Name;
        gender.text = Tool.GetGenderName(data.Gender);
        point.text = data.Point.ToString();
        birth.text = Tool.DateTimeFormat(data.Birth);
        enter.text = Tool.DateTimeFormat(data.Enter);
    }
    public void SelectState(bool is_select)
    {
        select.SetActive(is_select);
    }
    public void OnClick()
    {
        if (ClickFunc != null)
            ClickFunc(this);
    }
    protected override void OnEnable() { }
    protected override void OnUpdate() { }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}