using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace NodeTool
{
    [CustomEditor(typeof(NodeController))]
    public class NodeControllerEditor : Editor
    {
        NodeController Target;

        private void Awake()
        {
            Target = target as NodeController;
        }

        int selectIndex = 0;
        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            GUILayout.Space(5);

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Graph"))
                {
                    RuntimeNodeCanvas.Open(Target);
                }
            }

            // 保存
            if (GUI.changed)
            {
                EditorUtility.SetDirty(Target);
            }
        }
    }
}
