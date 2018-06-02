using UnityEngine;
using System.Collections;
using IMR;

public class testInter : Interaction<testModel, testRender, testRender2>
{
    public void creat(string name)
    {
        sendCmdWithParamters(testModel.CMD_CREAT,name,10);
    }

    public void destroy()
    {
        sendCmd(testModel.CMD_DESTROY);
    }    
}

public class testModel : DataModel
{
    public const string CMD_CREAT = "cmd_creat";
    public const string CMD_DESTROY = "cmd_destroy";

    public GameObject obj;

    public override void Dispose()
    {
        base.Dispose();
        UnityEngine.Object.Destroy(obj);
    }
}

public class testRender : DataRender<testModel>
{
    public override void excuteCmd(string cmd)
    {
        base.excuteCmd(cmd);

        if(cmd==testModel.CMD_DESTROY)
        {
            if(model.obj!=null)
            {
                UnityEngine.Object.Destroy(model.obj);
            }            
        }
    }    

    public override void excuteCmdWithParamters<T,T1>(string cmd, T t,T1 t1)
    {
        base.excuteCmdWithParamter(cmd, t);

        if (cmd == testModel.CMD_CREAT)
        {
            string name = t as string;
            model.obj = new GameObject(name);
        }
    }
}

public class testRender2 : DataRender<testModel>
{
    public override void start()
    {
    }

    public override void excuteCmdWithParamters<T,T1>(string cmd, T t,T1 t1)
    {
        base.excuteCmdWithParamter(cmd, t);

        if (cmd == testModel.CMD_CREAT)
        {
            model.obj.name = "testRender2";
        }
    }

    void creat(object[] objs)
    {
        model.obj.name = "testRender2";
    }
}
