using UnityEngine;
using System;

public class AppCommon : MonoBehaviour
{
    public Action updateAction;
    public Action fixedupdateAction;
    public Action lateupdateAction;
    public Action<bool> focusAction;
    public Action<bool> pauseAction;
    public Action quitAction;

    void Update()
    {
        if (updateAction == null)
            return;
        updateAction.Invoke();
    }

    void FixedUpdate()
    {
        if (fixedupdateAction == null)
            return;
        fixedupdateAction.Invoke();
    }

    void LateUpdate()
    {
        if (lateupdateAction == null)
            return;
        lateupdateAction.Invoke();
    }

    void OnApplicationFocus(bool focus)
    {
        if (focusAction == null)
            return;
        focusAction.Invoke(focus);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseAction == null)
            return;
        pauseAction.Invoke(pauseStatus);
    }

    void OnApplicationQuit()
    {
        if (quitAction == null)
            return;
        quitAction.Invoke();
    }
}
