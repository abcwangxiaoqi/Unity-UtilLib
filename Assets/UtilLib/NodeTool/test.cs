using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using NodeTool;

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



public class testShareData: SharedData
{
    public int state = 0;
}

public class testShareData2: SharedData
{
    public int state = 0;
}

//=========================================
//[Name("实体1")]
public class Enity1 : BaseNode
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
public class Enity2 : BaseNode
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
public class Enity3 : BaseNode
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
public class Enity4 : BaseNode
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

