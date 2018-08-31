
using UnityEditor;
namespace EditorTools
{
    public class FbxItem : ObjectBase, IFbxItem
    {
        public FbxItem(string _assetPath) : base(_assetPath) { }

        public void SetReadable(bool readable)
        {
            ModelImporter mi = importer as ModelImporter;
            mi.isReadable = readable;
        }
    }
}
