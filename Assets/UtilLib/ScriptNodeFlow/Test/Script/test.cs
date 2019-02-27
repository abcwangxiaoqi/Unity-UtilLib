using ScriptNodeFlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test:MonoBehaviour
{
    private void Start()
    {
        UnityEngine.Object o = Resources.Load("testScriptNodeFlow");

        GameObject flow = GameObject.Instantiate(o) as GameObject;
        flow.GetComponent<NodeController>().onFinish += Test_onFinish;
    }

    private void Test_onFinish(bool obj)
    {
        Debug.Log("State=>" + obj);
    }
}
