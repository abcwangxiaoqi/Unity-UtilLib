using System;
using System.Collections.Generic;
namespace IMR
{
    public interface IDataRender : IDisposable
    {
        void update();
        void excuteCmd(string cmd);
        void excuteCmdWithParamter<T>(string cmd,T t);
        void excuteCmdWithParamters<T, T1>(string cmd, T t, T1 t1);
        void excuteCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2);
        void excuteCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3);
        void excuteCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4);
        void lateupdate();
        void fixedupdate();
        void start();
        void bindModel(IDataModel m);
    }
    public abstract class DataRender<D> : IDataRender
        where D : IDataModel
    {
        protected D model;        

        public DataRender()
        {
            MSingletonFactory.Get<AppCommon>().updateAction += update;
            MSingletonFactory.Get<AppCommon>().lateupdateAction += lateupdate;
            MSingletonFactory.Get<AppCommon>().fixedupdateAction += fixedupdate;
        }

        public void bindModel(IDataModel m)
        {
            model = (D)m;
        }

        virtual public void start()
        { }

        /// <summary>
        /// 每帧执行
        /// </summary>
        virtual public void update()
        { }

        virtual public void lateupdate()
        { }

        virtual public void fixedupdate()
        { }

        public virtual void excuteCmd(string cmd)
        {
        }

        public virtual void excuteCmdWithParamter<T>(string cmd, T t)
        {
        }

        public virtual void excuteCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
        }

        public virtual void excuteCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
        }

        public virtual void excuteCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
        }

        public virtual void excuteCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
        }

        public virtual void Dispose()
        {
            MSingletonFactory.Get<AppCommon>().updateAction -= update;
            MSingletonFactory.Get<AppCommon>().lateupdateAction -= lateupdate;
            MSingletonFactory.Get<AppCommon>().fixedupdateAction -= fixedupdate;            
        }
    }
}
