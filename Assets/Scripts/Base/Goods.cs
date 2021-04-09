using Tar;
using LitJson;
using System;

/// <summary>
/// 商品类
/// </summary>
public class Goods : BaseData
{
    public float Price;// 单价
    public int Stock;// 库存
    public string Desc;// 描述
    public int Type;// 商品类型
    public int Num;// 购买数量
    public float Subtotal { get; private set; }// 金额小计
    public Goods(JsonData json)
    {
        this.Tag = Convert.ToInt32(Def.DataList.Goods);
        this.Id = json["id"] != null ? Convert.ToInt32(json["id"].ToString()) : -1;
        this.Name = json["name"] != null ? json["name"].ToString() : string.Empty;
        this.Price = json["price"] != null ? float.Parse(json["price"].ToString()) : -1f;
        this.Stock = json["type"] != null ? Convert.ToInt32(json["stock"].ToString()) : -1;
        this.Type = json["type"] != null ? Convert.ToInt32(json["type"].ToString()) : -1;
        this.Desc = json["desc"] != null ? json["desc"].ToString() : string.Empty;
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
    public void UpdatePrice(float money)
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