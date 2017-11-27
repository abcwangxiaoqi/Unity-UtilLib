using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallelTaskCollection : TaskCollection,IEnumerator
{
    int _paraNum = 3;
    public ParallelTaskCollection(int paraNum)
    {
        if (paraNum>0)
        {
            _paraNum = paraNum;
        }
    }

    object IEnumerator.Current
    {
        get
        {
            return curList.Count==0 ? null : curList[0].Current;
        }
    }
    List<IEnumerator> curList = new List<IEnumerator>();
    bool IEnumerator.MoveNext()
    {
        while (curList.Count < _paraNum && tasklist.Count > 0)
        {
            curList.Add(tasklist.Dequeue());
        }

        for (int i = 0; i < curList.Count; i++)
        {
            if(!curList[i].MoveNext())
            {
                curList.RemoveAt(i);
                i--;
            }
        }

        if (curList.Count == 0 && tasklist.Count==0)
        {
            finish();
            return false;
        }
        return true;
    }

    void IEnumerator.Reset()
    { }
}
