using Tar;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 销售清单
/// </summary>
public class SalesRecord : BaseData
{
    public double Money;
    public Staff Staff;
    public Member Member;
    public List<Goods> SalesList;// 销售清单
    public SalesRecord() { }
    public SalesRecord(JsonData json)
    {
        SalesList = new List<Goods>();
        this.Tag = Convert.ToInt32(Def.DataList.SalesRecord);
        this.Id = json["id"] != null ? Convert.ToInt32(json["id"].ToString()) : -1;
        this.Time = json["day"] != null ? json["day"].ToString() : string.Empty;
        // 时间戳转日期
        // long unixTimeStamp = 1478162177;
        // System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        // DateTime dt = startTime.AddSeconds(unixTimeStamp);
        // System.Console.WriteLine(dt.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
    }
    public SalesRecord(List<Goods> list)
    {
        SalesList = list;
    }
}

/// <summary>
/// 销售项
/// </summary>
public struct Sales
{
    public int id;
    public int num;
    public Sales(int id, int num)
    {
        this.id = id;
        this.num = num;
    }
}