using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorTools;
using UnityEditor;

public class EditorTest:Editor
{
    public static void ObjectBase()
    {
        ObjectBase obj = new ObjectBase("Assets/1.txt");
        TextAsset txt = obj.Load<TextAsset>();
    }

    [MenuItem("Tools/test")]
    static void test()
    {
        string s = "1234567890";
        char t = s.ToCharArray()[-2];
        Debug.Log(t);
    }
}