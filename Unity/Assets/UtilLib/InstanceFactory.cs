using System.Collections.Generic;
using UnityEngine;

public sealed class MSingletonFactory
{
    static Dictionary<string, MonoBehaviour> dic = new Dictionary<string, MonoBehaviour>();

    public static T Get<T>() where T : MonoBehaviour
    {
        string type = typeof(T).Name;
        if (dic.ContainsKey(type))
        {
            return (T)dic[type];
        }

        GameObject go = new GameObject(type);
        Object.DontDestroyOnLoad(go);
        T t = go.AddComponent<T>();
        dic.Add(type, t);
        return t;
    }

    public static void Destroy<T>() where T : MonoBehaviour
    {
        string type = typeof(T).Name;
        if (dic.ContainsKey(type))
        {
            Object.Destroy(dic[type].gameObject);
            dic[type] = null;
            dic.Remove(type);
        }
    }
}

public sealed class SingletonFactory
{
    static Dictionary<string, object> dic = new Dictionary<string, object>();

    public static T Get<T>() where T : class, new()
    {
        string type = typeof(T).Name;
        if (dic.ContainsKey(type))
        {
            return (T)dic[type];
        }
        T t = new T();
        dic.Add(type, t);
        return t;
    }

    public static void Destroy<T>() where T : class
    {
        string type = typeof(T).Name;
        if (dic.ContainsKey(type))
        {
            dic[type] = null;
            dic.Remove(type);
        }
    }
}
