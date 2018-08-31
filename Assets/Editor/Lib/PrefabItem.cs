
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
namespace EditorTools
{
    public class PrefabItem : ObjectBase, IPrefabItem
    {
        public PrefabItem(string assetpath)
            : base(assetpath)
        {

        }

        public void CreatPrefab(UnityEngine.Object obj, Action<GameObject, Dictionary<string, object>> callback = null, Dictionary<string, object> parameters = null)
        {
            GameObject go = null;
            if (obj != null)
            {
                go = GameObject.Instantiate(obj) as GameObject;
            }
            else
            {
                go = new GameObject();
            }

            if (callback != null)
            {
                callback(go, parameters);
            }

            string fold = PathHelper.GetFoldByFullName(path);
            if (!Directory.Exists(fold))
            {
                Directory.CreateDirectory(fold);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            PrefabUtility.CreatePrefab(path, go);
            GameObject.DestroyImmediate(go);
            AssetDatabase.Refresh();
        }
    }
}
