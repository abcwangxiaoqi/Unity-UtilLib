using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public interface IEntity
{
    void execute();
}

public class EEnity: IEntity
{
    public string ClassName;
    public IEntity next;

    public void execute()
    {

    }
}

public class Router: IEntity
{
    public class Conditiion
    {
        public string ClassName;
        public EEnity next;
    }

    public List<Conditiion> conditions;
    public EEnity defaultNext;

    public void execute()
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            Type t = Type.GetType(conditions[i].ClassName);
            ICondition c = Activator.CreateInstance(t) as ICondition;
        }
    }
}

public class WFMain
{
    IMMEntity Enter;

    public void SetEnter(IMMEntity enter)
    {
        Enter = enter;
    }

    public void Start()
    {

    }

    public void Stop()
    { }
}


public class MMEntity: IMMEntity
{
    

    //
    void star()
    {
        //notify();
    }


}

public class new1 : IMMEntity
{


    //
    void star()
    {
        //notify();
    }


}

public class MMRouter
{

}

public abstract class BaseMMEntity
{

}

public interface IMMEntity
{

}

public interface ICondition
{
    bool justify();
}

public class MMCondition: ICondition
{
    public bool justify()
    {
        return true;
    }
}
