using System;

/// <summary>
/// Mono LateUpdate帧计时器，在LateUpdate方法中每帧调用
/// </summary>
public class LateUpdateTimer : UpdateTimer
{

    public LateUpdateTimer(Action _callback, int _interval = 1) : base(_callback, _interval) { }

    /// <summary>
    /// 启动方法，Timer必须调用该方法启动
    /// </summary>
    public override void Start()
    {
        if (mRunning || mCallback == null)
        {
            return;
        }

        mRunning = true;
        TimerServiceManager.Instance.lateUpdateProvider.Add(this);
    }

    /// <summary>
    /// 停止后可以通过Start再次启动
    /// </summary>
    public override void Stop()
    {
        if (mRunning)
        {
            mRunning = false;
            TimerServiceManager.Instance.lateUpdateProvider.Remove(this);
        }
    }
}
