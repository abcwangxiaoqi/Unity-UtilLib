using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ScriptNodeFlow
{
    public class FixedWindow
    {
        public string shareData { get; private set; }

        int shareDataIndex = 0;

        List<string> ShareDataList = new List<string>();

        GUIStyle shareDataTitle;

        public FixedWindow(string shareDataName)
        {
            windowRect = new Rect(20, 50, 250, 100);

            Assembly _assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp.dll");
            Type[] tys = _assembly.GetTypes();

            ShareDataList = new List<string>() { "None" };
            foreach (var item in tys)
            {
                if (item.IsSubclassOf(typeof(SharedData)) && !item.IsInterface && !item.IsAbstract)
                {
                    ShareDataList.Add(item.FullName);
                }
            }

            shareDataTitle = new GUIStyle(UnityEditor.EditorStyles.boldLabel);
            shareDataTitle.fixedHeight = 25;
            shareDataTitle.fontSize = 20;
            shareDataTitle.alignment = TextAnchor.MiddleCenter;
            shareDataTitle.normal.textColor = Color.green;

            if (!string.IsNullOrEmpty(shareDataName))
            {
                shareDataIndex = ShareDataList.IndexOf(shareDataName);
            }
        }
        Rect windowRect;
        public void draw()
        {
            windowRect = GUI.Window(int.MaxValue, windowRect, gui, string.Empty);
        }

        void gui(int id)
        {
            GUILayout.Label("ShareData", shareDataTitle);

            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            shareDataIndex = EditorGUILayout.Popup(shareDataIndex, ShareDataList.ToArray());
            EditorGUI.EndDisabledGroup();

            if (shareDataIndex > 0)
            {
                shareData = ShareDataList[shareDataIndex];
            }
            else
            {
                shareData = null;
            }

        }
    }

}