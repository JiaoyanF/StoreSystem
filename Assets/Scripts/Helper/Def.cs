using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Def
{
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
        public const string LinkNoneTips = "提示：Link为空！";
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
    public enum GoodsType
    {
        Snacks,// 零食
        Drinks,// 酒水
        Commodity,// 日用品
        Other,// 其他
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
    public enum PowerType
    {
        Admin,// 管理员
        Ordinary,// 普通员工
        Temporary,// 临时工
    }

    /// <summary>
    /// 员工岗位
    /// </summary>
    public enum PostType
    {
        Cashier,// 收银员
        Tally,// 理货员
        Storage,// 仓库管理员
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

}