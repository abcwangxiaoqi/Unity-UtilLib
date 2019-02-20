using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WindowData:ScriptableObject
{
    public string shareData;

    public List<WindowDataEntity> entitylist = new List<WindowDataEntity>();
    public List<WindowDataRouter> routerlist = new List<WindowDataRouter>();

    public WindowDataBase Get(int id)
    {
        WindowDataBase result = entitylist.Find((WindowDataEntity e) => 
        {
            return e.id == id;
        });

        if (result != null)
            return result;

        result = routerlist.Find((WindowDataRouter e) =>
        {
            return e.id == id;
        });

        return result;
    }

    public WindowDataEntity GetEntrance()
    {
        return entitylist.Find((WindowDataEntity e) => 
        {
            return e.isEntrance == true;
        });
    }    
}

public enum WindowType
{
    Entity,
    Router
}

public class WindowDataBase
{    
    public int id;
    public string name;
    public Vector2 position;

    public virtual WindowType type
    {
        get
        {
            return WindowType.Entity;
        }
    }

}

[Serializable]
public class WindowDataEntity : WindowDataBase
{
    public bool isEntrance;
    public string className;

    //只存id 存实例会出现 死循环的情况
    public int next=-1;

    public override WindowType type => WindowType.Entity;
}

[Serializable]
public class WindowDataRouter : WindowDataBase
{
    public List<WindowDataCondition> conditions = new List<WindowDataCondition>();
    public int defaultEntity = -1;

    public override WindowType type => WindowType.Router;
}

[Serializable]
public class WindowDataCondition
{
    public string className;
    public int entity = -1;
}