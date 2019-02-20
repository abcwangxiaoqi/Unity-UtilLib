using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace WF
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        private readonly string _name;

        public string Name
        {
            get { return _name; }
        }

        public NameAttribute(string name)
        {
            _name = name;
        }
    }
}



public enum EntityState
{
    Idle,//待机状态 可触发执行
    Wait,//等待确认状态 
    Finished//完成状态 可进入下一节点
}

public abstract class SharedData
{
    public string wfName;
}

public class testShareData: SharedData
{
    public int state = 0;
}

public class testShareData2: SharedData
{
    public int state = 0;
}

public abstract class BaseEntity
{
    public EntityState State { get; private set; }

    protected SharedData shareData;
    public BaseEntity(SharedData data)
    {
        shareData = data;
    }

    public void run()
    {
        State = EntityState.Wait;
        execute();
    }

    //节点完全通过后 状态重置
    //由主MainWF检测到Finished状态后 主动调用
    public void reset()
    {
        State = EntityState.Idle;
    }

    protected abstract void execute();

    //异步流程 执行到一半 
    //外界触发被动停止
    public virtual void stop()
    { }

    //确定执行完后 执行该方法
    protected void finish()
    {
        State = EntityState.Finished;
    }
}

public abstract class BaseCondition
{
    protected SharedData shareData;
    public BaseCondition(SharedData data)
    {
        shareData = data;
    }

    public abstract bool justify();
}

//=========================================
//[Name("实体1")]
public class Enity1 : BaseEntity
{
    public Enity1(SharedData data) : base(data) { }

    protected override void execute()
    {
        Debug.Log("Enity1");

        (shareData as testShareData).state = 3;
        finish();
    }
}

//[Name("实体2")]
public class Enity2 : BaseEntity
{
    public Enity2(SharedData data) : base(data) { }

    protected override void execute()
    {
        Debug.Log("Enity2");
        (shareData as testShareData).state = 10;
        finish();
    }
}

//[Name("实体3")]
public class Enity3 : BaseEntity
{
    public Enity3(SharedData data) : base(data) { }

    protected override void execute()
    {
        Debug.Log("Enity3");
        (shareData as testShareData).state = 20;
        finish();
    }
}

//[Name("实体4")]
public class Enity4 : BaseEntity
{
    public Enity4(SharedData data) : base(data) { }

    protected override void execute()
    {
        Debug.Log("Enity4");
        (shareData as testShareData).state = 30;
        finish();
    }
}

//=============================================

//[Name("条件1")]
public class Condition1 : BaseCondition
{
    public Condition1(SharedData data) : base(data) { }

    public override bool justify()
    {
        Debug.Log("条件1");
        return (shareData as testShareData).state == 1;
    }
}

//[Name("条件2")]
public class Condition2 : BaseCondition
{
    public Condition2(SharedData data) : base(data) { }

    public override bool justify()
    {
        Debug.Log("条件2");
        return (shareData as testShareData).state == 2;
    }
}

//[Name("条件3")]
public class Condition3 : BaseCondition
{
    public Condition3(SharedData data) : base(data) { }

    public override bool justify()
    {
        Debug.Log("条件3");
        return (shareData as testShareData).state == 3;
    }
}

//[Name("条件4")]
public class Condition4 : BaseCondition
{
    public Condition4(SharedData data) : base(data) { }

    public override bool justify()
    {
        Debug.Log("条件4");
        return (shareData as testShareData).state == 4;
    }
}

