using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpdatePeriover<T> : TManager<T>
     where T : MonoBehaviour
{
    // 必须执行列表
    List<Action> corelist = new List<Action>();

    //窗口大小
    int windowSize = 10;

    List<Action> origanlList = new List<Action>();

    Queue<Action> frameOperation = new Queue<Action>();

    public void Add(Action action, bool core = true)
    {
        if (null == action)
            return;

        Action at = delegate ()
        {
            if (core)
            {
                corelist.Add(action);
            }
            else
            {
                origanlList.Add(action);
            }
        };

        frameOperation.Enqueue(at);
    }

    public void Delete(Action action, bool core = true)
    {
        if (null == action)
            return;

        Action at = delegate ()
        {
            if (core)
            {
                corelist.Remove(action);
            }
            else
            {
                int index = origanlList.IndexOf(action);

                if (index < 0)
                    return;

                origanlList.RemoveAt(index);

                if (index < nextWinIndex)
                {
                    nextWinIndex--;
                    Debug.Log("nextWinIndex=" + nextWinIndex);
                }
            }
        };
        frameOperation.Enqueue(at);
    }

    public void ClearCoreList()
    {
        corelist.Clear();
    }

    public void ClearOraginalList()
    {
        origanlList.Clear();
        nextWinIndex = 0;
    }

    public void ClearAll()
    {
        ClearCoreList();
        ClearOraginalList();
    }

    public void SetWindowSize(int num)
    {
        if (num <= 0)
            return;
        windowSize = num;
    }

    int nextWinIndex = 0;

    /// <summary>
    /// update frame operation
    /// </summary>
    void updateFrameOperations()
    {
        while (frameOperation.Count > 0)
        {
            frameOperation.Dequeue().Invoke();
        }
    }

    protected void execute()
    {
        // frame operation
        updateFrameOperations();

        //core
        for (int i = 0; i < corelist.Count; i++)
        {
            corelist[i].Invoke();
        }

        // windows clip
        if (origanlList.Count == 0)
            return;

        for (int i = 0; i < windowSize && origanlList.Count>0; i++)
        {
            origanlList[nextWinIndex].Invoke();

            nextWinIndex++;
            if (nextWinIndex >= origanlList.Count)
            {
                updateFrameOperations();
                nextWinIndex = 0;
            }
        }
    }
}

public class UpdateRun : UpdatePeriover<UpdateRun>
{
    private void Update()
    {
        execute();
    }
}

public class FixedUpdateRun : UpdatePeriover<FixedUpdateRun>
{
    private void FixedUpdate()
    {
        execute();
    }
}

public class LateUpdateRun : UpdatePeriover<LateUpdateRun>
{
    private void LateUpdate()
    {
        execute();
    }
}

public class AnsyOperate
{
    static AnsyOperate instance = null;
    public static AnsyOperate Instance
    {
        get
        {
            if(null == instance)
            {
                instance = new AnsyOperate();
            }
            return instance;
        }
    }

    public void Add(Action action)
    {
        if (null == action)
            return;

        Action at = null;
        at = delegate ()
        {
            action.Invoke();
            UpdateRun.Instance.Delete(at,false);
        };

        UpdateRun.Instance.Add(at,false);
    }
}