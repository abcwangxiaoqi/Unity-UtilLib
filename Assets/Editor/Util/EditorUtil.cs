#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorExpand
{
    public static void SelectedObject<T>(this T t) where T : Object
    {
        if (t == null)
            return;

        Selection.activeObject = t;
    }
}

public static class EditorUtil
{
    public static ScriptableItem CreatAssetCurPath<T>(string name, System.Action<T, Dictionary<string, object>> callback = null, Dictionary<string, object> parameters = null) where T : ScriptableObject
    {
        Object[] o = Selection.GetFiltered<Object>(SelectionMode.Assets);

        if (o == null || o.Length == 0)
            return null;
        Object tar = o[0];

        string assetpath = AssetDatabase.GetAssetPath(tar);
        string filename = FileHelper.getFileNameAndTypeByPath(assetpath);
        if (filename.Contains("."))
        {
            //is file not fold
            assetpath = assetpath.Replace("/" + filename, null);
        }

        assetpath += "/" + name + ".asset";

        ScriptableItem item = new ScriptableItem(assetpath);
        item.Creat<T>(callback, parameters);

        typeList t = item.Load<typeList>();
        t.SelectedObject();

        return item;
    }

    public static string GetTranPath(GameObject go,GameObject root=null)
    {
        if (go == null || go==root)
            return null;

        string path = go.name;

        if(go.transform.parent!=null)
        {
            path = GetTranPath(go.transform.parent.gameObject, root)+"/"+path;
        }

        return path;
    }
}
#endif
