using Tar;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 商品数据类
/// </summary>
public class Goods : BaseData
{
    public double Price;// 单价
    public int Stock;// 库存
    public string Tips;// 描述
    public int Type;// 商品类型
    public int Num;// 购买数量
    public double Subtotal { get; private set; }// 金额小计
    public Goods() { }
    public Goods(JsonData json)
    {
        this.Tag = Convert.ToInt32(Def.DataList.Goods);
        this.Id = json["id"] != null ? Convert.ToInt32(json["id"].ToString()) : -1;
        this.Name = json["name"] != null ? json["name"].ToString() : string.Empty;
        this.Price = json["price"] != null ? double.Parse(json["price"].ToString()) : -1f;
        this.Stock = json["type"] != null ? Convert.ToInt32(json["stock"].ToString()) : -1;
        this.Type = json["type"] != null ? Convert.ToInt32(json["type"].ToString()) : -1;
        this.Tips = json["tips"] != null ? json["tips"].ToString() : string.Empty;
    }
    /// <summary>
    /// 获取商品类型的名称
    /// </summary>
    public string GetTypeName()
    {
        return Def.GoodsType.Convert(Type);
    }
    /// <summary>
    /// 判断类型是否为此商品类型
    /// </summary>
    public bool IsTypeIndex(string name)
    {
        return Def.GoodsType.Convert(name) == Type;
    }
    // 更新数据
    public void Update(Goods new_data)
    {
        Log.Debug("修改数据");
        UpdatePrice(new_data.Price);
        UpdateStock(new_data.Stock);
    }
    // 设置购买数量
    public void SetBuyNum(int num)
    {
        this.Num = num > 0 ? num : 0;
        Subtotal = Price * Num;
    }
    /// <summary>
    /// 获取小计金额
    /// </summary>
    /// <returns></returns>
    public double GetTotalMoney()
    {
        return this.Num * this.Price;
    }
    // 添加库存
    public void AddStock(int num)
    {
        Log.Debug("添加库存：{0}", num);
    }
    // 修改单价
    public void UpdatePrice(double money)
    {
        if (Price == money)
            return;
        Log.Debug("修改单价：{0}", money);
    }
    public void UpdateStock(int num)
    {
        Log.Debug("修改库存：{0}", num);
    }

    protected override void Delete()
    {

    }
}

/// <summary>
/// 商品列表项
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
    Text num;
    Text total;
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
        num = Root.GetControl<Text>(this, "num");
        total = Root.GetControl<Text>(this, "total");
        // 信息列表
        SetTypeShow(true);
        SetStockShow(true);
        SetDescShow(true);
        // 收银列表
        SetNumShow(true);
        SetTotalShow(true);
    }
    protected override void RegEvents()
    {
        Root.SetBtnEvent(this.gameObject, OnClick);
    }
    public void RefreshData(Goods data)
    {
        this.data = data;
        this.gameObject.name = data.Id.ToString();
        index.text = data.Id.ToString();
        goods_name.text = data.Name;
        type.text = data.GetTypeName();
        price.text = data.Price.ToString();
        stock.text = data.Stock.ToString();
        desc.text = data.Tips.ToString();
        num.text = data.Num.ToString();
        total.text = data.GetTotalMoney().ToString();
    }
    /// <summary>
    /// 类型参数显示状态
    /// </summary>
    public void SetTypeShow(bool show)
    {
        Root.SetActive(type, show);
    }
    /// <summary>
    /// 库存参数显示状态
    /// </summary>
    public void SetStockShow(bool show)
    {
        Root.SetActive(stock, show);
    }
    /// <summary>
    /// 设置描述参数显示状态
    /// </summary>
    public void SetDescShow(bool show)
    {
        Root.SetActive(desc, show);
    }
    /// <summary>
    /// 设置购买次数参数显示状态
    /// </summary>
    public void SetNumShow(bool show)
    {
        Root.SetActive(num, show);
    }
    /// <summary>
    /// 设置小计参数显示状态
    /// </summary>
    public void SetTotalShow(bool show)
    {
        Root.SetActive(total, show);
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