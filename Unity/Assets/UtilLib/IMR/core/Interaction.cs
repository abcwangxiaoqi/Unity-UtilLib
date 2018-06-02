using System;

namespace IMR
{
    public abstract class Interaction<D, R1> : IDisposable
            where D : IDataModel, new()
            where R1 : IDataRender, new()
    {
        protected bool active = false;
        protected D model;
        public Interaction()
        {
            initilize();
        }

        void initilize()
        {
            model = new D();
            model.Register<R1>();
            active = true;
        }

        public virtual void Dispose()
        {
            if (false == active)
            {
                return;
            }
            model.Dispose();
            model = default(D);
            active = false;
        }

        protected void sendCmd(string cmd)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd);
        }

        protected void sendCmdWithParamter<T>(string cmd, T t)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t);
        }

        protected void sendCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1);
        }

        protected void sendCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3, t4);
        }
    }

    public abstract class Interaction<D, R1, R2> : IDisposable
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
    {
        protected bool active = false;
        protected D model;
        public Interaction()
        {
            initilize();
        }

        void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            active = true;
        }

        public virtual void Dispose()
        {
            if (false == active)
            {
                return;
            }
            model.Dispose();
            model = default(D);
            active = false;
        }

        protected void sendCmd(string cmd)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd);
        }

        protected void sendCmdWithParamter<T>(string cmd, T t)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t);
        }

        protected void sendCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1);
        }

        protected void sendCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3, t4);
        }
    }

    public abstract class Interaction<D, R1, R2, R3> : IDisposable
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
    {
        protected bool active = false;
        protected D model;
        public Interaction()
        {
            initilize();
        }

        void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            model.Register<R3>();
            active = true;
        }

        public virtual void Dispose()
        {
            if (false == active)
            {
                return;
            }
            model.Dispose();
            model = default(D);
            active = false;
        }

        protected void sendCmd(string cmd)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd);
        }

        protected void sendCmdWithParamter<T>(string cmd, T t)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t);
        }

        protected void sendCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1);
        }

        protected void sendCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3, t4);
        }
    }

    public abstract class Interaction<D, R1, R2, R3, R4> : IDisposable
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
            where R4 : IDataRender, new()
    {
        protected bool active = false;
        protected D model;
        public Interaction()
        {
            initilize();
        }

        void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            model.Register<R3>();
            model.Register<R4>();
            active = true;
        }

        public virtual void Dispose()
        {
            if (false == active)
            {
                return;
            }
            model.Dispose();
            model = default(D);
            active = false;
        }

        protected void sendCmd(string cmd)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd);
        }

        protected void sendCmdWithParamter<T>(string cmd, T t)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t);
        }

        protected void sendCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1);
        }

        protected void sendCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3, t4);
        }
    }

    public abstract class Interaction<D, R1, R2, R3, R4, R5> : IDisposable
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
            where R4 : IDataRender, new()
            where R5 : IDataRender, new()
    {
        protected bool active = false;
        protected D model;
        public Interaction()
        {
            initilize();
        }

        void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            model.Register<R3>();
            model.Register<R4>();
            model.Register<R5>();
            active = true;
        }

        public virtual void Dispose()
        {
            if (false == active)
            {
                return;
            }
            model.Dispose();
            model = default(D);
            active = false;
        }

        protected void sendCmd(string cmd)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd);
        }

        protected void sendCmdWithParamter<T>(string cmd, T t)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t);
        }

        protected void sendCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1);
        }

        protected void sendCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3, t4);
        }
    }

    public abstract class Interaction<D, R1, R2, R3, R4, R5, R6> : IDisposable
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
            where R4 : IDataRender, new()
            where R5 : IDataRender, new()
            where R6 : IDataRender, new()
    {
        protected bool active = false;
        protected D model;
        public Interaction()
        {
            initilize();
        }

        void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            model.Register<R3>();
            model.Register<R4>();
            model.Register<R5>();
            model.Register<R6>();
            active = true;
        }

        public virtual void Dispose()
        {
            if (false == active)
            {
                return;
            }
            model.Dispose();
            model = default(D);
            active = false;
        }

        protected void sendCmd(string cmd)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd);
        }

        protected void sendCmdWithParamter<T>(string cmd, T t)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t);
        }

        protected void sendCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1);
        }

        protected void sendCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3, t4);
        }
    }

    public abstract class Interaction<D, R1, R2, R3, R4, R5, R6, R7> : IDisposable
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
            where R4 : IDataRender, new()
            where R5 : IDataRender, new()
            where R6 : IDataRender, new()
            where R7 : IDataRender, new()
    {
        protected bool active = false;
        protected D model;
        public Interaction()
        {
            initilize();
        }

        void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            model.Register<R3>();
            model.Register<R4>();
            model.Register<R5>();
            model.Register<R6>();
            model.Register<R7>();
            active = true;
        }

        public virtual void Dispose()
        {
            if (false == active)
            {
                return;
            }
            model.Dispose();
            model = default(D);
            active = false;
        }

        protected void sendCmd(string cmd)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd);
        }

        protected void sendCmdWithParamter<T>(string cmd, T t)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t);
        }

        protected void sendCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1);
        }

        protected void sendCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3, t4);
        }
    }

    public abstract class Interaction<D, R1, R2, R3, R4, R5, R6, R7, R8> : IDisposable
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
            where R4 : IDataRender, new()
            where R5 : IDataRender, new()
            where R6 : IDataRender, new()
            where R7 : IDataRender, new()
            where R8 : IDataRender, new()
    {
        protected bool active = false;
        protected D model;
        public Interaction()
        {
            initilize();
        }

        void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            model.Register<R3>();
            model.Register<R4>();
            model.Register<R5>();
            model.Register<R6>();
            model.Register<R7>();
            model.Register<R8>();
            active = true;
        }

        public virtual void Dispose()
        {
            if (false == active)
            {
                return;
            }
            model.Dispose();
            model = default(D);
            active = false;
        }

        protected void sendCmd(string cmd)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd);
        }

        protected void sendCmdWithParamter<T>(string cmd, T t)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t);
        }

        protected void sendCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1);
        }

        protected void sendCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3>(string cmd, T t, T1 t1, T2 t2, T3 t3)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3);
        }

        protected void sendCmdWithParamters<T, T1, T2, T3, T4>(string cmd, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, t, t1, t2, t3, t4);
        }
    }
}