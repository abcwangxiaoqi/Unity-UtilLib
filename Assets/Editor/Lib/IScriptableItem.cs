using System;
using System.Collections.Generic;
using UnityEngine;
namespace EditorTools
{
    public interface IScriptableItem : IObjectBase
    {
        void Creat<T>(Action<T, Dictionary<string, object>> callback = null, Dictionary<string, object> parameters = null) where T : ScriptableObject;
    }
}

