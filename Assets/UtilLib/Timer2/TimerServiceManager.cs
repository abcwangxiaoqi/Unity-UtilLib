using UnityEngine;
using System.Collections.Generic;

public class TimerServiceManager : TManager<TimerServiceManager>
{
    private TimerServiceProvider mUpdateProvider = new TimerServiceProvider();
    private TimerServiceProvider mLateUpdateProvider = new TimerServiceProvider();


    private void Update()
    {
        mUpdateProvider.Update();
    }

    private void LateUpdate()
    {
        mLateUpdateProvider.Update();
    }

    /// <summary>
    /// Update中执行的Timer服务提供
    /// </summary>
    public TimerServiceProvider updateProvider { get { return mUpdateProvider; } }

    /// <summary>
    /// LateUpdate中执行的Timer服务提供
    /// </summary>
    public TimerServiceProvider lateUpdateProvider { get { return mLateUpdateProvider; } }

}
