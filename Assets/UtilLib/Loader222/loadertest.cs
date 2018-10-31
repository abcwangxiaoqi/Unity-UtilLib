using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;

public class loadertest : MonoBehaviour
{
    // Use this for initialization
    async void Start()
    {
        Debug.Log("Waiting 1 second...");
        await Task.Delay(System.TimeSpan.FromSeconds(1));
        Debug.Log("Done!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public static class AwaitExtensions
{
    public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
    {
        return Task.Delay(timeSpan).GetAwaiter();
    }
}
