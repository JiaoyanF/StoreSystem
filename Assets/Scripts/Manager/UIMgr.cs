using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Def;
using System;

/// <summary>
/// ui管理器
/// </summary>
public class UIMgr : Obj
{
    /// <summary>
    /// 当前ui
    /// </summary>
    /// <typeparam name="string">ui名称</typeparam>
    /// <typeparam name="UI">ui实例对象</typeparam>
    /// <returns></returns>
    public Map<string, UI> uis = new Map<string, UI>();
    public GameObject Root;// 根节点
    public UI ui;
    public ResourcesMgr ResMgr;// 资源管理器
    public NetMgr NetMgr;// 网络管理器
    public Loom Loom { get { return system_mgr.Loom; } }
    public Camera UICamera { private set; get; }// ui主相机
    public RawImage BG;// 背景

    public override void Awake()
    {
        base.Awake();

        ResMgr = system_mgr.GetSingleT<ResourcesMgr>();
        NetMgr = system_mgr.GetSingleT<NetMgr>();

        Root = (GameObject)Resources.Load(SysDefine.PrefabPath + "Root");// 获取根节点
        Root = UnityEngine.Object.Instantiate(Root);
        Root.name = Root.name.Replace("(Clone)", "");
        UnityEngine.Object.DontDestroyOnLoad(Root);// 切换场景不销毁
        UICamera = Tool.FindChild<Camera>(Root.transform, "Camera");
        BG = Tool.FindChild<RawImage>(Root.transform, "BGImg");

        FireEvent(new Events.UI.OpenUI("Start"));
    }

    public void ShowUI(UI ui)
    {
        // 普通界面关闭其他打开的普通界面
        if (ui.Layer == UILayer.Normal)
        {
            for (uis.Begin(); uis.Next();)
            {
                if (uis.Key == ui.Name) return;
                if (uis.Value.Layer == UILayer.Normal)
                {
                    uis.Value.Close();
                }
            }
        }
        // 全屏界面关闭其他所有打开界面
        if (ui.Layer == UILayer.Full)
        {
            for (uis.Begin(); uis.Next();)
            {
                if (uis.Key == ui.Name) return;
                if (uis.Value != null)
                {
                    uis.Value.Close();
                }
            }
        }
        if (!uis.ContainsKey(ui.Name))
        {
            uis.Add(ui.Name, ui);
        }
    }
    public void CloseUI(UI ui)
    {
        if (uis.ContainsKey(ui.Name))
        {
            uis.Remove(ui.Name);
        }
    }
    protected override void RegEvents()
    {
        RegEventHandler<Events.UI.OpenUI>(OnCreateUIEvent);
        RegEventHandler<Events.UI.CloseUI>(OnCloseUIEvent);
    }

    #region 私有方法

    private void OnCreateUIEvent(Obj sender, Events.UI.OpenUI e)
    {
        LoadUI(e.UI, e.Args);
    }
    private void OnCloseUIEvent(Obj sender, Events.UI.CloseUI e)
    {
        Log.Debug("关闭ui：{0}", e.UI);
        if (uis.ContainsKey(e.UI))
        {
            uis[e.UI].Close();
            CloseUI(uis[e.UI]);
            return;
        }
    }
    /// <summary>
    /// 加载ui
    /// </summary>
    /// <param name="UIName"></param>
    /// <returns></returns>
    private void LoadUI(string UIName, params object[] Args)
    {
        if (uis.ContainsKey(UIName))
        {
            uis[UIName].Show(Args);
            return;
        }

        ResMgr.LoadAsset(this, UIName, Args);// 加载资源并实例化
    }

    #endregion
}