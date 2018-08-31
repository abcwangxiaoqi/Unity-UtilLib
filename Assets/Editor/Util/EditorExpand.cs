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

    /// <summary>
    /// Sets the light map scale.
    /// </summary>
    /// <param name="renderers">Renderers.</param>
    /// <param name="scale">Scale.</param>
    public static void SetLightMapScale(this Renderer[] renderers, float scale)
    {
        if (null == renderers)
            return;


        foreach (var item in renderers)
        {
            SerializedObject so = new SerializedObject(item);
            so.FindProperty("m_ScaleInLightmap").floatValue = scale;
            so.ApplyModifiedProperties();
        }
    }

    /// <summary>
    /// tar whether is child of go
    /// </summary>
    /// <param name="go"></param>
    /// <param name="tar"></param>
    /// <returns></returns>
    public static bool isChild(this Transform go, GameObject tar)
    {
        if (null == go || null == tar)
        {
            return false;
        }

        if (go.parent != null && go.parent == tar.transform)
        {
            return true;
        }

        return isChild(go.parent, tar);
    }
}
