
using UnityEditor;
using UnityEngine;
using System.IO;
namespace EditorTools
{
    public class ObjectBase : IObjectBase
    {
        string assetPath = "";
        string name = "";
        string type = "";
        public ObjectBase(string _assetPath)
        {
            assetPath = PathHelper.GetRelativeAssetPath(_assetPath);
            name = FileHelper.getFileNameNoTypeByPath(assetPath);
            type = FileHelper.getFileTypeByPath(assetPath);
        }

        AssetImporter _importer;
        public AssetImporter importer
        {
            get
            {
                if (_importer == null)
                {
                    _importer = AssetImporter.GetAtPath(assetPath);
                }
                return _importer;
            }
        }

        public void SetAssetbundleName(string name)
        {
            importer.assetBundleName = name;
        }

        public Object Load()
        {
            Object go = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            return go;
        }

        public T Load<T>() where T : Object
        {
            T go = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return go;
        }

        public void Import()
        {
            AssetDatabase.ImportAsset(assetPath);
        }

        public void Save()
        {
            importer.SaveAndReimport();
        }

        public string Name
        {
            get { return name; }
        }


        public string[] GetDependencies()
        {
            return AssetDatabase.GetDependencies(assetPath);
        }


        public string Type
        {
            get { return type; }
        }


        public string path
        {
            get { return assetPath; }
        }


        public void CreatAsset(Object obj)
        {
            string fold = PathHelper.GetFoldByFullName(assetPath);
            if (!Directory.Exists(fold))
            {
                Directory.CreateDirectory(fold);
            }

            AssetDatabase.CreateAsset(obj, assetPath);
            AssetDatabase.SaveAssets();
        }


        public void ImportAsset()
        {
            AssetDatabase.ImportAsset(path);
        }

        public void WriteImportSettingsIfDirty()
        {
            AssetDatabase.WriteImportSettingsIfDirty(path);
        }

        public void ImportAsset(ImportAssetOptions Options)
        {
            AssetDatabase.ImportAsset(path, Options);
        }

        public void AddObjectToAsset(Object obj)
        {
            AssetDatabase.AddObjectToAsset(obj, path);
        }


        public void DeleteAsset()
        {
            AssetDatabase.DeleteAsset(path);
        }

        public void SaveAsset(Object obj)
        {
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }
    }
}