using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 启动器
/// </summary>
public class Launcher : MonoBehaviour
{
    SystemMgr SysMgr;
    void Start()
    {
        SysMgr = new SystemMgr();
        SysMgr.Launch(SysMgr);
    }

    void Update()
    {
        
    }
}