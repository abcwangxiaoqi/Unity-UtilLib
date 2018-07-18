using System.IO;
using System.Collections.Generic;

public class FileHelper
{
    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="filename">文件名 test.txt test.cs</param>
    /// <param name="content">写入内容</param>
    public static void CreatFile(string path, string filename, string content)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(filename)) return;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string filepath = path + "/" + filename;

        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }

        FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);

        StreamWriter sr = new StreamWriter(fs);
        sr.Write(content);//写入
        sr.Close();
        fs.Close();
    }


    /// <summary>
    /// 写入文本
    /// </summary>
    /// <param name="vFileName"></param>
    /// <param name="vContent"></param>
    public static void AppendToFile(string vFileName, string vContent)
    {
        StreamWriter SW;
        SW = File.AppendText(vFileName);
        SW.WriteLine(vContent);
        SW.Close();
    }

    /// <summary>
    /// 读取文本
    /// </summary>
    /// <param name="vFileName"></param>
    /// <returns></returns>
    public static string ReadFromFile(string vFileName)
    {
        string S = string.Empty;
        if (File.Exists(vFileName))
        {
            StreamReader SR;
            SR = File.OpenText(vFileName);
            S = SR.ReadLine();
            SR.Close();
        }
        return S;
    }
    /// <summary>
    /// 创建文本
    /// </summary>
    /// <param name="vFileName"></param>
    /// <param name="vContent"></param>
    public static void WriteToFile(string vFileName, string vContent)
    {
        StreamWriter SW;
        SW = File.CreateText(vFileName);
        SW.WriteLine(vContent);
        SW.Close();
    }

    /// <summary>
    /// 根据路径取得文件名.后缀
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    public static string getFileNameAndTypeByPath(string fullPath)
    {
        string filename = Path.GetFileName(fullPath);//Default.aspx
        return filename;
    }

    /// <summary>
    /// 根据路径取得文件名
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    public static string getFileNameNoTypeByPath(string fullPath)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);// Default
        return fileNameWithoutExtension;
    }

    /// <summary>
    /// get file's suffix
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    public static string getFileTypeByPath(string fullPath)
    {
        string extension = Path.GetExtension(fullPath);
        return extension;
    }

    /// <summary>
    /// 重命名文件
    /// </summary>
    /// <param name="sourcename"></param>
    /// <param name="targetname"></param>
    public static void Renamefile(string sourcename, string targetname)
    {
        if (string.IsNullOrEmpty(sourcename) ||
            string.IsNullOrEmpty(targetname))
            return;

        if (File.Exists(sourcename))
        {
            File.Move(sourcename, targetname);
        }
    }

    /// <summary>
    /// 拷贝文件
    /// </summary>
    /// <param name="source">源文件</param>
    /// <param name="taget">拷贝路径文件</param>
    /// <param name="recover">是否覆盖</param>
    public static void copyFile(string source, string taget, bool recover)
    {
        if (string.IsNullOrEmpty(source) ||
           string.IsNullOrEmpty(taget))
            return;

        string fold = PathHelper.GetFoldByFullName(taget);
        if (!Directory.Exists(fold))
        {
            Directory.CreateDirectory(fold);
        }
        File.Copy(source, taget, recover);
    }

    /// <summary>
    /// 是否是音乐文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool isMusic(string path)
    {
        string[] musictype = { ".mp3", ".wav" };
        List<string> musicType = new List<string>(musictype);
        int index = path.LastIndexOf(".");
        string type = path.Substring(index);
        return musicType.Contains(type);
    }

    /// <summary>
    /// 是否是图片
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool isPicture(string path)
    {
        string[] pictype = { ".png", ".psd", ".jpg" };
        List<string> picType = new List<string>(pictype);
        int index = path.LastIndexOf(".");
        string type = path.Substring(index).ToLower();
        return picType.Contains(type);
    }


    /// <summary>
    /// 取得文件父级文件名
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string getParentFileNameByPath(string path)
    {
        path = path.Replace(@"\", "/");

        int f = lastIndexOF(path, "/", 1);
        if (f == -1)
        {
            return "";
        }

        int t = lastIndexOF(path, "/", 2);
        string filename = path.Substring(t + 1, f - t - 1);
        return filename;
    }
    /// <summary>
    /// 字符串倒数指定字符索引
    /// </summary>
    /// <param name="str">string</param>
    /// <param name="fStr">value</param>
    /// <param name="num">倒数第几</param>
    /// <returns></returns>
    static int lastIndexOF(string str, string fStr, int num)
    {
        int indexOf = -1;
        for (int i = 0; i < num; ++i)
        {
            if (i == 0)
            {
                indexOf = str.LastIndexOf(fStr);
            }
            else
            {
                indexOf = str.LastIndexOf(fStr, indexOf - 1);
            }
        }
        return indexOf;
    }

    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static void CopyDirectory(string from, string to, bool recover)
    {
        #region 创建文件
        if (!Directory.Exists(to))
        {
            Directory.CreateDirectory(to);
        }        
        else
        {
            if(recover)
            {
                Directory.Delete(to, true);
                Directory.CreateDirectory(to);
            }
        }
        #endregion

        #region 导入文件
        List<string> files = PathHelper.getAllChildFiles(from, "");
        for (int i = 0; i < files.Count; i++)
        {
            string target = files[i].Replace(from, to);

            //Debug.Log("From=" + files[i]);
            //Debug.Log("To=" + target);

            string fold = PathHelper.GetFoldByFullName(target);
            if (!Directory.Exists(fold))
            {
                Directory.CreateDirectory(fold);
            }

            FileHelper.copyFile(files[i], target, recover);
        }
        #endregion
    }
}