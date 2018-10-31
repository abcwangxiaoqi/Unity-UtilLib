using UnityEngine;
using System.Collections;
using UnityEditor;

public static class PackageUtil
{
    /// <summary>
    /// 导出apk
    /// </summary>
    /// <param name="path">导出路径</param>
    public static string PackageApk(string[] secens, string path)
    {
        if (UnityEngine.Application.platform != RuntimePlatform.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android,BuildTarget.Android);
        }

        string error=BuildPipeline.BuildPlayer(secens, path, BuildTarget.Android, BuildOptions.None);
        return error;
    }

    /// <summary>
    /// 导出android工程文件
    /// </summary>
    /// <param name="secens">场景</param>
    /// <param name="path">导出路径</param>
    public static string ExportAndroidProject(string[] secens, string path)
    {
        if (UnityEngine.Application.platform != RuntimePlatform.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTargetGroup.Android,BuildTarget.Android);
        }

        string error=BuildPipeline.BuildPlayer(secens, path, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
        return error;
    }

    /// <summary>
    /// 导出ios
    /// </summary>
    /// <param name="path">导出路径</param>
    public static string PackageIOS(string[] secens, string path)
    {
        if (UnityEngine.Application.platform != RuntimePlatform.OSXEditor)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS,BuildTarget.iOS);
        }

        string error = BuildPipeline.BuildPlayer(secens, path, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
        return error;
    }
}
