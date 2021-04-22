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
        Popup,
        /// <summary>
        /// 提示
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
        public const string language = "Assets/StreamingAssets/lang.txt";
        public const string LinkNoneTips = "提示：Link为空！";
    }

    #endregion

    #region 数据库

    public enum DataList
    {
        Goods,
        Staff,
        Vip,
        SalesRecord,
        StaffRecord,
        VipRecord,
    }

    #endregion

    #region 功能枚举类型

    /// <summary>
    /// 性别
    /// </summary> 
    public class GenderType
    {
        public const string Man = "男";
        public const string Woman = "女";
        public static string Convert(int index)
        {
            switch (index)
            {
                case 0:
                    return Man;
                case 1:
                    return Woman;
                default:
                    return string.Empty;
            }
        }
        public static int Convert(string name)
        {
            switch (name)
            {
                case Man:
                    return 0;
                case Woman:
                    return 1;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// 商品类型
    /// </summary> 
    public class GoodsType
    {
        public const string Other = "其他";
        public const string Snacks = "零食";
        public const string Drinks = "酒水";
        public const string Commodity = "日用品";
        public static string Convert(int index)
        {
            switch (index)
            {
                case 0:
                    return Other;
                case 1:
                    return Snacks;
                case 2:
                    return Drinks;
                case 3:
                    return Commodity;
                default:
                    return string.Empty;
            }
        }
        public static int Convert(string name)
        {
            switch (name)
            {
                case Other:
                    return 0;
                case Snacks:
                    return 1;
                case Drinks:
                    return 2;
                case Commodity:
                    return 3;
                default:
                    return 0;
            }
        }
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
    public class PowerType
    {
        public const string Formal = "正式工";
        public const string Admin = "管理员";
        public static string Convert(int index)
        {
            switch (index)
            {
                case 0:
                    return Formal;
                case 1:
                    return Admin;
                default:
                    return string.Empty;
            }
        }
        public static int Convert(string name)
        {
            switch (name)
            {
                case Formal:
                    return 0;
                case Admin:
                    return 1;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// 顾客类型
    /// </summary>
    public enum GuestType
    {
        Vip,// 会员
        Other,// 普通
    }

    #endregion

}