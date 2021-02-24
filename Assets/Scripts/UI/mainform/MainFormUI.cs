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
    protected override void Initialize()
    {
        item = Get(this, "item");
    }
    private void ShowContext()
    {
        FireEvent(new Events.UI.OpenUI("SettleAccounts"));
    }
    protected override void RegEvents()
    {
        SetBtnEvent(item, ShowContext);
    }
    protected override void OnEnable()
    {
    }
    protected override void OnUpdata()
    {
    }
    protected override void OnDisable()
    {
    }
    protected override void OnDestroy()
    {
    }
}