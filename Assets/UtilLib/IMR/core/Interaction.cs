using System;

namespace IMR
{
    public abstract class InteractionBase<D> :IDisposable
        where D : IDataModel, new()
    {
        public InteractionBase()
        {
            initilize();
        }

        protected D model;
        protected bool active = false;
        /// <summary>
        /// 带参数 尽量使用 后面泛型方法 本方法带参数 会有 装箱拆箱
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parmters"></param>
        protected void sendCmd(string cmd, params object[] parmters)
        {
            if (!active)
            {
                initilize();
            }
            model.Enqueue(cmd, parmters);
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

        protected abstract void initilize();

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
    }

    public abstract class Interaction<D, R1> : InteractionBase<D>
            where D : IDataModel, new()
            where R1 : IDataRender, new()
    {
        protected override void initilize()
        {
            model = new D();
            model.Register<R1>();
            active = true;
        }
    }

    public abstract class Interaction<D, R1, R2> : InteractionBase<D>
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
    {

        protected override void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            active = true;
        }
    }

    public abstract class Interaction<D, R1, R2, R3> : InteractionBase<D>
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
    {
        protected override void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            model.Register<R3>();
            active = true;
        }
    }

    public abstract class Interaction<D, R1, R2, R3, R4> : InteractionBase<D>
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
            where R4 : IDataRender, new()
    {

        protected override void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            model.Register<R3>();
            model.Register<R4>();
            active = true;
        }
    }

    public abstract class Interaction<D, R1, R2, R3, R4, R5> : InteractionBase<D>
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
            where R4 : IDataRender, new()
            where R5 : IDataRender, new()
    {

        protected override void initilize()
        {
            model = new D();
            model.Register<R1>();
            model.Register<R2>();
            model.Register<R3>();
            model.Register<R4>();
            model.Register<R5>();
            active = true;
        }        
    }

    public abstract class Interaction<D, R1, R2, R3, R4, R5, R6> : InteractionBase<D>
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
            where R4 : IDataRender, new()
            where R5 : IDataRender, new()
            where R6 : IDataRender, new()
    {

        protected override void initilize()
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
    }

    public abstract class Interaction<D, R1, R2, R3, R4, R5, R6, R7> : InteractionBase<D>
            where D : IDataModel, new()
            where R1 : IDataRender, new()
            where R2 : IDataRender, new()
            where R3 : IDataRender, new()
            where R4 : IDataRender, new()
            where R5 : IDataRender, new()
            where R6 : IDataRender, new()
            where R7 : IDataRender, new()
    {
        protected override void initilize()
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
    }

    public abstract class Interaction<D, R1, R2, R3, R4, R5, R6, R7, R8> : InteractionBase<D>
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

        protected override void initilize()
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
    }
}