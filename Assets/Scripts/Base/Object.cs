using System;
using System.Collections.Generic;
using UnityEngine;

/*
    基于Object基类的子类，继承该类的对象在不使用时必须调用Dispose
*/
public class Object : IDisposable
{
    public int obj_id;
    public string obj_name;

    protected virtual void Dispose(bool disposed) {}
    public void Dispose(){

    }
}