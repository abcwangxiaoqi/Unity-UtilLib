using ObjectParse;
using ObjectParseEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

public class ObjectParseWindow : EditorWindow
{
    public static void OpenWindow()
    {
        var window = GetWindow<ObjectParseWindow>(true, "GameSettingEditor Window", true);
    }

    IDraw draw;
    object data;
    IObjectParse parse;

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

    Vector2 scroll;

    Object SourceObj = null;
    int parseIndex = 0;
    string parseError = null;

    EditorWindow newWindow = null;

    private void OnFocus()
    {
        //如果 新建窗口打开 则不能操作
        if(newWindow!=null)
        {
            newWindow.Focus();
        }
    }


    private void OnGUI()
    {
        GUILayout.Space(10);

        GUI.backgroundColor = Color.green;
        if(GUILayout.Button("new"))
        {
            newWindow = NewObjectWindow.OpenWindows((Object recive)=> 
            {
                SourceObj = recive;
            });
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("file", GUILayout.Width(50));
        SourceObj = EditorGUILayout.ObjectField(SourceObj, typeof(UnityEngine.Object), false);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.Label("type", GUILayout.Width(50));
        parseIndex = EditorGUILayout.Popup(parseIndex, parseNameList.ToArray());

        GUILayout.EndHorizontal();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Parse"))
        {
            if (SourceObj == null)
            {
                parseError = "SourceObj is null!!!";
            }
            else
            {
                parseError = null;
                try
                {
                    parse = Activator.CreateInstance(parseTypeList[parseIndex]) as IObjectParse;
                    data = parse.Parse(SourceObj);
                    draw = new ClassDraw(data);
                }
                catch (Exception e)
                {
                    parseError = e.Message;
                    Debug.LogError(e.Source);
                    Debug.LogError(e.Message);
                }
            }
        }
        GUI.backgroundColor = Color.white;

        if (!string.IsNullOrEmpty(parseError))
        {
            GUILayout.Label(parseError);
        }        

        GUILayout.Space(10);

        if (draw == null)
            return;

        scroll = GUILayout.BeginScrollView(scroll, GUI.skin.GetStyle("ObjectPickerPreviewBackground"));

        draw.Draw();

        GUILayout.EndScrollView();


        GUILayout.Space(10);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Save"))
        {
            parse.Save(SourceObj, data);
            AssetDatabase.Refresh();
        }
    }
}
