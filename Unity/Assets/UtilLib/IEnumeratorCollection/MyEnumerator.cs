using UnityEngine;
using System.Collections;

public abstract class MyEnumerator
{
    public CallBack finishAction = null;
    protected SimpleTask task = null;
    abstract public void AddIEnumerator(IEnumerator ienumerator);
    abstract public void Start();

    public void Stop()
    {
        task.Stop();
    }

    protected void finish()
    {
        if(finishAction != null)
        {
            finishAction();
        }
    }
}
