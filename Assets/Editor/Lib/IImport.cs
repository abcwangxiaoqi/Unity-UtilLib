
using UnityEditor;
namespace EditorTools
{
    public interface IImport
    {
        AssetImporter importer { get; }
        void SetAssetbundleName(string name);
        void Save();
    }
}