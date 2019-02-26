using Object = UnityEngine.Object;

public interface IObjectParse
{
    Object Create(string fold, string filename);
    object Parse(Object source);
    void Save(Object source,object obj);
}