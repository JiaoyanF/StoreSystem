using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 启动器
/// </summary>
public class Launcher : MonoBehaviour
{
    SystemMgr SysMgr;
    void Start()
    {
        Localization.LoadLang();
        SysMgr = new SystemMgr();
        SysMgr.Launch(SysMgr);
        
        SceneManager.LoadScene("Running");
    }

    void Update()
    {

    }
}