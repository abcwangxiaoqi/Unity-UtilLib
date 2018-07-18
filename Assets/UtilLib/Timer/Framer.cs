using UnityEngine;
using System.Collections;

/// <summary>
/// 每帧调用 1s=100帧
/// </summary>
public class Framer
{
    bool loop = true;
    SimpleTask task = null;
    CallBack callback = null;
    int _frame = 1;
    public Framer(CallBack _callback,int frame=1, bool _loop = true)
    {
        this.callback = _callback;
        this.loop = _loop;
        if(frame>0)
        {
            _frame = frame;
        }
    }

    public void Start()
    {
        Stop();
        task = new SimpleTask(FrameStart(), true);
        task.Start();
    }

    IEnumerator FrameStart()
    {
        if (loop)
        {
            while (true)
            {
                for (int i = 0; i < _frame; i++)
                {
                    yield return null;
                }
               
                if (callback != null)
                {
                    callback.Invoke();
                }
            }
        }
        else
        {
            for (int i = 0; i < _frame; i++)
            {
                yield return null;
            }

            if (callback != null)
            {
                callback.Invoke();
            }
        }
        task = null;
    }

    public void Stop()
    {
        if (task != null)
        {
            task.Stop();
            task = null;
        }
    }
}
