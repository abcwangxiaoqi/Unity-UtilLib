
using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

public class LoadData
{
    public string url;
    public Dictionary<string, string> paramters;
    public Dictionary<string, string> header;
    public Action callback;
}

public class ResponseData
{
    public string url;
    public string error;
    public bool succes;
    public string text;
    public byte[] bytes;
}

//https://www.jb51.net/article/142945.htm

public class LoaderTask
{
    bool state = true;
    Task task = null;
    public LoaderTask()
    {
        task = Task.Factory.StartNew(Loop);
    }

    void Loop()
    {
        while (state)
        {
            Thread.Sleep(100);

            //pull data from contianer

        }
    }

    public string Post(string url)
    {
        string result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        //获取内容 
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        return result;
    }

    public string Post(string url, Dictionary<string, string> dic)
    {
        string result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded";
        #region 添加Post 参数 
        StringBuilder builder = new StringBuilder();
        int i = 0;
        foreach (var item in dic)
        {
            if (i > 0)
                builder.Append("&");
            builder.AppendFormat("{0}={1}", item.Key, item.Value);
            i++;
        }
        byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
        req.ContentLength = data.Length;
        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
        }
        #endregion
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        //获取响应内容 
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        return result;
    }

    public string Get(string url)
    {
        string result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        try
        {
            //获取内容 
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
        }
        finally
        {
            stream.Close();
        }
        return result;
    }

    public string Get(string url, Dictionary<string, string> dic)
    {
        string result = "";
        StringBuilder builder = new StringBuilder();
        builder.Append(url);
        if (dic.Count > 0)
        {
            builder.Append("?");
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
        }
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
        //添加参数 
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        try
        {
            //获取内容 
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
        }
        finally
        {
            stream.Close();
        }
        return result;
    }

    public void Start()
    {
        if (null == task)
            return;

        task.Start();
    }

    public void Stop()
    {
        if (null == task)
            return;

        task.Dispose();
    }
}
