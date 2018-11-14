using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateTeest : MonoBehaviour {

    int[] arry = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    int curindex = 0;

    int size = 7;

	// Use this for initialization
	void Start () {
        UpdateRun.Instance.SetWindowSize(3);


        AnsyOperate.Instance.Add(() => 
        {
            Debug.Log(1);
        });

        AnsyOperate.Instance.Add(() =>
        {
            Debug.Log(2);
        });

        AnsyOperate.Instance.Add(() =>
        {
            Debug.Log(3);
        });

        AnsyOperate.Instance.Add(() =>
        {
            Debug.Log(4);
        });

        AnsyOperate.Instance.Add(() =>
        {
            Debug.Log(5);
        });

        AnsyOperate.Instance.Add(() =>
        {
            Debug.Log(6);
        });

        AnsyOperate.Instance.Add(Seven);

        AnsyOperate.Instance.Add(() =>
        {
            Debug.Log(8);
        });

        AnsyOperate.Instance.Add(() =>
        {
            Debug.Log(9);
        });

        AnsyOperate.Instance.Add(() =>
        {
            Debug.Log(10);
        });
    }

    void Seven()
    {
        Debug.Log(7);
    }
}
