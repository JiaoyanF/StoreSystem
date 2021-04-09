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
/// 商品管理
/// </summary> 
public class GoodsManageUI : UI
{
    public override UILayer Layer { get { return UILayer.Normal; } }
    private GameObject AddBtn;// 添加
    private GameObject ModBtn;// 修改
    private GameObject DelBtn;// 删除
    private GameObject Title;// 列表标题
    private GameObject List;// 列表
    private GoodsItem Item;// 商品项原型
    List<GoodsItem> GoodsList = new List<GoodsItem>();
    GoodsItem CurrItem;// 当前选中商品项
    protected override void Initialize()
    {
        AddBtn = Get(this, "Add");
        ModBtn = Get(this, "Modify");
        DelBtn = Get(this, "Delete");
        Title = Get(this, "Title");
        List = Get(this, "List");
        Item = NewElement<GoodsItem>(this, Get(List, "Item"));
    }
    protected override void RegEvents()
    {
        SetBtnEvent(AddBtn, ClickAddBtn);
        SetBtnEvent(ModBtn, ClickModBtn);
        SetBtnEvent(DelBtn, ClickDelBtn);
        NetMgr.RegEvent(NetTag.Goods.GetData, RefreshGoodsList);
    }
    public void ClickAddBtn()
    {
        Log.Debug("添加商品");
    }
    public void ClickModBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_GOODS")));
            return;
        }

    }
    public void ClickDelBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_GOODS")));
            return;
        }

    }
    void RefreshGoodsList(JsonData con)
    {
        JsonData data = con["data"];
        foreach (JsonData item in data)
        {
            GoodsItem goods_module = Item.Clone<GoodsItem>();
            goods_module.ClickFunc = ClickFunc;
            goods_module.RefreshData(new Goods(item));
            GoodsList.Add(goods_module);
        }
        Log.Format("商品个数：{0}", GoodsList.Count);
    }
    /// <summary>
    /// 商品项点击事件
    /// </summary>
    /// <param name="item"></param>
    public void ClickFunc(GoodsItem item)
    {
        if (CurrItem != null)
        {
            if (CurrItem.data.Id == item.data.Id) return;// 重复点击
            CurrItem.SelectState(false);
        }
        item.SelectState(true);
        CurrItem = item;
        Log.Format("当前项：{0}", CurrItem.data.Name);
    }
    protected override void OnEnable()
    {
        NetMgr.SendMessage(NetTag.Goods.GetData);
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
/// <summary>
/// 商品项
/// </summary>
public class GoodsItem : UIElement
{
    GameObject select;// 选中高亮
    Text index;
    Text goods_name;
    Text price;
    Text stock;
    Text desc;
    public Goods data;// 数据
    public delegate void OnItemClick(GoodsItem item);
    public OnItemClick ClickFunc;
    protected override void Initialize()
    {
        select = Root.Get(this, "select");
        index = Root.GetControl<Text>(Root.Get(this, "idx"));
        goods_name = Root.GetControl<Text>(Root.Get(this, "name"));
        price = Root.GetControl<Text>(Root.Get(this, "price"));
        stock = Root.GetControl<Text>(Root.Get(this, "stock"));
        desc = Root.GetControl<Text>(Root.Get(this, "desc"));
    }
    protected override void RegEvents()
    {
        Root.SetBtnEvent(this.gameObject, OnClick);
        // Root.NetMgr.RegEvent(NetTag.Login.LoginVerify, LoginResult);
    }
    public void RefreshData(Goods data)
    {
        this.data = data;
        Root.SetText(index, data.Id);
        Root.SetText(goods_name, data.Name);
        Root.SetText(price, data.Price);
        Root.SetText(stock, data.Stock);
        Root.SetText(desc, data.Desc);
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