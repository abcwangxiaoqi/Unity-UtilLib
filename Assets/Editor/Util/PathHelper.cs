using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class PathHelper
{
    /// <summary>
    /// 得到unity项目内路径
    /// </summary>
    /// <param name="_fullPath"></param>
    /// <returns></returns>
    public static string GetRelativeAssetPath(string _fullPath)
    {

        _fullPath = _fullPath.Replace("\\", "/");
        int idx = _fullPath.IndexOf("Assets");
        string assetRelativePath = _fullPath.Substring(idx);
        return assetRelativePath;
    }

    /// <summary>
    /// 重命名文件夹
    /// </summary>
    /// <param name="sourcename"></param>
    /// <param name="targetname"></param>
    public static void Renamedic(string sourcename, string targetname)
    {
        if (string.IsNullOrEmpty(sourcename) ||
           string.IsNullOrEmpty(targetname))
            return;
        if (Directory.Exists(sourcename))
        {
            Directory.Move(sourcename, targetname);
        }
    }

    /// <summary>
    /// 根据文件路径 取得文件夹路径
    /// </summary>
    /// <param name="fullname"></param>
    /// <returns></returns>
    public static string GetFoldByFullName(string fullname)
    {
        fullname = fullname.Replace("\\", "/");
        int index = fullname.LastIndexOf("/");
        if (index > 0)
        {
            return fullname.Substring(0, index + 1);
        }
        return null;
    }

    /// <summary>
    /// 删除目录下指定类型文件
    /// </summary>
    /// <param name="path"></param>
    public static void deleteFilebySuffix(string path, string suffix)
    {
        List<string> files = getAllChildFiles(path, suffix);
        foreach (string item in files)
        {
            File.Delete(item);
        }
    }

    /// <summary>
    /// 得到路径下所有指定文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="suffix">txt xlsx lua</param>
    /// <param name="files"></param>
    /// <returns></returns>
    public static List<string> getAllChildFiles(string path, string suffix = "lua", List<string> files = null)
    {
        if (files == null) files = new List<string>();

        if (!Directory.Exists(path))
        {
            return files;
        }

        addFiles(path, suffix, files);
        string[] dires = Directory.GetDirectories(path);
        foreach (string dirp in dires)
        {
            getAllChildFiles(dirp, suffix, files);
        }
        return files;
    }
    static void addFiles(string direPath, string suffix, List<string> files)
    {
        string[] fileMys = Directory.GetFiles(direPath);
        string[] suffixs = suffix.Split(',');
        foreach (string f in fileMys)
        {
            foreach (var s in suffixs)
            {
                if (f.ToLower().EndsWith(s.ToLower()))
                {
                    files.Add(f);
                }
            }
        }
    }

}
