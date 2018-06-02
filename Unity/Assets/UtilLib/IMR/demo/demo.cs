using UnityEngine;
using System.Collections;

public class demo : MonoBehaviour {

    testInter t;

    private void Start()
    {
        t = new testInter();
    }

    GUIStyle style;

    // Use this for initialization
    void OnGUI()
    {
        style = GUI.skin.button;
        style.fontSize = 10;
        
        if (GUILayout.Button("designer creat", style))
        {
            t.creat("demo");
        }
        if (GUILayout.Button("designer destroy", style))
        {
            t.destroy();
        }
        if (GUILayout.Button("model dispose", style))
        {
            t.Dispose();
        }
    }
}
