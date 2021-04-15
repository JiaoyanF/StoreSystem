using Tar;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 会员
/// </summary>
public class Member : BaseData
{
    public int Gene;// 性别
    public int Point;// 积分
    public string Birth;// 出生日期
    public string Join;// 入会日期
    public Member() { }
    public Member(JsonData json)
    {
        this.Tag = Convert.ToInt32(Def.DataList.SalesRecord);
        this.Id = json["id"] != null ? Convert.ToInt32(json["id"].ToString()) : 0;
        this.Name = json["name"] != null ? json["name"].ToString() : string.Empty;
        this.Gene = json["gender"] != null ? Convert.ToInt32(json["gender"].ToString()) : 0;
        this.Birth = json["birth"] != null ? json["birth"].ToString() : string.Empty;
        this.Join = json["join"] != null ? json["join"].ToString() : string.Empty;
        this.Point = json["point"] != null ? Convert.ToInt32(json["point"].ToString()) : 0;
    }
}