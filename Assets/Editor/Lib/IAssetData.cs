
using UnityEditor;
using UnityEngine;
namespace EditorTools
{
    public interface IAssetData
    {
        T Load<T>() where T : Object;
        Object Load();
        void Import();
        void CreatAsset(Object obj);
        void SaveAsset(Object obj);
        string[] GetDependencies();
        string path { get; }
        void ImportAsset();
        void WriteImportSettingsIfDirty();
        void ImportAsset(ImportAssetOptions Options);
        void AddObjectToAsset(Object obj);
        void DeleteAsset();
    }
}