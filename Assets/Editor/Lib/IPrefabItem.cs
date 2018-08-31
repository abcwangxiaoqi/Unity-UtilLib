
using System.Collections;
using System.Collections.Generic;
using System;
namespace EditorTools
{
    public interface IPrefabItem : IObjectBase
    {
        void CreatPrefab(UnityEngine.Object obj, Action<UnityEngine.GameObject, Dictionary<string, object>> callback = null, Dictionary<string, object> parameters = null);
    }
}
