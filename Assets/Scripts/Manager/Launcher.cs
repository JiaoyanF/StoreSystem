using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    protected internal LauncherUI LauncherUI = null;
    void Start()
    {
        //加载UI
        // GameObject launcherui = UnityEngine.Object.Instantiate(Resources.Load("Prefab/Launcher")) as GameObject;
        // this.LauncherUI = launcherui.AddComponent<LauncherUI>();
        
    }

    void Update()
    {
        
    }
}
