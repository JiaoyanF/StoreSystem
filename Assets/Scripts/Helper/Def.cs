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
/// UI层类型
/// </summary> 
public enum UILayer
{
    /// <summary>
    /// 普通窗体
    /// </summary> 
    Normal,
    /// <summary>
    /// 全屏窗体
    /// </summary> 
    Full,
    /// <summary>
    /// 弹出窗体
    /// </summary> 
    Tips,
}

/// <summary>
/// 系统定义
/// </summary>
public class SysDefine
{
    public const string PrefabPath = "Prefab/";// 预制体路径
    public const string RawImagePath = "RawImage/";
    public const string language = "Assets/Resources/Other/lang.txt";
}

#endregion

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