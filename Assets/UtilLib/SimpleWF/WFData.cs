using System;
using System.Collections.Generic;

public class WindowData
{
    public List<WindowDataEnity> enitylist = new List<WindowDataEnity>();
    public List<WindowDataRouter> routerlist = new List<WindowDataRouter>();
}


public class WindowDataBase
{
    public float x;
    public float y;
    public int id;
    public string name;
}

public class WindowDataEnity : WindowDataBase
{
    public bool entry;
    public string className;

    //nextEnity 和 nextCondition只有一个有值
    public WindowDataEnity nextEnity;
    public WindowDataRouter nextRouter;

}

public class WindowDataRouter : WindowDataBase
{
    public List<WindowDataCondition> conditions = new List<WindowDataCondition>();
    public WindowDataEnity defaultEnity;
}

public class WindowDataCondition
{
    public string className;
    public WindowDataEnity enity;
}