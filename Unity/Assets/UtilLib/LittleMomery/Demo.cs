using UnityEngine;
using System.Collections;
using LittleMomery;

public class testCacheItem:MomeryItem
{
    public override void unload()
    {
        Debug.Log("override load,key=" + key + ",type=" + type);
    }
}

public class testItem
{
    public Vector3 vec = Vector3.zero;
}

public class Demo : MonoBehaviour {



    GUIStyle style;
    string key = "test";
    string v = "testvalue";


    testItem item = new testItem();
    private void Start()
    {
        SingletonFactory.Get<MomeryInter>().RegisterMap<testItem, testCacheItem>();
    }

    // Use this for initialization
    void OnGUI()
    {
        style = GUI.skin.button;
        style.fontSize = 10;

        if (GUILayout.Button("insert cache", style))
        {
            SingletonFactory.Get<MomeryInter>().Insert<testItem>(key, item);
        }
        if (GUILayout.Button("get cache", style))
        {
            testItem val = SingletonFactory.Get<MomeryInter>().Get<testItem>(key);
            Debug.Log(val.vec);
        }
        if (GUILayout.Button("destroy cache", style))
        {
            SingletonFactory.Get<MomeryInter>().Delete<testItem>(key);
        }
    }
}
