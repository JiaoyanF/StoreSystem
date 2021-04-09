using Tar;
using Def;
using System;
using LitJson;

/// <summary>
/// 使用者
/// </summary>
public class Staff : BaseData
{
    public int Power;// 权限
    public int Post;// 岗位
    public int Gene;// 性别
    public Staff(JsonData json)
    {
        this.Tag = Convert.ToInt32(Def.DataList.Staff);
        this.Id = Convert.ToInt32(json["id"].ToString());
        this.Name = json["name"].ToString();
        this.Power = Convert.ToInt32(json["power"].ToString());
        this.Post = Convert.ToInt32(json["post"].ToString());
        this.Gene = Convert.ToInt32(json["gender"].ToString());
    }
}