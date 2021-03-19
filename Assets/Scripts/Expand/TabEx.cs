using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// tab键拓展
/// </summary>
public class TabEx : MonoBehaviour
{
    private EventSystem system;
    private Map<int, GameObject> Objs = new Map<int, GameObject>();
    private int index;// 用于存储得到的字典的索引

    void Start()
    {
        // 初始化字段
        system = EventSystem.current;
        
        index = 0;
        // 给字典赋值
        for (int i = 0; i < transform.childCount; i++)
        {
            Objs.Add(i, transform.GetChild(i).gameObject);
        }
        // 得到字典中对应索引的游戏物体
        GameObject obj;
        Objs.TryGetValue(index, out obj);
        // 设置第一个可交互的UI为高亮状态
        system.SetSelectedGameObject(obj, new BaseEventData(system));
    }

    void Update()
    {
        // 当有 UI 高亮(得到高亮的UI，不为空)并且 按下Tab键
        if (system.currentSelectedGameObject != null && Input.GetKeyDown(KeyCode.Tab))
        {
            // 得到当前高亮状态的 UI 物体
            GameObject hightedObj = system.currentSelectedGameObject;
            // 看是场景中第几个物体
            foreach (KeyValuePair<int, GameObject> item in Objs)
            {
                if (item.Value == hightedObj)
                {
                    index = item.Key + 1;
                    // 超出索引 将Index归零
                    if (index == Objs.Count)
                    {
                        index = 0;
                    }
                    break;
                }
            }
            // 得到对应索引的游戏物体
            GameObject obj;
            Objs.TryGetValue(index, out obj);
            // 使得到的游戏物体高亮
            system.SetSelectedGameObject(obj, new BaseEventData(system));
        }
    }
}