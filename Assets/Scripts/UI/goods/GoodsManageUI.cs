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
    private Dropdown Screen;// 筛选项
    private InputField Search;// 搜索框
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
        Screen = GetControl<Dropdown>(this, "Screen");
        Search = GetControl<InputField>(this, "Search");
        AddBtn = Get(this, "Add");
        ModBtn = Get(this, "Modify");
        DelBtn = Get(this, "Delete");
        Title = Get(this, "Title");
        List = Get(this, "List");
        Item = NewElement<GoodsItem>(this, Get(List, "Item"));
    }
    protected override void RegEvents()
    {
        // Search.onEndEdit.AddListener(ScreenShow);
        Screen.onValueChanged.AddListener(ScreenShow);// 值改变事件
        Search.onValueChanged.AddListener(ScreenShow);// 值改变事件
        
        SetBtnEvent(AddBtn, ClickAddBtn);
        SetBtnEvent(ModBtn, ClickModBtn);
        SetBtnEvent(DelBtn, ClickDelBtn);
        NetMgr.RegEvent(NetTag.Goods.GetData, RefreshGoodsList);
        NetMgr.RegEvent(NetTag.Goods.DeleteGoods, RefreshGoodsList);
    }
    public void ClickAddBtn()
    {
        Log.Debug("添加商品");
        FireEvent(new Events.UI.OpenUI("GoodsInfo"));
    }
    public void ClickModBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_GOODS")));
            return;
        }
        FireEvent(new Events.UI.OpenUI("GoodsInfo", CurrItem.data));
        // FireEvent(new Events.UI.OpenUI("CommonModify", CurrItem.data));
        // CommonModifyUI ui = ui_mgr.GetUI<CommonModifyUI>("CommonModify");
        // ui.SetClass(CurrItem.data);
    }
    public void ClickDelBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_GOODS")));
            return;
        }
        Map<string, Action> btns = new Map<string, Action>();
        btns.Add("确认", delegate () { NetMgr.SendMessage(NetTag.Goods.DeleteGoods, CurrItem.data.Id.ToString()); });
        btns.Add("取消", null);
        FireEvent(new Events.UI.OpenUI("CommonPanel", Localization.Format("DELETEGOODS_SURE_TIPS", CurrItem.data.Name), btns));
    }
    // 筛选显示
    void ScreenShow<T>(T args)
    {
        // args.GetType() == typeof(string)
        int select_index = Screen.value;
        string input_key = Search.text;
        foreach (GoodsItem item in GoodsList)
        {
            string compare = string.Empty;
            switch (select_index)
            {
                case 1:
                    compare = item.data.Id.ToString();
                    break;
                case 2:
                    compare = item.data.GetTypeName();
                    break;
                case 3:
                    compare = item.data.Name;
                    break;
                default:
                    compare = item.data.Id.ToString() + "#" + item.data.GetTypeName() + "#" + item.data.Name;
                    break;
            }
            if (compare.Contains(input_key) || input_key == string.Empty)
            {
                SetActive(item, true);
            }else
            {
                SetActive(item, false);
            }
        }
    }
    // 刷新商品列表
    void RefreshGoodsList(JsonData con)
    {
        if (Tool.ContainsKey(con, "result") && Convert.ToBoolean(con["result"].ToString()) == false)
        {
            if (Tool.ContainsKey(con, "reason"))
                FireEvent(new Events.UI.OpenUI("CommonTips", con["reason"].ToString()));
            return;
        }

        if (Tool.ContainsKey(con, "data"))
            CloneGoodsItem(con["data"]);
        
        Screen.value = 0;
        Search.text = string.Empty;
    }
    // 克隆商品项们
    private void CloneGoodsItem(JsonData data)
    {
        GoodsList = Item.Clone<GoodsItem>(GoodsList, Tool.GetJsonCount(data));
        int index = 0;
        foreach (JsonData item in data)
        {
            GoodsList[index].RefreshData(new Goods(item));
            GoodsList[index].ClickFunc = ClickFunc;
            index++;
        }
        // foreach (JsonData item in data)
        // {
        //     GoodsItem goods_module = Item.Clone<GoodsItem>();
        //     goods_module.ClickFunc = ClickFunc;
        //     goods_module.RefreshData(new Goods(item));
        //     GoodsList.Add(goods_module);
        // }
        Log.Debug("商品个数：{0}", GoodsList.Count);
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
        // Log.Format("当前项：{0}", CurrItem.data.Name);
        // Tool.GetMembers(item.data);
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
    Text type;
    Text price;
    Text stock;
    Text desc;
    public Goods data;// 数据
    public delegate void OnItemClick(GoodsItem item);
    public OnItemClick ClickFunc;
    protected override void Initialize()
    {
        select = Root.Get(this, "select");
        index = Root.GetControl<Text>(this, "idx");
        goods_name = Root.GetControl<Text>(this, "name");
        type = Root.GetControl<Text>(this, "type");
        price = Root.GetControl<Text>(this, "price");
        stock = Root.GetControl<Text>(this, "stock");
        desc = Root.GetControl<Text>(this, "desc");
    }
    protected override void RegEvents()
    {
        Root.SetBtnEvent(this.gameObject, OnClick);
        // Root.NetMgr.RegEvent(NetTag.Login.LoginVerify, LoginResult);
    }
    public void RefreshData(Goods data)
    {
        this.data = data;
        this.gameObject.name = data.Id.ToString();
        Root.SetText(index, data.Id);
        Root.SetText(goods_name, data.Name);
        Root.SetText(type, data.GetTypeName());
        Root.SetText(price, data.Price);
        Root.SetText(stock, data.Stock);
        Root.SetText(desc, data.Tips);
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