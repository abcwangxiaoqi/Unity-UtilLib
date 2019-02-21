using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[CustomEditor(typeof(MainWF))]
public class MainWFEditor : Editor {

    MainWF Target;

    private void Awake()
    {
        Target = target as MainWF;
    }

    int selectIndex = 0;
    public override void OnInspectorGUI()
    {
        //EditorGUI.BeginDisabledGroup(true);

        base.OnInspectorGUI();

        //EditorGUI.EndDisabledGroup();

        if(Application.isPlaying)
        {
            if(GUILayout.Button("Graph"))
            {
                RuntimeWFEditorWindow.Open(Target);
            }
        }

        // 保存
        if (GUI.changed)
        {
            EditorUtility.SetDirty(Target);
        }
    }
}
