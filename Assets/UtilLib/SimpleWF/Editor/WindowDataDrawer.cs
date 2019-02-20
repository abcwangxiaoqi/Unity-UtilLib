using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WindowData))]
public class WindowDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);

        base.OnInspectorGUI();

        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);

        GUI.color = Color.green;
        if(GUILayout.Button("OpenGraph",GUILayout.Height(50)))
        {
            WFEditorWindow.Open(target);
        }
    }
}
