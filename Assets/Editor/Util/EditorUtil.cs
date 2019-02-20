using EditorTools;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class EditorUtil
{
    /// <summary>
    /// 当前选择目录下创建 .asset
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static ScriptableItem CreatAssetCurPath<T>(string name, System.Action<T, Dictionary<string, object>> callback = null, Dictionary<string, object> parameters = null) where T : ScriptableObject
    {
        Object[] o = Selection.GetFiltered<Object>(SelectionMode.Assets);

        if (o == null || o.Length == 0)
            return null;
        Object tar = o[0];

        string assetpath = AssetDatabase.GetAssetPath(tar);
        string filename = FileHelper.getFileNameAndTypeByPath(assetpath);
        if (filename.Contains("."))
        {
            //is file not fold
            assetpath = assetpath.Replace("/" + filename, null);
        }

        string path = assetpath+ "/" + name + ".asset";

        int num = 1;
        while(File.Exists(path))
        {
            path = assetpath + "/" + name + num+ ".asset";

            num++;
        }

        ScriptableItem item = new ScriptableItem(path);
        item.Creat<T>(callback, parameters);

        T t = item.Load<T>();
        t.SelectedObject();

        return item;
    }

    /// <summary>
    /// 获取子对象路径
    /// </summary>
    /// <param name="go"></param>
    /// <param name="root"></param>
    /// <returns></returns>
    public static string GetTranPath(GameObject go,GameObject root=null)
    {
        if (go == null || go==root)
            return null;

        string path = go.name;
        
        if(go.transform.parent!=null && go.transform.parent.gameObject!=root)
        {
            path = GetTranPath(go.transform.parent.gameObject, root)+"/"+path;
        }

        return path;
    }

    /// <summary>
    /// Occlusions the bake.
    /// </summary>
    /// <returns><c>true</c>, if bake was occlusioned, <c>false</c> otherwise.</returns>
    /// <param name="scene">Scene.</param>
    /// <param name="smallestHole">Smallest hole.</param>
    /// <param name="smallestOccluder">Smallest occluder.</param>
    /// <param name="backfaceThreshold">Backface threshold.</param>
    public static bool OcclusionBake(string scene,float smallestHole=0.25f,float smallestOccluder=5f,float backfaceThreshold=100f)
    {
        Scene s = EditorSceneManager.OpenScene(scene);

        StaticOcclusionCulling.smallestHole = smallestHole;
        StaticOcclusionCulling.smallestOccluder = smallestOccluder;
        StaticOcclusionCulling.backfaceThreshold = backfaceThreshold;

        bool success= StaticOcclusionCulling.Compute();

        EditorSceneManager.SaveScene(s);

        return success;
    }

    /// <summary>
    /// export file 
    /// </summary>
    /// <param name="assetPathName"></param>
    /// <param name="outputPath"></param>
    /// <param name="excludeSuffix"></param>
    public static void ExportPackage(string assetPathName, string outputPath,params string[] excludeSuffix)
    {
        if (string.IsNullOrEmpty(assetPathName) || string.IsNullOrEmpty(outputPath))
        {
            Debug.LogError("assetPathName or  outputPath is a empty path!");
            return;
        }

        string[] files = AssetDatabase.GetDependencies(assetPathName);
        List<string> packageFiles = new List<string>();

        bool include = true;
        foreach (var item in files)
        {
            include = true;
            if (excludeSuffix!=null)
            {
                foreach (var suffix in excludeSuffix)
                {
                    if (FileHelper.getFileTypeByPath(item) == suffix)
                    {
                        include = false;
                        break;
                    }
                }
            }

            if(include)
            {
                packageFiles.Add(item);
            }            
        }

        AssetDatabase.ExportPackage(packageFiles.ToArray(), outputPath);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// export asset in zip
    /// </summary>
    /// <param name="assetPathName"></param>
    /// <param name="outputPath"></param>
    /// <param name="excludeSuffix"></param>
    public static void ExportAssetZip(string assetPathName, string outputPath, params string[] excludeSuffix)
    {
        if (string.IsNullOrEmpty(assetPathName) || string.IsNullOrEmpty(outputPath))
        {
            Debug.LogError("assetPathName or  outputPath is a empty path!");
            return;
        }            

        if (FileHelper.getFileTypeByPath(outputPath) != ".zip")
        {
            Debug.LogError("outputPath is not a right path!");
            return;
        }           

        string[] files = AssetDatabase.GetDependencies(assetPathName);

        //add target files
        List<string> packageFiles = new List<string>();
        bool include = true;
        foreach (var item in files)
        {
            include = true;
            if (excludeSuffix != null)
            {
                string sfx = FileHelper.getFileTypeByPath(item);
                foreach (var suffix in excludeSuffix)
                {                    
                    if (sfx == suffix)
                    {
                        include = false;
                        break;
                    }
                }
            }

            if (include)
            {
                packageFiles.Add(item);
            }
        }
        
        //add mate files
        List<string> packageMateFiles = new List<string>();
        foreach (var item in packageFiles)
        {
            packageMateFiles.Add(item + ".meta");
        }
        packageFiles.AddRange(packageMateFiles);


        //set a temp path
        string tempFold = "~tempZip";

        foreach (var item in packageFiles)
        {
            string name = FileHelper.getFileNameAndTypeByPath(item);
            string tarPath = tempFold + "/" + name;
            FileHelper.copyFile(item, tarPath,true);
        }

        //creat zip
        ZipHelper.ZipFileDirectory(tempFold, outputPath);

        //delete temp fold
        FileUtil.DeleteFileOrDirectory(tempFold);
        
        AssetDatabase.Refresh();
    }
}
