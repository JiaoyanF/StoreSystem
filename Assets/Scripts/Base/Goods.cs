using Tar;
using LitJson;
using System;
using System.Collections.Generic;

/// <summary>
/// 商品类
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