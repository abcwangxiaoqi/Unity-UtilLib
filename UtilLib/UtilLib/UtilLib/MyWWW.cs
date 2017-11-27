using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyWWW : IEnumerator, System.IDisposable
{
    float _timeOut = -1f;
    WWW _www = null;
    float _timePassed = 0f;
    public MyWWW(string url,float timeout=-1)
    {
        _www = new WWW(url);
        _timeOut = timeout;
        _timePassed = 0;
    }

    public MyWWW(string url, byte[] postData,float timeout = -1)
    {
        _www = new WWW(url, postData);
        _timeOut = timeout;
        _timePassed = 0;
    }

    public MyWWW(string url, WWWForm form, float timeout = -1)
    {
        _www = new WWW(url,form);
        _timeOut = timeout;
        _timePassed = 0;
    }

    public MyWWW(string url, byte[] postData, Dictionary<string,string> headers, float timeout = -1)
    {
        _www = new WWW(url, postData, headers);
        _timeOut = timeout;
        _timePassed = 0;
    }

    public string text
    {
        get
        {
            return _www.text;
        }
    }

    public Texture texture
    {
        get
        {
            return _www.texture;
        }
    }

    public AssetBundle assetBundle
    {
        get
        {
            return _www.assetBundle;
        }
    }

    public AudioClip audioClip
    {
        get
        {
            return _www.audioClip;
        }
    }

    public byte[] bytes
    {
        get
        {
            return _www.bytes;
        }
    }

    public string error
    {
        get
        {
            if (timeoutFlag)
            {
                return "time out";
            }
            else
            {
                return _www.error;
            }            
        }
    }

    public bool isDone
    {
        get
        {
            return _www.isDone;
        }
    }

    public string url
    {
        get
        {
            return _www.url;
        }
    }

    public float progress
    {
        get
        {
            return _www.progress;
        }
    }

    bool disposeFlag = false;

    bool timeoutFlag = false;

    object IEnumerator.Current
    {
        get { return _www; }
    }

    bool IEnumerator.MoveNext()
    {
        _timePassed += Time.deltaTime;

        if (_timeOut > 0.0f && _timePassed > _timeOut)
        {
            timeoutFlag = true;
            return false;
        }
        bool result;

        if (_www == null)
        {
            result = false;
        }
        else
        {            
            if(!disposeFlag)
            {
                result = _www.isDone == false;
            }         
            else
            {
                result = false;
            }   
        }
        return result;
    }

    void IEnumerator.Reset()
    {
    }

    public void Dispose()
    {
        disposeFlag = true;
        _www.Dispose();
    }
}
