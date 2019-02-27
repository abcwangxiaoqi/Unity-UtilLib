using UnityEditor;
using UnityEngine;

public static class MyEditorLayout
{
    public static void Label(string txt, out Rect rect)
    {
        GUIContent content = new GUIContent(txt);
        rect = GUILayoutUtility.GetRect(content, EditorStyles.label);
        GUI.Label(rect, txt, EditorStyles.label);
    }

    public static void Label(string txt, GUIStyle style, out Rect rect)
    {
        GUIContent content = new GUIContent(txt);
        rect = GUILayoutUtility.GetRect(content, style);
        GUI.Label(rect, txt, style);
    }
    
    public static bool Button(string txt, GUIStyle style, out Rect rect)
    {
        GUIContent content = new GUIContent(txt);
        rect = GUILayoutUtility.GetRect(content, style);
        return GUI.Button(rect, content, style);
    }

    public static bool Button(string txt, out Rect rect)
    {
        GUIContent content = new GUIContent(txt);
        rect = GUILayoutUtility.GetRect(content, EditorStyles.miniButton);
        return GUI.Button(rect, content, EditorStyles.miniButton);
    }
}
