using System.Collections.Generic;
using System;

namespace IMR
{
    public interface IDataModel : IDisposable
    {
        void Enqueue(string cmd);
        void Enqueue<T>(string cmd, T t);      
        void Enqueue<T,T1>(string cmd, T t,T1 t1);
        void Enqueue<T, T1,T2>(string cmd, T t, T1 t1,T2 t2);
        void Enqueue<T, T1, T2,T3>(string cmd, T t, T1 t1, T2 t2,T3 t3);
        void Enqueue<T, T1, T2, T3,T4>(string cmd, T t, T1 t1, T2 t2, T3 t3,T4 t4);
        void Register<T>() where T : IDataRender, new();
    }

    public abstract class DataModel : IDataModel
    {
        List<IDataRender> renders = new List<IDataRender>();

        public void Enqueue(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
                return;

            for (int i = 0; i < renders.Count; i++)
            {
                renders[i].excuteCmd(cmd);
            }
        }

        public void Enqueue<T>(string cmd, T t)
        {
            if (string.IsNullOrEmpty(cmd))
                return;

            for (int i = 0; i < renders.Count; i++)
            {
                renders[i].excuteCmdWithParamter(cmd, t);
            }
        }

        public void Enqueue<T, T1>(string cmd, T t, T1 t1)
        {
            if (string.IsNullOrEmpty(cmd))
                return;

            for (int i = 0; i < renders.Count; i++)
            {
                renders[i].excuteCmdWithParamters(cmd, t,t1);
            }
        }

        public void Enqueue<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            if (string.IsNullOrEmpty(cmd))
                return;

            for (int i = 0; i < renders.Count; i++)
            {
                renders[i].excuteCmdWithParamters(cmd, t,t1,t2);
            }
        }

        public void Enqueue<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
            if (string.IsNullOrEmpty(cmd))
                return;

            for (int i = 0; i < renders.Count; i++)
            {
                renders[i].excuteCmdWithParamters(cmd, t,t1,t2,t3);
            }
        }

        public void Enqueue<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (string.IsNullOrEmpty(cmd))
                return;

            for (int i = 0; i < renders.Count; i++)
            {
                renders[i].excuteCmdWithParamters(cmd, t,t1,t2,t3,t4);
            }
        }

        public virtual void Dispose()
        {
            for (int i = 0; i < renders.Count; i++)
            {
                renders[i].Dispose();
                renders[i]=null;
            }

            renders.Clear();
        }
        
        public void Register<T>()
            where T : IDataRender,new()
        {
            IDataRender render = new T();
            render.bindModel(this);
            render.start();
            renders.Add(render);
        }
    }
}
