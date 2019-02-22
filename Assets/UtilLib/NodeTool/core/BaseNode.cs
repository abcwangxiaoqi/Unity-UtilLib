
namespace NodeTool
{
    public abstract class BaseNode
    {
        public EntityState State { get; private set; }

        protected SharedData shareData;
        public BaseNode(SharedData data)
        {
            shareData = data;
        }

        public void run()
        {
            State = EntityState.Wait;
            execute();
        }

        //节点完全通过后 状态重置
        //由主MainWF检测到Finished状态后 主动调用
        public void reset()
        {
            State = EntityState.Idle;
        }

        protected abstract void execute();

        //异步流程 执行到一半 
        //外界触发被动停止
        public virtual void stop()
        { }

        //确定执行完后 执行该方法
        protected void finish()
        {
            State = EntityState.Finished;
        }
    }
}