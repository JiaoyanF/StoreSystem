using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 主面板
/// </summary> 
public class MainFormUI : UI
{
    public override UILayer Layer { get { return UILayer.Full; } }
    private GameObject item;
    public override void Initialize()
    {
        item = Get(this, "item");
    }
    private void ShowContext()
    {
        Log.Debug("进入按钮");
        FireEvent(new Events.UI.OpenUI("SettleAccounts"));
    }
    public override void RegEvents()
    {
        SetBtnEvent(item, ShowContext);
    }
    public override void OnEnable()
    {
    }
    public override void OnUpdata()
    {
    }
    public override void OnDisable()
    {
    }
    public override void OnDestroy()
    {
    }
}