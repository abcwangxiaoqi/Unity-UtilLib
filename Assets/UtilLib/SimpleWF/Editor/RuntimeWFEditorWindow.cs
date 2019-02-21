using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RuntimeWFEditorWindow : WFWindow
{
    static MainWF target;
    static RuntimeWFEditorWindow windows;
    public static void Open(MainWF obj)
    {
        if (obj == null)
            return;

        target = obj;
        windows = RuntimeWFEditorWindow.GetWindow<RuntimeWFEditorWindow>(obj.name);
    }


    protected override void Awake()
    {
        base.Awake();

        target.onFinish += (bool b) =>
        {
            finished = true;

            foreach (var item in windowList)
            {
                item.SetState(State.Idle);
            }
        };

        generateWindowData(target.data);

        foreach (var item in windowList)
        {
            item.switchMode(Mode.Runtime);
        }

        fixedWindow.switchMode(Mode.Runtime);
    }

    bool finished = false;

    protected override void OnGUI()
    {
        if (!finished)
        {
            foreach (var item in windowList)
            {
                if (item.Id == target.currentID)
                {
                    item.SetState(State.Running);
                }
                else
                {
                    item.SetState(State.Idle);
                }
            }
        }

        base.OnGUI();

    }
}
