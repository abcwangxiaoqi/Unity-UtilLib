using UnityEngine;
using System.Collections;

public class ParallelIEnumerator : MyEnumerator
{
    ParallelTaskCollection otc = null;
     public ParallelIEnumerator(int paralleNum=3)
    {
        otc = new ParallelTaskCollection(paralleNum);
        otc.AddFinishAction(finish);
     }

     public override void AddIEnumerator(IEnumerator ienumerator)
     {
         otc.AddIEnumerator(ienumerator);
     }

     public override void Start()
     {
         task = new SimpleTask(otc);
     }
}