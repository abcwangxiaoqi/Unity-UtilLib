using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using EditorTools;

public class MyTimelineEditor
{
    [MenuItem("Timeline/Test")]
    static void Test()
    {
        string path = "Assets/TimelineTest/Mytl.playable";




        ObjectBase objectBase = new ObjectBase(path);
        TimelineAsset asset = objectBase.Load<TimelineAsset>();
        Debug.Log(asset.name);
    }
}

[CustomEditor(typeof(PlayableAsset))]
public class PlayableTimelineEditor : Editor
{

    private void Awake()
    {

        Debug.Log("Awake");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    public override void OnInspectorGUI()
    {
       // base.OnInspectorGUI();

        if (GUILayout.Button("dd"))
        { }
    }
}

[CustomEditor(typeof(ControlTrack))]
public class PlayableAssetEditor:Editor
{

    private void Awake()
    {
        ControlTrack t = target as ControlTrack;
        var clips= t.GetClips();

        foreach (var item in clips)
        {
            Debug.Log(item.asset.name);
        }

        Debug.Log("Awake");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("dd"))
        {}
    }
}
