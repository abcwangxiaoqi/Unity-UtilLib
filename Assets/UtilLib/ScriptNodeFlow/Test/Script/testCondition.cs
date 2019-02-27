using ScriptNodeFlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition1 : RouterCondition
{
    public Condition1(SharedData data) : base(data) { }

    public override bool justify()
    {
        Debug.Log("Condition1");

        //get shared data
        testShareData data = shareData as testShareData;


        return data.state == 1;
    }
}

public class Condition2 : RouterCondition
{
    public Condition2(SharedData data) : base(data) { }

    public override bool justify()
    {
        Debug.Log("Condition2");
        return (shareData as testShareData).state == 2;
    }
}

public class Condition3 : RouterCondition
{
    public Condition3(SharedData data) : base(data) { }

    public override bool justify()
    {
        Debug.Log("Condition3");
        return (shareData as testShareData).state == 3;
    }
}

public class Condition4 : RouterCondition
{
    public Condition4(SharedData data) : base(data) { }

    public override bool justify()
    {
        Debug.Log("Condition4");
        return (shareData as testShareData).state == 4;
    }
}
