using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 锁定面板
/// </summary> 
public class LockingUI : UI
{
    public override UILayer Layer { get { return UILayer.Full; } }
    InputField un_input;
    InputField ps_input;
    protected override void Initialize()
    {
        un_input = GetControl<InputField>(this, "un_input");
        ps_input = GetControl<InputField>(this, "ps_input");
    }
    protected override void RegEvents()
    {
    }

    protected override void OnEnable()
    {
    }
    protected override void OnUpdate()
    {
    }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}