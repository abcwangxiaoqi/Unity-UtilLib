
using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;


//https://www.jb51.net/article/142945.htm
namespace Application
{
    public class NewClass
    {
        bool state = true;
        Task task = null;
        public NewClass()
        {
            task = Task.Factory.StartNew(Loop);
        }

        void Loop()
        {
            while(state)
            {
                Thread.Sleep(100);

                //pull data from contianer

            }

        }

        public void Start()
        {
            if (null == task)
                return;

            task.Start();
        }

        public void Stop()
        {
            if (null == task)
                return;

            task.Dispose();
        }


        async void test()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
