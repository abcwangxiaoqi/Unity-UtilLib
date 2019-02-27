using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace ScriptNodeFlow
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

            GUILayout.Space(10);

            GUI.color = Color.green;

            //true : gameobject in Hierarchy
            //false : prefab
            bool isGameObject = Target.gameObject.activeInHierarchy;

            EditorGUI.BeginDisabledGroup(!(Application.isPlaying && isGameObject)); 
            
            if (GUILayout.Button("Graph"))
            {
                RuntimeNodeCanvas.Open(Target);
            }

            EditorGUI.EndDisabledGroup();

            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(Target);
            }
        }
    }
}
