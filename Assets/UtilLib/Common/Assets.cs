using UnityEngine;

/// <summary>
/// 注：所有目录的使用最后都会带“/”，使用时不需在前面添加
/// </summary>
public class Assets
{
    /// <summary>
    /// unity发布webgl的文件夹名称
    /// </summary>
    public const string UNITY_ROOT = "static/unity/";

    static Assets()
    {
        // StreamingAssets Path
        StreamingAssetsPath = Application.streamingAssetsPath + "/";

        // StreamingAssets Url Path
        if (Application.platform == RuntimePlatform.Android)
        {
            StreamingAssetsUrlPath = StreamingAssetsPath;
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            int tempIndex = StreamingAssetsPath.IndexOf("#");
            if (tempIndex > -1)
            {
                StreamingAssetsUrlPath = StreamingAssetsPath.Substring(0, tempIndex) + UNITY_ROOT + "StreamingAssets/";
            }
            else
            {
                StreamingAssetsUrlPath = StreamingAssetsPath;
            }
        }
        else
        {
            StreamingAssetsUrlPath = "file:///" + StreamingAssetsPath;
        }

        //RuntimeAssets Path
        RuntimeAssetsPath = Application.persistentDataPath + "/";

        //AssetBundle Url Path
        RuntimeAssetsUrlPath = "file:///" + RuntimeAssetsPath;
    }


    /// <summary>
    /// 资源包目录，指StreamingAssets的路径
    /// </summary>
    public static string StreamingAssetsPath
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源包目录，指StreamingAssets路径并能使用WWW加载
    /// </summary>
    public static string StreamingAssetsUrlPath
    {
        get;
        private set;
    }

    /// <summary>
    /// 运行时资源路径，资源拷贝和下载存放的路径，指Application.persistentDataPath路径
    /// </summary>
    public static string RuntimeAssetsPath
    {
        get;
        private set;
    }

    /// <summary>
    /// 运行时资源路径，资源拷贝和下载存放的路径，指Application.persistentDataPath路径，并能使用WWW加载
    /// </summary>
    /// <returns></returns>
    public static string RuntimeAssetsUrlPath
    {
        get;
        private set;
    }

}