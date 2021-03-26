using Tar;
using LitJson;

/// <summary>
/// 商品类
/// </summary>
public class Goods : BaseData
{
    public float price;// 单价
    public int stock;// 库存
    public int num;// 数量（购买
    public Goods(JsonData json)
    {
        
    }
}