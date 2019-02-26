using System;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ParseNameAttribute : Attribute
{
    private readonly string _name;

    public string Name
    {
        get { return _name; }
    }

    public ParseNameAttribute(string name)
    {
        _name = name;
    }
}