using System.Collections;
using UnityEngine;
/// <summary>
/// 定时调用，该定时采用世界平行时间
/// </summary>
public class NativeTimer
{
    WaitForSeconds time = null;
    bool loop = true;
    int lpTime = 0;
    int lpindex = 0;
    SimpleTask task = null;
    CallBack callback = null;

    /// <summary>
    /// 时间单位秒
    /// </summary>
    public NativeTimer(CallBack _callback, float t, bool loop = true,int lpTime=0)
    {
        callback = _callback;
        time = new WaitForSeconds(t);
        this.loop = loop;
        this.lpTime = lpTime;
    }

    public void Start()
    {
        Stop();
        task = new SimpleTask(timeStart(), false);
        task.Start();
    }

    IEnumerator timeStart()
    {
        if (loop)
        {
            if(lpTime>0)
            {
                while (lpindex>lpTime)
                {
                    yield return time;
                    if (callback != null)
                    {
                        callback.Invoke();
                    }
                    lpindex++;
                }
            }
            else
            {
                while (true)
                {
                    yield return time;
                    if (callback != null)
                    {
                        callback.Invoke();
                    }
                }
            }
        }
        else
        {
            yield return time;
            if (callback != null)
            {
                callback.Invoke();
            }
        }
        task = null;
    }

    public void Stop()
    {
        lpindex = 0;
        if (task != null)
        {
            task.Stop();
            task = null;
        }
    }
}

