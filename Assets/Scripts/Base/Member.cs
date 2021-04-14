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
    public Member() { }
    public Member(JsonData json)
    {
        this.Tag = Convert.ToInt32(Def.DataList.SalesRecord);
        this.Id = json["id"] != null ? Convert.ToInt32(json["id"].ToString()) : -1;
    }
}