using ScriptNodeFlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enity1 : Node
{
    public Enity1(SharedData data) : base(data) { }

    protected override void execute()
    {
        Debug.Log("Enity1");

        //get share data and you can modify it
        (shareData as testShareData).state = 3;

        //call finish method when you're sure finished completely
        finish();
    }
}

public class Enity6 : Node
{
    public Enity6(SharedData data) : base(data) { }

    protected override void execute()
    {
        Debug.Log("Enity1");

        (shareData as testShareData).state = 3;
        finish();
    }
}

public class Enity2 : Node
{
    public Enity2(SharedData data) : base(data) { }

    protected override void execute()
    {
        Debug.Log("Enity2");
        (shareData as testShareData).state = 10;
        finish();
    }
}

public class Enity3 : Node
{
    public Enity3(SharedData data) : base(data) { }

    protected override void execute()
    {
        Debug.Log("Enity3");
        (shareData as testShareData).state = 20;
        finish();
    }
}

public class Enity4 : Node
{
    public Enity4(SharedData data) : base(data) { }

    protected override void execute()
    {
        Debug.Log("Enity4");
        (shareData as testShareData).state = 30;
        finish();
    }
}
