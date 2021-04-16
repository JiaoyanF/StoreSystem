using Tar;
using Def;
using System;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 使用者：员工
/// </summary>
public class Staff : BaseData
{
    public int Power;// 权限
    public int Gender;// 性别
    public string Tel;// 电话
    public string Password;
    public string Birth;
    public string Enter;
    public Staff() { }
    public Staff(JsonData json)
    {
        this.Tag = Convert.ToInt32(Def.DataList.Staff);
        this.Id = json["id"] != null ? json["id"].ToString() : string.Empty;
        this.Name = json["name"] != null ? json["name"].ToString() : string.Empty;
        this.Password = json["password"] != null ? json["password"].ToString() : string.Empty;
        this.Power = json["power"] != null ? Convert.ToInt32(json["power"].ToString()) : 0;
        this.Gender = json["gender"] != null ? Convert.ToInt32(json["gender"].ToString()) : 0;
        this.Tel = json["tel"] != null ? json["tel"].ToString() : string.Empty;
        this.Birth = json["birth"] != null ? json["birth"].ToString() : string.Empty;
        this.Enter = json["enter"] != null ? json["enter"].ToString() : string.Empty;
    }
}

/// <summary>
/// 员工列表项
/// </summary>
public class StaffItem : UIElement
{
    GameObject select;// 选中高亮
    Text index;
    Text staff_name;
    Text tel;
    Text gender;
    Text power;
    Text birth;
    Text enter;
    public Staff data;// 数据
    public delegate void OnItemClick(StaffItem item);
    public OnItemClick ClickFunc;
    protected override void Initialize()
    {
        select = Root.Get(this, "select");
        index = Root.GetControl<Text>(this, "index");
        staff_name = Root.GetControl<Text>(this, "name");
        tel = Root.GetControl<Text>(this, "tel");
        gender = Root.GetControl<Text>(this, "gender");
        power = Root.GetControl<Text>(this, "power");
        birth = Root.GetControl<Text>(this, "birth");
        enter = Root.GetControl<Text>(this, "enter");
    }
    protected override void RegEvents()
    {
        Root.SetBtnEvent(this.gameObject, OnClick);
    }
    public void RefreshData(Staff data)
    {
        this.data = data;
        this.gameObject.name = data.Id.ToString();
        index.text = data.Id.ToString();
        staff_name.text = data.Name;
        tel.text = data.Tel;
        power.text = Tool.GetPowerName(data.Power);
        gender.text = Tool.GetGenderName(data.Gender);
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