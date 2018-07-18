#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class EditorExpand
{
    public static void SelectedObject<T>(this T t) where T : Object
    {
        if (t == null)
            return;

        Selection.activeObject = t;
    }
}
#endif
