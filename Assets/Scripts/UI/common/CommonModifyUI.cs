using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;
using Tar;
using LitJson;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Reflection;

/// <summary>
/// 公共修改数据面板
/// </summary> 
public class CommonModifyUI : UI
{
    public override UILayer Layer { get { return UILayer.Popup; } }
    Text title;
    Button Ensure;// 确认
    Button Reset;// 重置
    BaseData data;// 传入的数据类
    List<FieldInfo> members = new List<FieldInfo>();
    InputItem InputItem;// 输入
    List<InputItem> InputList = new List<InputItem>();
    List<ShowMember> MemberList = new List<ShowMember>();// 要显示的字段

    protected override void Initialize()
    {
        title = GetControl<Text>(this, "Title");
        Ensure = GetControl<Button>(this, "Ensure");
        Reset = GetControl<Button>(this, "Reset");
        InputItem = NewElement<InputItem>(this, Get(this, "input"));

        MemberList.Add(new ShowMember("Id", "编号", true));
        MemberList.Add(new ShowMember("Name", "名称"));
        MemberList.Add(new ShowMember("Price", "单价"));
        MemberList.Add(new ShowMember("Stock", "库存"));
        MemberList.Add(new ShowMember("Type", "类型", false, 1));
        MemberList.Add(new ShowMember("Desc", "描述"));
    }
    protected override void RegEvents()
    {
        SetBtnEvent(Get(this, "mask"), delegate () { Close(); });
        SetBtnEvent(Ensure, SureBtnClick);
        SetBtnEvent(Reset, ResetData);
    }
    private void SureBtnClick()
    {
        // NetMgr.SendMessage(NetTag.Goods.AddGoods, god);
    }
    protected override void OnEnable()
    {
    }
    public void SetClass<T>(T module) where T : BaseData
    {
        data = module;
        title.text = Localization.Format("MODIFYGOODS_SURE_TITLE", data.Id.ToString());
        ResetData();
    }
    // 重置数据
    private void ResetData()
    {
        members = Tool.GetField(data);
        foreach (FieldInfo item in members)
        {
            Log.Debug("字段:{0}，值：{1}", item.Name, item.GetValue(data));
            ShowMember show_mem = null;
            foreach (ShowMember mem in MemberList)
            {
                if (mem.id == item.Name)
                {
                    show_mem = mem;
                    break;
                }
            }
            if (show_mem == null) return;
            InputItem input_clone = InputItem.Clone<InputItem>();
            input_clone.gameObject.name = item.Name;
            input_clone.RefreshData(show_mem.name, item.GetValue(data).ToString(), show_mem.read_only);
            InputList.Add(input_clone);
        }
    }
    protected override void OnUpdate()
    {
    }
    protected override void OnDisable()
    {
    }
    protected override void OnDestroy()
    {
    }
}

class ShowMember
{
    public string id { get; }
    public string name { get; }
    public bool read_only { get; }
    public int control_type { get; }
    public ShowMember(string id, string name, bool read_only = false, int control_type = 0)
    {
        this.id = id;
        this.name = name;
        this.read_only = read_only;
        this.control_type = control_type;
    }
}

class InputItem : UIElement
{
    Text desc;
    InputField tex;
    protected override void Initialize()
    {
        desc = Root.GetControl<Text>(this, "des");
        tex = Root.GetControl<InputField>(this, "tex");
    }
    protected override void RegEvents()
    {
    }
    public void RefreshData(string des, string value, bool is_read_only = false)
    {
        Log.Debug("设置：{0}", des);
        desc.text = des;
        tex.text = value;
        tex.readOnly = is_read_only;
    }
    protected override void OnEnable()
    {
    }
    protected override void OnUpdate()
    {
    }
    protected override void OnDisable()
    {
    }
    protected override void OnDestroy()
    {
    }
}