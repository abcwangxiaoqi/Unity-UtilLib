using System;

/// <summary>
/// Mono Update帧计时器，在Update方法中每帧调用
/// </summary>
public class UpdateTimer : ITimer
{
    protected Action mCallback = null;
    protected bool mRunning = false;


    protected int interval = 1;
    private int mCurrentInterval = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_callback"></param>
    /// <param name="_interval">根据需求控制频率 频率太快 会引起过多gc</param>
    public UpdateTimer(Action _callback,int _interval = 5)
    {
        mCallback = _callback;

        interval = _interval;
        mCurrentInterval = 0;
    }

    /// <summary>
    /// 是否在运行
    /// </summary>
    public bool runing
    {
        get { return mRunning; }
    }

    /// <summary>
    /// 启动方法，Timer必须调用该方法启动
    /// </summary>
    public virtual void Start()
    {
        if (mRunning || mCallback == null)
        {
            return;
        }

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
        mCurrentInterval++;
        if(mCurrentInterval == interval)
        {
            mCurrentInterval = 0;
            mCallback.Invoke();
        }
        //if (UnityEngine.Time.frameCount % interval == 0)
        //{
        //    mCallback.Invoke();
        //}        
    }
}