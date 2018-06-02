namespace SimpleWF
{
    public abstract class WFEntity : IEntity
    {
        public IEntity next { get; private set; }
        public bool finish
        {
            get
            {
                return state == 1;
            }
        }

        int state = -1;//-1位激活 0正在运行 1完成

        public WFEntity()
        {
            state = -1;
        }

        public virtual void stop()
        {
            state = -1;
        }

        public void update()
        {
            if (state == 0)
                return;
            state = 0;
            execute();            
        }

        public abstract void execute();

        public void Then(WFEntity entity)
        {
            next = entity;
        }

        public void Router(WFRouter router)
        {
            next = router;
        }

        public void reset()
        {
            state = -1;
        }

        protected void notify()
        {
            state = 1;
        }

        public string EndMsg { get; private set; }

        /// <summary>
        /// 直接指向结束点 默认next没有 就会指向结束点 为了代码编写的完整性
        /// </summary>
        public void End(string msg=null)
        {
            EndMsg = msg;
            next = null;
        }

        public virtual void dispose()
        { }
    }
}
