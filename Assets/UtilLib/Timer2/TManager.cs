using UnityEngine;

/// <summary>
/// MonoBehaviour子类的单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class TManager<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T mInstance;
    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject(typeof(T).ToString());
                GameObject.DontDestroyOnLoad(go);
                mInstance = go.AddComponent<T>();
            }
            return mInstance;
        }
    }

    public void Initialize(GameObject parent)
    {
        if (parent != null)
        {
            this.gameObject.transform.parent = parent.transform;
        }
    }

}
