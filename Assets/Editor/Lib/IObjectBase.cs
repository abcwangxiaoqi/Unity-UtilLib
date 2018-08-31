namespace EditorTools
{
    public interface IObjectBase : IAssetData, IImport
    {
        string Name { get; }
        string Type { get; }
    }
}