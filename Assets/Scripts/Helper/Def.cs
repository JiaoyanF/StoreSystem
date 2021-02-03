using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    公共定义类
*/
public struct Def
{

}


#region 系统枚举类型

/// <summary>
/// UI窗体位置类型
/// </summary> 
public enum UIFormType
{
    /// <summary>
    /// 普通窗体
    /// </summary> 
    Normal,
    /// <summary>
    /// 固定窗体
    /// </summary> 
    Fixed,
    /// <summary>
    /// 弹出窗体
    /// </summary> 
    PopUp,
}

/// <summary>
/// ui窗体显示类型
/// </summary>
public enum UIShowType
{
    Normal,
    ReverseChange,
    HideOther,
}

/// <summary>
/// ui窗体透明度类型
/// </summary>
public enum UILucenyType
{
    Lucency,// 完全透明，不能穿透
    Translucence,// 半透明
    ImPenetrable,// 低透明
    Pentrate,// 可以穿透
}

#endregion

/// <summary>
/// 系统定义
/// </summary>
public class SysDefine
{
    public const string PrefabPath = "Prefab/";// 预制体路径
    public const string RawImagePath = "RawImage/";
}


#region 功能枚举类型

/// <summary>
/// 性别
/// </summary> 
public enum Gender
{
    Man,// 男
    Woman,// 女
}

/// <summary>
/// 商品类型
/// </summary> 
public enum MerchandiseType
{
    Commodity,// 日用品
    Drinks,// 酒水
    Fresh,// 生鲜
    Food,// 食品
    Appliance,// 家用电器
    Furniture,// 家具
    Stationery,// 文具、图书
    Clothing,// 服装
}

/// <summary>
/// 操作类型
/// </summary>
public enum OperationType
{
    Add,// 添加
    Updata,// 修改
    Delete,// 删除
}

/// <summary>
/// 员工权限
/// </summary>
public enum StaffRightsType
{
    Admin,// 管理员
    Ordinary,// 普通员工
    Temporary,// 临时工
}

/// <summary>
/// 员工岗位
/// </summary>
public enum StaffPostType
{
    Cashier,// 收银员
    Guide,// 导购
}

/// <summary>
/// 顾客类型
/// </summary>
public enum GuestType
{
    Vip,// 会员
    Other,// 普通
}

public enum Receipt
{

}

#endregion