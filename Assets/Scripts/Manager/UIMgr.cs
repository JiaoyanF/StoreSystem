using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Camera UICamera { private set; get; }// ui主相机

    public override void Awake()
    {
        base.Awake();

        ResMgr = system_mgr.GetSingleT<ResourcesMgr>();

        Root = (GameObject)Resources.Load(SysDefine.PrefabPath + "Root");// 获取根节点
        Root = UnityEngine.Object.Instantiate(Root);
        UnityEngine.Object.DontDestroyOnLoad(Root);// 切换场景不销毁
        UICamera = Tool.FindChild<Camera>(Root.transform, "Camera");
        
        FireEvent(new Events.UI.OpenUI("Start", Localization.Format("SYSTEM_NAME"), "111"));
    }

    public void ShowUI(UI ui)
    {
        if (ui.Layer == UILayer.Full)
        {
            for (uis.Begin(); uis.Next();)
            {
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
    }

    private void OnCreateUIEvent(Obj sender, Events.UI.OpenUI e)
    {
        Log.Debug("open_ui：" + e.UI);
        LoadUI(e.UI, e.Args);
    }

    #region 私有方法

    /// <summary>
    /// 加载ui
    /// </summary>
    /// <param name="UIName"></param>
    /// <returns></returns>
    private void LoadUI(string UIName, params object[] Args)
    {
        if (uis.ContainsKey(UIName))
        {
            uis[UIName].Show();
            return;
        }

        ResMgr.LoadAsset(this, UIName, Args);// 加载资源并实例化
    }

    #endregion
}