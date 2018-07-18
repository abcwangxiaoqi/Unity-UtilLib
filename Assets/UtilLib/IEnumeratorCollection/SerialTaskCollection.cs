using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SerialTaskCollection : TaskCollection,IEnumerator
{
    IEnumerator current = null;

    object IEnumerator.Current
    {
        get 
        {
            return current.Current;
        }
    }

    bool IEnumerator.MoveNext()
    {
        if (current == null)
        {
            current = tasklist.Dequeue();
        }

        if (!current.MoveNext())
        {
            if (tasklist.Count > 0)
            {
                current = tasklist.Dequeue();
                return true;
            }
            else
            {
                finish();
                return false;
            }
        }
        return true;
    }

    void IEnumerator.Reset()
    {}
}
