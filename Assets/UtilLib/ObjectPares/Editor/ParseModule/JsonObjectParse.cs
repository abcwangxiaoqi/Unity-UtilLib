using LitJson;
using System.IO;
using UnityEditor;
using UnityEngine;

public abstract class JsonObjectParse<T> : IObjectParse
    where T : class, new()
{
    public Object Create(string fold, string filename)
    {
        string path = fold + "/" + filename;

        T t = new T();

        string js = JsonMapper.ToJson(t);

        File.WriteAllText(path, js);

        AssetDatabase.Refresh();

        return AssetDatabase.LoadAssetAtPath<Object>(path);
    }

    public object Parse(Object source)
    {
        string path = AssetDatabase.GetAssetPath(source);
        object obj = null;

        if (!path.EndsWith(".json"))
        {
            Debug.LogError("path is wrong,must be end of .json");
            return obj;
        }

        TextAsset txt = source as TextAsset;

        obj = JsonMapper.ToObject<T>(txt.text);

        return obj;
    }

    public void Save(Object source, object obj)
    {
        string path = AssetDatabase.GetAssetPath(source);
        string json = JsonMapper.ToJson(obj);
        File.WriteAllText(path, json);
    }
}