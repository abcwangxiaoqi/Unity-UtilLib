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
        Type t = Type.GetType(ClassName);
        BaseMMEntity entity = Activator.CreateInstance(t) as BaseMMEntity;
        entity.OnFinish += () => 
        {
            if(next!=null)
            {
                next.execute();
            }
            else
            {
                //整个流程结束 抛出
            }
        };

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
        bool conditonsFlag = false;
        for (int i = 0; i < conditions.Count; i++)
        {
            Type t = Type.GetType(conditions[i].ClassName);
            MCondition c = Activator.CreateInstance(t) as MCondition;

            if(c.execute())
            {
                conditonsFlag = true;
                break;
            }
        }

        //条件列表都不符合 执行默认
        if(!conditonsFlag)
        {
            if(defaultNext != null)
            {
                defaultNext.execute();
            }
            else
            {
                //整个流程结束 抛出
            }
        }
    }
}

public class WFMain
{
    EEnity Enter;

    //成功true  失败false
    public event Action<bool> OnFinish;

    public bool Running { get; private set; }

    public void SetEnter(EEnity enter)
    {
        Running = true;

        Enter = enter;
    }

    public void Start()
    {
        Enter.execute();
    }

    public void Stop()
    {
        Running = false;
    }
}


public class MMEntity: BaseMMEntity
{

    public override void execute()
    {
        throw new NotImplementedException();
    }


}

public class new1 : BaseMMEntity
{


    public override void execute()
    {
        throw new NotImplementedException();
    }


}

public abstract class BaseMMEntity
{
    public event Action OnFinish;
    
    public abstract void execute();

    protected void Finish()
    {
        OnFinish.Invoke();
    }

}

public abstract class MCondition
{

    IEntity next;

    public bool execute()
    {
        bool res = justify();

        if(res)
        {
            next.execute();
        }

        return res;
    }

    protected abstract bool justify();
}

public class MMCondition: MCondition
{
    protected override bool justify()
    {
        //判断条件
        return true;
    }
}
