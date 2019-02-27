using UnityEngine;
using System.Collections;
using UnityEditor;

public static class PackageUtil
{
    /// <summary>
    /// 导出apk
    /// </summary>
    /// <param name="path">导出路径</param>
    public static void PackageApk(string[] secens, string path)
    {
        if (UnityEngine.Application.platform != RuntimePlatform.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }
        BuildPipeline.BuildPlayer(secens, path, BuildTarget.Android, BuildOptions.None);
    }

    /// <summary>
    /// 导出android工程文件
    /// </summary>
    /// <param name="secens">场景</param>
    /// <param name="path">导出路径</param>
    public static void ExportAndroidProject(string[] secens, string path)
    {
        if (UnityEngine.Application.platform != RuntimePlatform.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        BuildPipeline.BuildPlayer(secens, path, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    /// <summary>
    /// 导出ios
    /// </summary>
    /// <param name="path">导出路径</param>
    public static void PackageIOS(string[] secens, string path)
    {
        if (UnityEngine.Application.platform != RuntimePlatform.OSXEditor)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        }

        BuildPipeline.BuildPlayer(secens, path, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
    }
}
