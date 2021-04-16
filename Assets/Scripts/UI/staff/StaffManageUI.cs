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
/// 员工管理
/// </summary> 
public class StaffManageUI : UI
{
    public override UILayer Layer { get { return UILayer.Normal; } }
    private Dropdown Screen;// 筛选项
    private InputField Search;// 搜索框
    private GameObject List;// 列表
    private StaffItem Item;// 原型
    List<StaffItem> StaffList = new List<StaffItem>();
    StaffItem CurrItem;// 当前选中商品项
    protected override void Initialize()
    {
        Screen = GetControl<Dropdown>(this, "Screen");
        Search = GetControl<InputField>(this, "Search");
        List = Get(this, "List");
        Item = NewElement<StaffItem>(this, Get(List, "StaffItem"));
    }
    protected override void RegEvents()
    {
        Screen.onValueChanged.AddListener(ScreenShow);// 值改变事件
        Search.onValueChanged.AddListener(ScreenShow);// 值改变事件

        SetBtnEvent(Get(this, "Add"), ClickAddBtn);
        SetBtnEvent(Get(this, "Modify"), ClickModBtn);
        SetBtnEvent(Get(this, "Delete"), ClickDelBtn);
        RegEventHandler<Events.User.Get>(RefreshData);
        RegEventHandler<Events.User.Add>(AddData);
        RegEventHandler<Events.User.Update>(UpdateData);
        RegEventHandler<Events.User.Delete>(DeleteData);
    }
    public void ClickAddBtn()
    {
        FireEvent(new Events.UI.OpenUI("StaffInfo"));
    }
    public void ClickModBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_STAFF")));
            return;
        }
        FireEvent(new Events.UI.OpenUI("StaffInfo", CurrItem.data));
    }
    public void ClickDelBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_STAFF")));
            return;
        }
        Map<string, Action> btns = new Map<string, Action>();
        btns.Add("确认", delegate () { NetMgr.SendMessage(NetTag.Staff.DeleteStaff, CurrItem.data.Id); });
        btns.Add("取消", null);
        FireEvent(new Events.UI.OpenUI("CommonPanel", Localization.Format("DELETESTAFF_SURE_TIPS", CurrItem.data.Name), btns));
    }
    // 筛选显示
    void ScreenShow<T>(T args)
    {
        int select_index = Screen.value;
        string input_key = Search.text;
        foreach (StaffItem item in StaffList)
        {
            string compare = string.Empty;
            switch (select_index)
            {
                case 1:
                    compare = item.data.Id;
                    break;
                case 2:
                    compare = item.data.Name;
                    break;
                case 3:
                    compare = Tool.GetPowerName(item.data.Power);
                    break;
                default:
                    compare = item.data.Id + "#" + Tool.GetPowerName(item.data.Power) + "#" + item.data.Name;
                    break;
            }
            if (compare.Contains(input_key) || input_key == string.Empty)
            {
                SetActive(item, true);
            }
            else
            {
                SetActive(item, false);
            }
        }
    }
    /// <summary>
    /// 刷新员工列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void RefreshData(Obj sender, Events.User.Get e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        CloneStaffItem(e.Data);
        Screen.value = 0;
        Search.text = string.Empty;
    }
    /// <summary>
    /// 添加员工
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void AddData(Obj sender, Events.User.Add e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        StaffItem new_item = Item.Clone<StaffItem>();
        new_item.RefreshData(e.NewStaff);
        new_item.ClickFunc = ClickFunc;
        for (int i = 0; i < StaffList.Count; i++)
        {
            if (i + 1 < StaffList.Count && int.Parse(StaffList[i + 1].data.Id) > int.Parse(e.NewStaff.Id))
            {
                new_item.transform.SetSiblingIndex(i + 2);
                StaffList.Insert(i, new_item);
                break;
            }
        }
    }
    /// <summary>
    /// 修改员工
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void UpdateData(Obj sender, Events.User.Update e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        foreach (var item in StaffList)
        {
            if (item.data.Id == e.Data.Id)
            {
                item.RefreshData(e.Data);
                break;
            }
        }
    }
    /// <summary>
    /// 删除员工
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DeleteData(Obj sender, Events.User.Delete e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        for (int i = 0; i < StaffList.Count; i++)
        {
            if (StaffList[i].data.Id == e.Id)
            {
                StaffList[i].Dispose();
                StaffList.RemoveAt(i);
                break;
            }
        }
        FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("STAFF_DELETE_WIN")));
    }
    // 克隆员工项们
    private void CloneStaffItem(List<Staff> data)
    {
        StaffList = Item.Clone<StaffItem>(StaffList, data.Count);
        int index = 0;
        foreach (Staff item in data)
        {
            StaffList[index].RefreshData(item);
            StaffList[index].ClickFunc = ClickFunc;
            index++;
        }
        Log.Debug("员工个数：{0}", StaffList.Count);
    }
    /// <summary>
    /// 员工项点击事件
    /// </summary>
    /// <param name="item"></param>
    private void ClickFunc(StaffItem item)
    {
        if (CurrItem != null)
        {
            CurrItem.SelectState(false);
            // 重复点击
            if (CurrItem.data.Id == item.data.Id)
            {
                CurrItem = null;
                return;
            }
        }
        item.SelectState(true);
        CurrItem = item;
        // Log.Format("当前项：{0}", CurrItem.data.Name);
    }
    protected override void OnEnable()
    {
        NetMgr.SendMessage(NetTag.Staff.GetData);
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