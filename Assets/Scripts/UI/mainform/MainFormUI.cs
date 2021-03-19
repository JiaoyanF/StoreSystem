using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 主面板
/// </summary> 
public class MainFormUI : UI
{
    public override UILayer Layer { get { return UILayer.Full; } }
    private Button Sales;
    private Button GoodsMan;
    private Button VipMan;
    private Button StaffMan;
    private Button About;
    Map<Button, List<Button>> Btns = new Map<Button, List<Button>>();
    protected override void Initialize()
    {
        Sales = GetControl<Button>(Get(this, "Sales"));
        GoodsMan = GetControl<Button>(Get(this, "GoodsMan"));
        VipMan = GetControl<Button>(Get(this, "VipMan"));
        StaffMan = GetControl<Button>(Get(this, "StaffMan"));
        About = GetControl<Button>(Get(this, "About"));

        ShowBG(true);
        ChangeBG("MainBG");
        GetBtns(StaffMan);
        GetBtns(About);
    }
    protected override void RegEvents()
    {
        TitleBtnClickEvent();
        SubBtnClickEvent();
    }
    /// <summary>
    /// 获取标题组
    /// </summary>
    /// <param name="title_btn"></param>
    public void GetBtns(Button title_btn)
    {
        GameObject con = Get(title_btn.gameObject, "Context");
        List<Button> lis = GetComponentChild<Button>(con);
        Btns.Add(title_btn, lis);
    }
    /// <summary>
    /// 标题导航按钮点击事件
    /// </summary>
    private void TitleBtnClickEvent()
    {
        foreach (var item in Btns)
        {
            GameObject con = Get(item.Key.gameObject, "Context");
            SetBtnEvent(item.Key, delegate ()
            {
                SetActive(con, true);
            });
        }
    }
    public void SubBtnClickEvent()
    {
        List<Button> child_btn = Btns[About];
        SetBtnEvent(child_btn[3], ui_mgr.system_mgr.Loom.ExitSystem);
    }
    protected override void OnEnable() { }
    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideSubpanel();
        }
    }
    /// <summary>
    /// 隐藏展开菜单
    /// </summary>
    private void HideSubpanel()
    {
        foreach (var item in Btns)
        {
            GameObject con = Get(item.Key.gameObject, "Context");
            SetActive(con, false);
        }
    }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}