using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    人
*/
public class Person : Obj
{
    private int age;
    private bool gender;
    public int Age
    {
        get { return age; }
    }
    public int Gender
    {
        get
        {
            if (gender)
            {
                return 1;
            }
            return 0;
        }
    }
}