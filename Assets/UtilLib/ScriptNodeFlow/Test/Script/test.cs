using ScriptNodeFlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test:MonoBehaviour
{
    GameObject flow;
    private void Awake()
    {
        UnityEngine.Object o = Resources.Load("testScriptNodeFlow");

        flow = GameObject.Instantiate(o) as GameObject;
        flow.GetComponent<NodeController>().onFinish += Test_onFinish;

    }

    private void Test_onFinish(bool obj)
    {
        Debug.Log("State=>" + obj);
    }
}
