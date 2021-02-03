using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Link : MonoBehaviour
{
    public GameObject Root;// 根节点
    public bool Show;
    public List<UILink> Links;// 链接列表
    public class UILink
    {
        public string Name;
        public GameObject LinkObj;
        public Component component;
    }
}

[CustomEditor(typeof(Link))]
public class LinkEditor : Editor
{
    
}