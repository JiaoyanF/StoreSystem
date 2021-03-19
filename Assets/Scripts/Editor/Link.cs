using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Def;

[CustomEditor(typeof(Link))]// 应用于哪个脚本的界面
public class LinkEditor : Editor
{
    // 脚本对象
    Link link;
    // bool doOnce;
    // int countTemp;
    // 要控制的脚本参数
    SerializedProperty Root, Links, num, show_list;

    void OnEnable()
    {
        link = target as Link;// 获取编辑自定义的对象
        Root = serializedObject.FindProperty("Root");
        Links = serializedObject.FindProperty("Links");
        num = serializedObject.FindProperty("num");
        show_list = serializedObject.FindProperty("show_list");
    }
    /// <summary>
    /// 自定义检视面板
    /// </summary>
    public override void OnInspectorGUI()
    {
        // GUI.changed = false;
        // if (link.Links != null)
        // {
        //     ModifyLink();
        //     for (int i = 0; i < link.Links.Count; i++)
        //     {
        //         LinkItem item = link.Links[i];
        //         if (item == null) continue;
        //         GameObject link_obj = item.LinkObj;
        //         if (!link_obj)
        //         {
        //             item.Name = SysDefine.LinkNoneTips;
        //             continue;
        //         }
        //         if (string.IsNullOrEmpty(item.Name) || item.Name == SysDefine.LinkNoneTips)
        //         {
        //             item.Name = link_obj.name;
        //         }
        //     }
        //     if (GUI.changed)
        //     {
        //         EditorUtility.SetDirty(link);
        //     }
        // }


        serializedObject.Update();
        // EditorGUILayout.Space();// 空行
        // EditorGUILayout.LabelField("来个标题");

        // 这之间的组件是垂直布局（默认就是垂直布局）
        EditorGUILayout.BeginVertical();

        Root.objectReferenceValue = GetRootObject(link.transform).gameObject;
        EditorGUILayout.ObjectField("Root", Root.objectReferenceValue, typeof(GameObject), true);
        SetLinkList();

        EditorGUILayout.EndVertical();

        SetButton();

        EditorUtility.SetDirty(link);

        serializedObject.ApplyModifiedProperties();
    }

    // private void ModifyLink()
    // {
    //     if (!doOnce)
    //     {
    //         countTemp = link.Links.Count;
    //         doOnce = true;
    //     }
    //     if (countTemp != link.Links.Count)
    //     {
    //         // 置空新link
    //         if (link.Links.Count > countTemp)
    //         {
    //             for (int i = countTemp; i < link.Links.Count; i++)
    //             {
    //                 link.Links[i] = null;
    //             }
    //         }
    //         // 去空格
    //         for (int i = 0; i < link.Links.Count; i++)
    //         {
    //             if (link.Links[i] != null && link.Links[i].LinkObj != null)
    //             {
    //                 link.Links[i].Name = link.Links[i].Name.Trim();
    //                 link.Links[i].LinkObj.name = link.Links[i].LinkObj.name.Trim();
    //             }
    //         }
    //         countTemp = link.Links.Count;
    //     }
    // }


    /// <summary>
    /// 找根节点
    /// </summary>
    /// <param name="childObject"></param>
    /// <returns></returns>
    private Transform GetRootObject(Transform childObject)
    {
        if (childObject.parent == null)
        {
            return childObject;
        }
        else
        {
            return GetRootObject(childObject.parent);
        }
    }
    /// <summary>
    /// 设置Link主列表
    /// </summary>
    private void SetLinkList()
    {
        show_list.boolValue = EditorGUILayout.Foldout(show_list.boolValue, "Links");
        if (!show_list.boolValue) return;

        num.intValue = EditorGUILayout.IntField("num", num.intValue);
        // int differ = num - Links.arraySize;

        // // 设定个数与列表长度
        // if (differ > 0)
        // {
        //     differ = System.Math.Abs(differ);
        //     for (int i = 0; i < differ; i++)
        //     {
        //         LinkItem item = new LinkItem();
        //         Links.Add(item);
        //     }
        // }
        // else if (differ < 0)
        // {
        //     differ = System.Math.Abs(differ);
        //     Links.RemoveAt(Links.Count);
        // }

        // foreach (LinkItem item in Links)
        // {
        //     SetLinkItem(item);
        // }
    }
    /// <summary>
    /// 设置link子项
    /// </summary>
    private void SetLinkItem(LinkItem item)
    {
        // 判断LinkItem项中是否有数据，及名字的刷新
        if (item.Name == null || item.Name == SysDefine.LinkNoneTips)
        {
            if (item.LinkObj != null)
            {
                item.Name = item.LinkObj.name;
            }
            else
            {
                item.Name = SysDefine.LinkNoneTips;
            }
        }
        item.show = EditorGUILayout.Foldout(item.show, item.Name);
        if (!item.show) return;
        item.Name = EditorGUILayout.TextField("Name", item.Name);
        item.LinkObj = (GameObject)EditorGUILayout.ObjectField("Link Obj", item.LinkObj, typeof(GameObject), true);
    }
    /// <summary>
    /// 设置按钮
    /// </summary>
    private void SetButton()
    {
        // 水平布局
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("按钮1"))
        {
            Log.Debug("按下按钮1");
        }

        if (GUILayout.Button("按钮2"))
        {
            Log.Debug("按下按钮2");
        }

        EditorGUILayout.EndHorizontal();
    }
}