using System;
using System.Collections.Generic;

namespace SimpleWF
{
    public class WFRouter : IEntity
    {
        public IEntity next { get; private set; }
        public bool finish { get; private set; }

        IEntity defentity;
        List<WFRouterCondition> cconditions = new List<WFRouterCondition>();

        public WFRouter()
        {
            finish = false;
        }

        public void stop()
        {
            finish = false;
        }

        public string EndMsg { get; private set; }

        public void update()
        {
            for (int i = 0; i < cconditions.Count; i++)
            {
                if(cconditions[i].execute())
                {
                    finish = true;
                    next = cconditions[i].entity;

                    if(next==null)
                    {
                        EndMsg = cconditions[i].EndMsg;
                    }
                    break;
                }
            }

            if(!finish && defentity != null)
            {
                finish = true;
                next = defentity;
            }
        }

        public WFRouterCondition If(Func<bool> func)
        {
            WFRouterCondition cod = new WFRouterCondition( func);
            cconditions.Add(cod);
            return cod;
        }

        public WFEntity Default(WFEntity entity)
        {
            defentity = entity;
            return entity;
        }

        public void reset()
        {
            finish = false;
        }

        public void dispose()
        {

        }
    }
}
