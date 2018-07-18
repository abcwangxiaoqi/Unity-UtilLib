using UnityEngine;
using System.Collections;

public class SerialIEnumerator : MyEnumerator
{
    SerialTaskCollection stc = null;
    public SerialIEnumerator()
    {
        stc = new SerialTaskCollection();
        stc.AddFinishAction(finish);
    }

    public override void AddIEnumerator(IEnumerator ienumerator)
    {
        stc.AddIEnumerator(ienumerator);
    }

    public override void Start()
    {
        task = new SimpleTask(stc);
    }
}
