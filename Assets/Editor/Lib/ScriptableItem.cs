﻿
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class ScriptableItem : ObjectBase
{
    public ScriptableItem(string assetpath)
        : base(assetpath)
    {

    }

    public void Creat<T>(Action<T, Dictionary<string, object>> callback = null, Dictionary<string, object> parameters = null)
        where T:ScriptableObject
    {      
        T t = ScriptableObject.CreateInstance<T>();

        if (callback != null)
        {
            callback.Invoke(t, parameters);
        }
        CreatAsset(t);
    }
}
