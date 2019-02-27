using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptNodeFlow
{
    [CustomEditor(typeof(NodeCanvasData))]
    public class NodeCanvasDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);

            base.OnInspectorGUI();

            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);

            GUI.color = Color.green;

            EditorGUI.BeginDisabledGroup(Application.isPlaying || EditorApplication.isCompiling);

            if (GUILayout.Button("OpenGraph", GUILayout.Height(50)))
            {
                EditorNodeCanvas.Open(target);
            }

            EditorGUI.EndDisabledGroup();
        }
    }

}