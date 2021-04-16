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
/// 销售记录面板
/// </summary> 
public class SalesRecordUI : UI
{
    public override UILayer Layer { get { return UILayer.Normal; } }
    private Dropdown Screen;// 筛选项
    private InputField Search;// 搜索框
    private GameObject DelBtn;// 删除
    private GameObject Title;// 列表标题
    private RecordItem Item;// 原型
    List<RecordItem> RecordList = new List<RecordItem>();
    RecordItem CurrItem;// 当前选中项(可能之后要加整个清单退货)
    GoodsListPanel ListPanel;// 详细商品购买列表面板
    protected override void Initialize()
    {
        Screen = GetControl<Dropdown>(this, "Screen");
        Search = GetControl<InputField>(this, "Search");
        DelBtn = Get(this, "Delete");
        Title = Get(this, "Title");
        Item = NewElement<RecordItem>(this, Get(this, "RecordItem"));
        ListPanel = NewElement<GoodsListPanel>(this, Get(this, "GoodsList"));
    }
    protected override void RegEvents()
    {
        RegEventHandler<Events.Sales.GetRecord>(RefreshData);
    }
    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RefreshData(Obj sender, Events.Sales.GetRecord e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        ClonRecordItem(e.Data);
    }
    private void ClonRecordItem(List<Record> data)
    {
        RecordList = Item.Clone<RecordItem>(RecordList, data.Count);
        int index = 0;
        foreach (Record item in data)
        {
            RecordList[index].RefreshData(item);
            RecordList[index].MoreClickFunc = MoreClick;
            // RecordList[index].ClickFunc = ClickFunc;
            index++;
        }
        Log.Debug("记录个数：{0}", RecordList.Count);
    }
    /// <summary>
    /// "更多商品"点击事件
    /// </summary>
    private void MoreClick(Record item)
    {
        ListPanel.RefreshShow(item.SalesList);
    }
    protected override void OnEnable()
    {
        NetMgr.SendMessage(NetTag.SalesRecord.GetRecord);
    }
    protected override void OnUpdate() { }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}

/// <summary>
/// 商品清单展示
/// </summary>
public class GoodsListPanel : UIElement
{
    List<GoodsItem> BuyItems;
    GoodsItem Item;
    protected override void Initialize()
    {
        BuyItems = new List<GoodsItem>();
        Item = Root.NewElement<GoodsItem>(Root, Root.Get(this, "GoodsItem"));
    }
    protected override void RegEvents()
    {
        Root.SetBtnEvent(Root.Get(this, "mask"), delegate () { Root.SetActive(this, false); });
    }
    public void RefreshShow(List<Goods> list)
    {
        Root.SetActive(this, true);
        BuyItems = Item.Clone<GoodsItem>(BuyItems, list.Count);
        int index = 0;
        foreach (Goods item in list)
        {
            Log.Debug("商品项：{0}", JsonMapper.ToJson(item));
            BuyItems[index].RefreshData(item);
            BuyItems[index].SetSalesListShow();
            // BuyItems[index].ClickFunc = ClickFunc;
            index++;
        }
    }
    protected override void OnEnable() { }
    protected override void OnUpdate() { }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}