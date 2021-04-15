using Tar;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// 记录
/// </summary>
public class Record : BaseData
{
    public double Money;
    public int Staff;
    public int Vip;
    public List<Goods> SalesList;// 销售清单
    public Record() { }
    public Record(JsonData json)
    {
        Log.Debug("商品清单：{0}", json["goods"].ToJson());
        List<string> items = new List<string>();
        SalesList = new List<Goods>();
        if (json["goods"].ToJson().Contains(";"))
        {
            items = new List<string>(Regex.Split(json["goods"].ToJson(), ";"));
        }
        else
        {
            items.Add(json["goods"].ToJson());
        }
        foreach (string item in items)
        {
            SalesList.Add(new Goods(item));
        }
        this.Tag = Convert.ToInt32(Def.DataList.SalesRecord);
        this.Id = json["id"] != null ? Convert.ToInt32(json["id"].ToString()) : -1;
        this.Time = json["time"] != null ? json["time"].ToString() : string.Empty;
        this.Money = json["money"] != null ? double.Parse(json["money"].ToString()) : -1f;
        this.Staff = json["staff"] != null ? Convert.ToInt32(json["staff"].ToString()) : -1;
        this.Vip = json["vip"] != null ? Convert.ToInt32(json["vip"].ToString()) : -1;
    }
}

/// <summary>
/// 记录列表项
/// </summary>
public class RecordItem : UIElement
{
    GameObject select;// 选中高亮
    Text index;
    Text time;
    Text goods;
    Text money;
    Text vip;
    Text staff;
    public Record data;// 数据
    public delegate void OnItemClick(RecordItem item);
    public delegate void OnMoreClick(Record data);
    public OnItemClick ClickFunc;
    public OnMoreClick MoreClickFunc;
    protected override void Initialize()
    {
        select = Root.Get(this, "select");
        index = Root.GetControl<Text>(this, "idx");
        time = Root.GetControl<Text>(this, "time");
        goods = Root.GetControl<Text>(this, "goods");
        money = Root.GetControl<Text>(this, "money");
        vip = Root.GetControl<Text>(this, "vip");
        staff = Root.GetControl<Text>(this, "staff");
    }
    protected override void RegEvents()
    {
        Root.SetBtnEvent(this.gameObject, OnClick);
        Root.SetBtnEvent(goods.gameObject, MoreClick);
    }
    private void OnClick()
    {
        if (ClickFunc != null)
            ClickFunc(this);
    }
    private void MoreClick()
    {
        if (MoreClickFunc != null)
            MoreClickFunc(this.data);
    }
    public void RefreshData(Record data)
    {
        this.data = data;
        this.gameObject.name = data.Id.ToString();
        index.text = data.Id.ToString();
        // 时间戳转日期
        DateTime dt = Tool.ConvertStringToDateTime(data.Time);
        time.text = dt.ToString("yyyy/MM/dd HH:mm:ss:ffff");
        goods.text = data.SalesList[0].Name + "...";
        money.text = data.Money.ToString();
        vip.text = data.Vip.ToString();
        staff.text = data.Staff.ToString();
    }
    /// <summary>
    /// 设置选中效果
    /// </summary>
    /// <param name="is_select"></param>
    public void SelectState(bool is_select)
    {
        select.SetActive(is_select);
    }
    protected override void OnEnable() { }
    protected override void OnUpdate() { }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}