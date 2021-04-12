using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 收银
/// </summary> 
public class SettleAccountsUI : UI
{
    public override UILayer Layer { get { return UILayer.Normal; } }
    private GameObject Nav;// 导航栏
    private InputField InputField;// 输入
    private GameObject Btns;// 按钮组
    private GameObject CurrentInfo;// 当前项信息
    private Text cur_price;// 当前项:单价
    private Text cur_total;// 当前项:小计
    private GameObject TotalInfo;// 总计信息
    private Text tot_count;// 总计:件数
    private Text tot_num;// 总计:品数
    private Text tot_money;// 总计:金额
    private GoodsItem Item;// 商品项
    List<GoodsItem> GoodsList = new List<GoodsItem>();
    GoodsItem CurrItem;// 当前选中商品项
    protected override void Initialize()
    {
        Nav = Get(this, "Nav");
        Btns = Get(Nav, "Btns");
        InputField = GetControl<InputField>(Nav, "InputField");
        CurrentInfo = Get(this, "CurrentInfo");
        cur_price = GetControl<Text>(CurrentInfo, "cur_price");
        cur_total = GetControl<Text>(CurrentInfo, "cur_total");
        TotalInfo = Get(this, "TotalInfo");
        tot_count = GetControl<Text>(TotalInfo, "tot_count");
        tot_num = GetControl<Text>(TotalInfo, "tot_num");
        tot_money = GetControl<Text>(TotalInfo, "tot_money");
        Item = NewElement<GoodsItem>(this, Get(this, "GoodsItem"));
    }
    protected override void RegEvents()
    {
        SetBtnEvent(Get(Btns, "Add"), ClickAddBtn);
        SetBtnEvent(Get(Btns, "Delete"), ClickDeleteBtn);
        SetBtnEvent(Get(Btns, "Settlement"), ClickSettlementBtn);
    }
    private void ClickAddBtn()
    {
        if (InputField.text == string.Empty)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("INPUT_IS_NULL_TIPS")));
            return;
        }
        NetMgr.SendMessage(NetTag.Goods.GetData, InputField.text);
    }
    private void ClickDeleteBtn()
    {
        if (CurrItem == null)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UNCHECKED_GOODS")));
            return;
        }
    }
    private void ClickSettlementBtn()
    {
        if (GoodsList.Count == 0)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("SETTLEMENT_LIST_NULL_TIPS")));
            return;
        }
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