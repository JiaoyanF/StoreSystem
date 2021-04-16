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
/// 收银
/// </summary> 
public class SettleAccountsUI : UI
{
    public override UILayer Layer { get { return UILayer.Normal; } }
    private GameObject Nav;// 导航栏
    private InputField VipInput;// 会员号
    private InputField IdInput;// 输入id
    private InputField NumInput;// 输入数量
    private GameObject Btns;// 按钮组
    private GameObject CurrentInfo;// 当前项信息
    private Text cur_price;// 当前项:单价
    private Text cur_total;// 当前项:小计
    private GameObject TotalInfo;// 总计信息
    private Text tot_count;// 总计:件数
    private Text tot_num;// 总计:品数
    private Text tot_money;// 总计:金额
    private GoodsItem Item;// 商品项
    private SettlementPanel SettlementPanel;
    List<GoodsItem> GoodsList = new List<GoodsItem>();
    GoodsItem CurrItem;// 当前选中商品项
    Member CurrVip;// 当前vip信息
    protected override void Initialize()
    {
        Nav = Get(this, "Nav");
        Btns = Get(Nav, "Btns");
        VipInput = GetControl<InputField>(Nav, "VipInput");
        IdInput = GetControl<InputField>(Nav, "IdInput");
        NumInput = GetControl<InputField>(Nav, "NumInput");
        CurrentInfo = Get(this, "CurrentInfo");
        cur_price = GetControl<Text>(CurrentInfo, "cur_price");
        cur_total = GetControl<Text>(CurrentInfo, "cur_total");
        TotalInfo = Get(this, "TotalInfo");
        tot_count = GetControl<Text>(TotalInfo, "tot_count");
        tot_num = GetControl<Text>(TotalInfo, "tot_num");
        tot_money = GetControl<Text>(TotalInfo, "tot_money");
        Item = NewElement<GoodsItem>(this, Get(this, "GoodsItem"));
        SettlementPanel = NewElement<SettlementPanel>(this, Get(this, "SettlementPanel"));
    }
    protected override void RegEvents()
    {
        VipInput.onEndEdit.AddListener(VipChange);// 值改变事件
        SetBtnEvent(Get(Btns, "Add"), ClickAddBtn);
        SetBtnEvent(Get(Btns, "Delete"), ClickDeleteBtn);
        SetBtnEvent(Get(Btns, "Settlement"), ClickSettlementBtn);
        RegEventHandler<Events.GoodsEve.AddStop>(AddShopingList);
        RegEventHandler<Events.GoodsEve.SureSettlement>(Settlement);
        RegEventHandler<Events.GoodsEve.Settlement>(SettlementResult);
        RegEventHandler<Events.Vip.Get>(VipResult);
    }
    /// <summary>
    /// 输入vip
    /// </summary>
    /// <param name="str"></param>
    private void VipChange(string str)
    {
        if (str == string.Empty)
            return;
        NetMgr.SendMessage(NetTag.Vip.GetData, str);
    }
    /// <summary>
    /// 获取vip结果
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void VipResult(Obj sender, Events.Vip.Get e)
    {
        if (e.Result == false)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        if (e.Data.Count != 1)
            return;
        CurrVip = e.Data[0];
    }
    /// <summary>
    /// 加入购买列表
    /// </summary>
    private void ClickAddBtn()
    {
        if (IdInput.text == string.Empty)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("INPUT_IS_NULL_TIPS")));
            return;
        }
        if (int.Parse(NumInput.text) <= 0)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("BUT_NUM_ERR_TIPS")));
            return;
        }
        JsonData data = new JsonData();
        data["id"] = IdInput.text;
        data["num"] = NumInput.text;
        NetMgr.SendMessage(NetTag.Goods.AddShoping, data);
    }
    /// <summary>
    /// 移除商品
    /// </summary>
    private void ClickDeleteBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_GOODS")));
            return;
        }
        for (int i = 0; i < GoodsList.Count; i++)
        {
            if (GoodsList[i].data.Id == CurrItem.data.Id)
            {
                GoodsList[i].Dispose();
                GoodsList.RemoveAt(i);
                break;
            }
        }
    }
    /// <summary>
    /// 点击结算
    /// </summary>
    private void ClickSettlementBtn()
    {
        if (GoodsList.Count == 0)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("SETTLEMENT_LIST_NULL_TIPS")));
            return;
        }
        SettlementPanel.RefreshData(GoodsList);
    }
    /// <summary>
    /// 商品项点击事件
    /// </summary>
    /// <param name="item"></param>
    private void ClickFunc(GoodsItem item)
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
        RefreshCurrData();
    }
    /// <summary>
    /// 请求结算
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Settlement(Obj sender, Events.GoodsEve.SureSettlement e)
    {
        Record sales_record = new Record();
        sales_record.SalesList = e.Data;
        if (CurrVip != null)
            sales_record.Vip = CurrVip.Id;
        NetMgr.SendMessage(NetTag.Goods.Settlement, sales_record);
    }
    private void AddShopingList(Obj sender, Events.GoodsEve.AddStop e)
    {
        if (e.Result == false)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        // 判断列表里是否已有商品
        foreach (var item in GoodsList)
        {
            if (e.Data.Id == item.data.Id)
            {
                item.data.AddBuyNum(e.Num);
                if (item.data.MeetStock())
                {
                    FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("BUT_STOCK_ERR_TIPS", item.data.Stock.ToString())));
                    return;
                }
                item.RefreshData(item.data);
                return;
            }
        }
        // 添加新商品
        GoodsItem goods_item = Item.Clone<GoodsItem>();
        e.Data.Num = e.Num;
        goods_item.RefreshData(e.Data);
        goods_item.SetCashierListShow();
        goods_item.ClickFunc = ClickFunc;
        GoodsList.Add(goods_item);
        if (CurrItem == null)
            CurrItem = goods_item;
        RefreshCurrData();
        RefreshTotaData();
    }
    /// <summary>
    /// 刷新选中商品数据
    /// </summary>
    private void RefreshCurrData()
    {
        cur_price.text = CurrItem != null ? Localization.Format("GOODS_PRICE_TITLE", CurrItem.data.Price.ToString()) : Localization.Format("GOODS_PRICE_TITLE", "0");
        cur_total.text = CurrItem != null ? Localization.Format("GOODS_SUBTOTAL_TITLE", CurrItem.data.GetTotalMoney().ToString()) : Localization.Format("GOODS_SUBTOTAL_TITLE", "0");
    }
    /// <summary>
    /// 刷新总计数据
    /// </summary>
    private void RefreshTotaData()
    {
        int count = 0;
        double money = 0;
        foreach (var item in GoodsList)
        {
            count += item.data.Num;
            money += item.data.GetTotalMoney();
        }
        tot_count.text = Localization.Format("GOODS_PRICE_TITLE", count.ToString());
        tot_num.text = Localization.Format("GOODS_NUM_TITLE", GoodsList.Count.ToString());
        tot_money.text = Localization.Format("GOODS_MONEY_TITLE", money.ToString());
    }
    /// <summary>
    /// 结账成功
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SettlementResult(Obj sender, Events.GoodsEve.Settlement e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        foreach (var item in GoodsList)
        {
            item.Dispose();
        }
        GoodsList.Clear();
        InitInput();
        FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("SETTLE_WIN")));
    }
    /// <summary>
    /// 初始化输入框
    /// </summary>
    private void InitInput()
    {
        VipInput.text = string.Empty;
        IdInput.text = string.Empty;
        NumInput.text = "1";
    }
    protected override void OnEnable()
    {
        InitInput();
    }
    protected override void OnUpdate() { }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}
