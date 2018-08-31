using UnityEngine;
using System;

public class Timer : ITimer
{
    private Action mCallback = null;
    private float mInterval = 0.03f;
    private float mTime = 0f;
    private bool mRunning = false;

    /// <summary>
    /// 间隔为秒，且间隔时间不能低于帧间隔时间，低于帧间隔时间，按帧处理
    /// </summary>
    public Timer(Action _callback, float _interval = 1f)
    {
        mInterval = _interval;
        mCallback = _callback;
    }

    /// <summary>
    /// 间隔时间，单位秒
    /// </summary>
    public float interval
    {
        get { return mInterval; }
        set { mInterval = value; }
    }

    /// <summary>
    /// 是否在运行
    /// </summary>
    public bool runing
    {
        get { return mRunning; }
    }

    public virtual void Start()
    {
        if (mRunning || mCallback == null)
        {
            return;
        }

        mTime = 0;
        mRunning = true;
        TimerServiceManager.Instance.updateProvider.Add(this);
    }

    /// <summary>
    /// 停止后可以通过Start再次启动
    /// </summary>
    public virtual void Stop()
    {
        if (mRunning)
        {
            mRunning = false;
            TimerServiceManager.Instance.updateProvider.Remove(this);
        }
    }

    /// <summary>
    /// 销毁后，该Timer无法再次启动
    /// </summary>
    public virtual void Destroy()
    {
        Stop();
        mCallback = null;
    }

    public virtual void Cumulative()
    {
        mTime += Time.deltaTime;
        if (mTime > mInterval)
        {
            mTime -= mInterval;
            if(mTime < 0f)
            {
                mTime = 0f;
            }
            mCallback.Invoke();
        }
    }

}
