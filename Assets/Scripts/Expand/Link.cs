using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[AddComponentMenu("Expand/Link")]// 将脚本添加到Component菜单
[HelpURL("https://www.baidu.com")]// 脚本帮助按钮的跳转
public class Link : MonoBehaviour
{
    // [Header("标题")]// 添加标题
    // [Multiline(5)]// 添加string多行输入
    // [Range(0, 20)]// 添加滑块
    // [Tooltip("这是一个悬停提示")]// 鼠标悬停提示
    // [Space(30)]// 添加指定距离

    public GameObject Root;// 根节点
    public List<LinkItem> Links = new List<LinkItem>();// 链接列表
    public int num = 0;// link个数
    public bool show_list = true;// 展开link主菜单
}

public class LinkItem
{
    public bool show = false;
    public string Name;
    public GameObject LinkObj;
}