/// <summary>
/// 结账面板
/// </summary>
public class SettlementPanel : UIElement
{
    InputField actual_input;// 实收
    Text should_tex;// 应收
    Text change_tex;// 找零
    double should_money;
    double actual;
    double change;
    List<Goods> BuyItems;

    protected override void Initialize()
    {
        actual_input = Root.GetControl<InputField>(this, "actual_input");
        should_tex = Root.GetControl<Text>(this, "should_tex");
        change_tex = Root.GetControl<Text>(this, "change_tex");
    }
    protected override void RegEvents()
    {
        Root.SetBtnEvent(Root.Get(this, "mask"), delegate () { InitData(); Root.SetActive(this, false); });
        Root.SetBtnEvent(Root.Get(this, "Ensure"), SureBtnClick);
        Root.SetBtnEvent(Root.Get(this, "Reset"), InitData);
        actual_input.onValueChanged.AddListener(RefreshShow);
    }
    public void RefreshData(List<GoodsItem> list)
    {
        Root.SetActive(this, true);
        InitData();
        should_money = 0;
        BuyItems = new List<Goods>();
        foreach (GoodsItem item in list)
        {
            should_money += item.data.GetTotalMoney();
            BuyItems.Add(item.data);
        }
        should_tex.text = Localization.Format("SETTLEMENT_SHOULD_TITLE", should_money.ToString());
        change_tex.text = Localization.Format("SETTLEMENT_CHANGE_TITLE", change.ToString());
    }
    /// <summary>
    /// 刷新显示
    /// </summary>
    private void RefreshShow(string s)
    {
        change = actual_input.text != string.Empty ? double.Parse(actual_input.text) - should_money : 0;
        string str = Localization.Format("SETTLEMENT_CHANGE_TITLE", change.ToString());
        if (change < 0)
            str = Localization.Format("SETTLEMENT_SHORTAGE_ERR");
        change_tex.text = str;
    }
    private void SureBtnClick()
    {
        if (change < 0)
        {
            Root.FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("SETTLEMENT_SHORTAGE_ERR")));
            return;
        }
        if (actual_input.text == string.Empty)
        {
            Root.FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("SETTLEMENT_NULL_ERR")));
            return;
        }
        Root.FireEvent(new Events.GoodsEve.SureSettlement(BuyItems));
        InitData();
        Root.SetActive(this, false);
    }
    private void InitData()
    {
        actual_input.text = "";
        change = 0;
    }
    protected override void OnEnable() { }
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SureBtnClick();
        }
    }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}