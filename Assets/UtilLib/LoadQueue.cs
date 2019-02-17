using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadQueue
{
    static LoadQueue instance = null;
    public static LoadQueue Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new LoadQueue();
            }
            return instance;
        }
    }

    UpdateTimer timer = null;

    Queue<Action> actionlist = new Queue<Action>();
    int bfNum = 5;

    public LoadQueue()
    {
        timer = new UpdateTimer(loadAction,1);
        timer.Start();
    }

    void loadAction()
    {
        if (actionlist.Count == 0)
            return;
        for (int i = 0; i < bfNum; i++)
        {
            Action action = actionlist.Dequeue();
            if(action==null)
            {
                continue;
            }

            action();
        }
    }

    public void LoadPool(Action action)
    {
        if (action == null)
            return;

        actionlist.Enqueue(action);
    }
}
