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
        Item = NewElement<GoodsItem>(this, Get(List, "GoodsItem"));
    }
    protected override void RegEvents()
    {
        // Search.onEndEdit.AddListener(ScreenShow);
        Screen.onValueChanged.AddListener(ScreenShow);// 值改变事件
        Search.onValueChanged.AddListener(ScreenShow);// 值改变事件
        
        SetBtnEvent(AddBtn, ClickAddBtn);
        SetBtnEvent(ModBtn, ClickModBtn);
        SetBtnEvent(DelBtn, ClickDelBtn);
        RegEventHandler<Events.GoodsEve.Get>(RefreshData);
        RegEventHandler<Events.GoodsEve.Add>(AddData);
        RegEventHandler<Events.GoodsEve.Update>(UpdateData);
        RegEventHandler<Events.GoodsEve.Delete>(DeleteData);
    }
    public void ClickAddBtn()
    {
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
    /// <summary>
    /// 刷新商品列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void RefreshData(Obj sender, Events.GoodsEve.Get e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        CloneGoodsItem(e.Data);
        Screen.value = 0;
        Search.text = string.Empty;
    }
    /// <summary>
    /// 添加商品
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void AddData(Obj sender, Events.GoodsEve.Add e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        GoodsItem new_item = Item.Clone<GoodsItem>();
        new_item.RefreshData(e.NewGoods);
        new_item.SetInfoListShow();
        new_item.ClickFunc = ClickFunc;
        for (int i = 0; i < GoodsList.Count; i++)
        {
            if (i + 1 < GoodsList.Count && GoodsList[i + 1].data.Id > e.NewGoods.Id)
            {
                new_item.transform.SetSiblingIndex(i + 2);// 位置索引+2是因为还有一个"初号机"的位置要算上_(:3」∠)_
                GoodsList.Insert(i, new_item);
                break;
            }
        }
    }
    /// <summary>
    /// 修改商品信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void UpdateData(Obj sender, Events.GoodsEve.Update e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        foreach (var item in GoodsList)
        {
            if (item.data.Id == e.NewGoods.Id)
            {
                item.RefreshData(e.NewGoods);
                break;
            }
        }
    }
    /// <summary>
    /// 删除商品
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DeleteData(Obj sender, Events.GoodsEve.Delete e)
    {
        if (!e.Result)
        {
            FireEvent(new Events.UI.OpenUI("CommonTips", e.Reason));
            return;
        }
        for (int i = 0; i < GoodsList.Count; i++)
        {
            if (GoodsList[i].data.Id == e.Id)
            {
                GoodsList[i].Dispose();
                GoodsList.RemoveAt(i);
                break;
            }
        }
    }
    // 克隆商品项们
    private void CloneGoodsItem(List<Goods> data)
    {
        GoodsList = Item.Clone<GoodsItem>(GoodsList, data.Count);
        int index = 0;
        foreach (Goods item in data)
        {
            GoodsList[index].RefreshData(item);
            GoodsList[index].SetInfoListShow();
            GoodsList[index].ClickFunc = ClickFunc;
            // Log.Debug("{0}顺序:{1}",GoodsList[index].name,GoodsList[index].transform.GetSiblingIndex());
            index++;
        }
        Log.Debug("商品个数：{0}", GoodsList.Count);
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