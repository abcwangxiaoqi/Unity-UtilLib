
using UnityEditor;
public interface IImport
{
    AssetImporter importer { get; }
    void SetAssetbundleName(string name);
    void Save();
}