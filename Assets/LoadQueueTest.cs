using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadQueueTest : MonoBehaviour {

    // Use this for initialization
    GameObject obj;
	void Start () {
        
         obj = Resources.Load<GameObject>("Sphere");


		
	}

    List<GameObject> gl = new List<GameObject>();

    int max = 10000;

    private void OnGUI()
    {
        if(GUILayout.Button("1"))
        {
            for (int i = 0; i < max; i++)
            {
                GameObject go =  GameObject.Instantiate(obj);
                go.transform.position = getpos();
                gl.Add(go);
            }
        }
        else if(GUILayout.Button("2"))
        {
            for (int i = 0; i < max; i++)
            {
                LoadQueue.Instance.LoadPool(() => 
                {
                    GameObject go = GameObject.Instantiate(obj);
                    go.transform.position = getpos();
                });
            }
        }
        else if (GUILayout.Button("destroy"))
        {
            for (int i = 0; i < gl.Count; i++)
            {
                Destroy(gl[i]);
            }
            gl.Clear();
        }
        else if (GUILayout.Button("destroy"))
        {
            for (int i = 0; i < gl.Count; i++)
            {
                GameObject g = gl[i];
                LoadQueue.Instance.LoadPool(() =>
                {
                    Destroy(g);
                });
            }
            gl.Clear();
        }
    }

    Vector3 getpos()
    {
        return new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
