using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.IO;

using Object = UnityEngine.Object;

public class NewObjectWindow : EditorWindow
{
    public static EditorWindow OpenWindows(Action<Object> callback)
    {
        NewObjectWindow current = GetWindow<NewObjectWindow>(true, "New Object Window", true);
        current.reciveCallback = callback;
        return current;
    }

    Action<Object> reciveCallback = null;

    List<Type> parseTypeList = new List<Type>();
    List<string> parseNameList = new List<string>();

    private void Awake()
    {
         Assembly _assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp-Editor.dll");
        //Assembly _assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp.dll");
        Type[] tys = _assembly.GetTypes();

        foreach (var item in tys)
        {
            var attributes = item.GetCustomAttributes(typeof(ParseNameAttribute), false);
            if (attributes == null || attributes.Length == 0)
                continue;

            ParseNameAttribute parseName = attributes[0] as ParseNameAttribute;
            parseNameList.Add(parseName.Name);
            parseTypeList.Add(item);
        }
    }

    string fold;
    string filename;
    int parseIndex = 0;
    string error;
    IObjectParse parse;
    private void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("fold", GUILayout.Width(50));
        GUILayout.TextField(fold);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("select", GUILayout.Width(50)))
        {
            string tmp = EditorUtility.OpenFolderPanel("", Application.dataPath, "");

            //只能选择项目内路径
            if (!tmp.StartsWith(Application.dataPath))
            {
                tmp = null;
            }

            if (!string.IsNullOrEmpty(tmp))
            {
                fold = "Assets" + tmp.Replace(Application.dataPath, null);
            }
        }
        GUI.backgroundColor = Color.white;

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.Label("filename", GUILayout.Width(50));
        filename = GUILayout.TextField(filename);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.Label("type", GUILayout.Width(50));
        parseIndex = EditorGUILayout.Popup(parseIndex, parseNameList.ToArray());

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("creat"))
        {
            if (!Directory.Exists(fold))
            {
                error = "fold is not right";
                return;
            }

            if (string.IsNullOrEmpty(filename))
            {
                error = "filename is not right";
                return;
            }

            parse = Activator.CreateInstance(parseTypeList[parseIndex]) as IObjectParse;

            try
            {
                Object obj = parse.Create(fold, filename);

                if (reciveCallback != null)
                {
                    reciveCallback.Invoke(obj);
                }

                Close();
            }
            catch (Exception e)
            {
                error = e.Message;
                Debug.LogError(e.Source);
                Debug.LogError(e.Message);
            }
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Space(10);

        if (!string.IsNullOrEmpty(error))
        {
            GUILayout.Label(error);
        }
    }
}