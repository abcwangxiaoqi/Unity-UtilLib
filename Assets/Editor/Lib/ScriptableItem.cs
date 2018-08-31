
using System;
using System.Collections.Generic;
using UnityEngine;
namespace EditorTools
{
    public class ScriptableItem : ObjectBase, IScriptableItem
    {
        public ScriptableItem(string assetpath)
            : base(assetpath)
        {

        }

        public void Creat<T>(Action<T, Dictionary<string, object>> callback = null, Dictionary<string, object> parameters = null)
            where T : ScriptableObject
        {
            T t = ScriptableObject.CreateInstance<T>();

            if (callback != null)
            {
                callback.Invoke(t, parameters);
            }
            CreatAsset(t);
        }
    }
}