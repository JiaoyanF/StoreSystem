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
/// 商品信息：增加、修改面板
/// </summary> 
public class GoodsInfoUI : UI
{
    public override UILayer Layer { get { return UILayer.Popup; } }
    InputField index_input;// 编号输入
    InputField name_input;// 名称输入
    InputField price_input;// 单价输入
    InputField stock_input;// 库存输入
    InputField desc_input;// 描述输入
    Dropdown type_drop;// 商品类型选择
    Button Ensure;// 确定按钮
    Button Reset;// 重置按钮
    Goods UpdateData = null;
    protected override void Initialize()
    {
        index_input = GetControl<InputField>(this, "index_input");
        name_input = GetControl<InputField>(this, "name_input");
        price_input = GetControl<InputField>(this, "price_input");
        stock_input = GetControl<InputField>(this, "stock_input");
        desc_input = GetControl<InputField>(this, "desc_input");
        type_drop = GetControl<Dropdown>(this, "type_drop");
        Ensure = GetControl<Button>(this, "Ensure");
        Reset = GetControl<Button>(this, "Reset");
    }
    protected override void RegEvents()
    {
        SetBtnEvent(Get(this, "mask"), delegate () { Close(); });
        SetBtnEvent(Ensure, SureBtnClick);
        SetBtnEvent(Reset, ResetBtnClick);
        RegEventHandler<Events.GoodsEve.Add>(GetResult);
        RegEventHandler<Events.GoodsEve.Update>(GetResult);
    }
    private void SureBtnClick()
    {
        if (index_input.text == "")
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDGOODS_INDEX_TIPS")));
            return;
        }
        if (name_input.text == "")
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDGOODS_NAME_TIPS")));
            return;
        }
        if (price_input.text == "")
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDGOODS_PRICE_TIPS")));
            return;
        }
        if (int.Parse(stock_input.text) < 0)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADDGOODS_STOCK_TIPS")));
            return;
        }
        Goods god = new Goods();
        god.Id = index_input.text;
        god.Name = name_input.text;
        god.Price = double.Parse(price_input.text);
        god.Stock = int.Parse(stock_input.text);
        god.Tips = desc_input.text;
        god.Type = type_drop.value;
        if (UpdateData != null)
        {
            NetMgr.SendMessage(NetTag.Goods.UpdateGoods, god);
            return;
        }
        NetMgr.SendMessage(NetTag.Goods.AddGoods, god);
    }
    // 添加商品结果
    private void GetResult(Obj sender, Events.GoodsEve.Add e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("ADD_WIN")));
        Close();
    }
    private void GetResult(Obj sender, Events.GoodsEve.Update e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        FireEvent(new Events.UI.OpenUI("CommonTips", Localization.Format("UPDATE_WIN")));
        Close();
    }
    private void ResetBtnClick()
    {
        if (UpdateData != null)
        {
            RefreshData();
            return;
        }
        index_input.text = "";
        name_input.text = "";
        price_input.text = "";
        stock_input.text = "";
        desc_input.text = "";
        type_drop.value = 0;
    }
    protected override void OnEnable()
    {
        // 修改数据
        if (context != null && context.Length > 0)
        {
            UpdateData = context[0] as Goods;
            RefreshData();
        }
    }
    private void RefreshData()
    {
        index_input.readOnly = true;
        index_input.text = UpdateData.Id;
        name_input.text = UpdateData.Name;
        price_input.text = UpdateData.Price.ToString();
        stock_input.text = UpdateData.Stock.ToString();
        desc_input.text = UpdateData.Tips;
        type_drop.value = UpdateData.Type;
    }
    protected override void OnUpdate()
    {
    }
    protected override void OnDisable()
    {
        context = null;
        UpdateData = null;
    }
    protected override void OnDestroy()
    {
    }
}