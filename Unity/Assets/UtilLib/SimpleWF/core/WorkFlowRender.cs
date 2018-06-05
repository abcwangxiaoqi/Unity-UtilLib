using System;
using IMR;

namespace SimpleWF
{
    public class WorkFlowRender : DataRender<WorkFlowModel>
    {
        public override void start()
        {
            base.start();
            
        }

        public override void excuteCmd(string cmd, params object[] paramters)
        {
            base.excuteCmd(cmd);

            if(cmd== WorkFlowModel.CMD_START)
            {
                execute();
            }
            else if(cmd == WorkFlowModel.CMD_STOP)
            {
                stop();
            }
        }

        public override void excuteCmdWithParamter<T>(string cmd, T t)
        {
            base.excuteCmdWithParamter(cmd, t);

            if(cmd== WorkFlowModel.CMD_STARTWITH)
            {
                model.startItem = t as WFEntity;
            }
            else if(cmd == WorkFlowModel.CMD_REGISTER)
            {
                model.finishAction += t as Action<bool, string>;
            }
            else if (cmd == WorkFlowModel.CMD_UNREGISTER)
            {
                model.finishAction -= t as Action<bool, string>;
            }
        }

        void execute()
        {
            stop();
            model.switchFlag = true;
        }

        void stop()
        {
            if(model.curItem!=null)
            {
                model.curItem.stop();
            }            
            model.curItem = model.startItem;
            model.switchFlag = false;
        }

        void notify(bool success,string msg)
        {            
            if (model.finishAction != null)
            {
                model.finishAction.Invoke(success, msg);
            }
        }

        public override void update()
        {
            base.update();

            if (!model.switchFlag)
                return;

            if (model.curItem == null)
                return;

            if (!model.curItem.finish)
            {
                try
                {
                    model.curItem.update();
                }
                catch (Exception e)
                {
                    notify(false,e.Message);
                }
            }
            else
            {
                if(model.curItem.next==null)
                {
                    notify(true, model.curItem.EndMsg);
                    model.curItem = model.curItem.next;
                }
                else
                {
                    model.curItem = model.curItem.next;
                    model.curItem.reset();
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            stop();
        }
    }
}
