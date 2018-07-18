using System;

namespace SimpleWF
{
    public class WFRouterCondition
    {
        public WFEntity entity;
        Func<bool> func;
        public WFRouterCondition(Func<bool> _func)
        {
            func = _func;
        }

        public void Then(WFEntity item)
        {
            entity = item;
        }

        public bool execute()
        {
            if (func != null)
                return func.Invoke();

            return false;
        }

        public string EndMsg { get; private set; }

        /// <summary>
        /// 直接指向 结束点
        /// </summary>
        public void End(string msg=null)
        {
            EndMsg = msg;
            entity = null;
        }
    }
}
