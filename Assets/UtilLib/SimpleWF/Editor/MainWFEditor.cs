using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[CustomEditor(typeof(MainWF))]
public class MainWFEditor : Editor {

    MainWF Target;
    List<string> ShareDataList = new List<string>();

    private void Awake()
    {
        Target = target as MainWF;
    }

    private void OnEnable()
    {      
        ShareDataList.Clear();

        Assembly assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp.dll");
        Type[] tys = assembly.GetTypes();

        ShareDataList.Add("None");
        foreach (var item in tys)
        {
            if (item.IsSubclassOf(typeof(SharedData)) && !item.IsInterface && !item.IsAbstract)
            {
                ShareDataList.Add(item.FullName);
            }
        }
        
        selectIndex = ShareDataList.IndexOf(Target.shareData);
    }

    int selectIndex = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        selectIndex = EditorGUILayout.Popup(selectIndex, ShareDataList.ToArray());

        if(selectIndex>0)
        {
            Target.shareData = ShareDataList[selectIndex];
        }
        else
        {
            Target.shareData = null;
        }
        

        // 保存
        if (GUI.changed)
        {
            EditorUtility.SetDirty(Target);
        }
    }
}
