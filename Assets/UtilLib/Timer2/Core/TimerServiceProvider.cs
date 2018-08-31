using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimerServiceProvider
{
    private List<ITimer> timers = new List<ITimer>(16);
    /// <summary>
    /// 用于标示是否需要检测有无效的Timer
    /// </summary>
    private bool isCheckStopTimer = false;

    public void Update()
    {
        for (int i = 0, length = timers.Count; i < length; i++)
        {
            if (timers[i] != null && timers[i].runing)
            {
                timers[i].Cumulative();
            }
            else
            {
                //Debug.LogWarning(">> TimerServiceManager > Timer is null or stop > " + i);
                isCheckStopTimer = true;
            }
        }

        if (isCheckStopTimer)
        {
            isCheckStopTimer = false;
            for (int i = 0, length = timers.Count; i < length; i++)
            {
                if (timers[i] == null || !timers[i].runing)
                {
                    timers.RemoveAt(i);
                    i--;
                    length--;
                }
            }
        }
    }

    /// <summary>
    /// 启动Timer时，请使用Timer Start方法
    /// </summary>
    public void Add(ITimer timer)
    {
        if (timer == null || timers.Contains(timer))
        {
            return;
        }
        timers.Add(timer);
    }

    /// <summary>
    /// 移除Timer时，请使用Timer Stop方法
    /// </summary>
    public void Remove(ITimer timer)
    {
        this.isCheckStopTimer = true;
    }

}
