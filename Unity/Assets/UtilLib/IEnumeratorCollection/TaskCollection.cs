using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TaskCollection
{
    protected Queue<IEnumerator> tasklist = new Queue<IEnumerator>();

    public void AddIEnumerator(IEnumerator enumerator)
    {
        tasklist.Enqueue(enumerator);
    }

    System.Action action = null;
    public void AddFinishAction(System.Action _action)
    {
        action += _action;
    }

    protected void finish()
    {
        if(action!=null)
        {
            action();
        }
    }
}