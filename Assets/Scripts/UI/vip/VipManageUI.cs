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
/// 会员管理
/// </summary> 
public class VipManageUI : UI
{
    public override UILayer Layer { get { return UILayer.Normal; } }
    private Dropdown Screen;// 筛选项
    private InputField Search;// 搜索框
    private GameObject List;// 列表
    private VipItem Item;// 原型
    List<VipItem> VipList = new List<VipItem>();
    VipItem CurrItem;// 当前选中商品项
    protected override void Initialize()
    {
        Screen = GetControl<Dropdown>(this, "Screen");
        Search = GetControl<InputField>(this, "Search");
        List = Get(this, "List");
        Item = NewElement<VipItem>(this, Get(List, "VipItem"));
    }
    protected override void RegEvents()
    {
        Screen.onValueChanged.AddListener(ScreenShow);// 值改变事件
        Search.onValueChanged.AddListener(ScreenShow);// 值改变事件

        SetBtnEvent(Get(this, "Add"), ClickAddBtn);
        SetBtnEvent(Get(this, "Modify"), ClickModBtn);
        SetBtnEvent(Get(this, "Delete"), ClickDelBtn);
        RegEventHandler<Events.Vip.Get>(RefreshData);
        RegEventHandler<Events.Vip.Add>(AddData);
        RegEventHandler<Events.Vip.Update>(UpdateData);
        RegEventHandler<Events.Vip.Delete>(DeleteData);
    }
    public void ClickAddBtn()
    {
        FireEvent(new Events.UI.OpenUI("VipInfo"));
    }
    public void ClickModBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_VIP")));
            return;
        }
        FireEvent(new Events.UI.OpenUI("VipInfo", CurrItem.data));
    }
    public void ClickDelBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_VIP")));
            return;
        }
        Map<string, Action> btns = new Map<string, Action>();
        btns.Add("确认", delegate () { NetMgr.SendMessage(NetTag.Vip.DeleteVip, CurrItem.data.Id); });
        btns.Add("取消", null);
        FireEvent(new Events.UI.OpenUI("CommonPanel", Localization.Format("DELETEVIP_SURE_TIPS", CurrItem.data.Name), btns));
    }
    // 筛选显示
    void ScreenShow<T>(T args)
    {
        int select_index = Screen.value;
        string input_key = Search.text;
        foreach (VipItem item in VipList)
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
                    compare = Tool.GetGenderName(item.data.Gender);
                    break;
                default:
                    compare = item.data.Id + "#" + Tool.GetGenderName(item.data.Gender) + "#" + item.data.Name;
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
    /// 刷新会员列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void RefreshData(Obj sender, Events.Vip.Get e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        CloneVipItem(e.Data);
        Screen.value = 0;
        Search.text = string.Empty;
    }
    /// <summary>
    /// 添加会员响应
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void AddData(Obj sender, Events.Vip.Add e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        VipItem new_item = Item.Clone<VipItem>();
        new_item.RefreshData(e.NewVip);
        new_item.ClickFunc = ClickFunc;
        for (int i = 0; i < VipList.Count; i++)
        {
            if (i + 1 < VipList.Count && string.CompareOrdinal(VipList[i].data.Id, e.NewVip.Id) > 0)
            {
                new_item.transform.SetSiblingIndex(i + 1);
                VipList.Insert(i, new_item);
                break;
            }
        }
    }
    /// <summary>
    /// 修改会员响应
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void UpdateData(Obj sender, Events.Vip.Update e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        foreach (var item in VipList)
        {
            if (item.data.Id == e.Data.Id)
            {
                item.RefreshData(e.Data);
                break;
            }
        }
    }
    /// <summary>
    /// 删除会员响应
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DeleteData(Obj sender, Events.Vip.Delete e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        for (int i = 0; i < VipList.Count; i++)
        {
            if (VipList[i].data.Id == e.Id)
            {
                VipList[i].Dispose();
                VipList.RemoveAt(i);
                break;
            }
        }
        FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("DELETE_WIN")));
    }
    // 克隆会员项们
    private void CloneVipItem(List<Member> data)
    {
        VipList = Item.Clone<VipItem>(VipList, data.Count);
        int index = 0;
        foreach (Member item in data)
        {
            VipList[index].RefreshData(item);
            VipList[index].ClickFunc = ClickFunc;
            index++;
        }
        Log.Debug("会员个数：{0}", VipList.Count);
    }
    /// <summary>
    /// 会员项点击事件
    /// </summary>
    /// <param name="item"></param>
    private void ClickFunc(VipItem item)
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
        NetMgr.SendMessage(NetTag.Vip.GetData);
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