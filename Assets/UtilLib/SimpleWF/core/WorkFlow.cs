using System;
using IMR;

namespace SimpleWF
{
    public class WorkFlow : Interaction<WorkFlowModel, WorkFlowRender>
    {
        public bool running
        {
            get
            {
                return model.switchFlag;
            }
        }

        public WFEntity StartWith(WFEntity entity)
        {
            sendCmdWithParamter(WorkFlowModel.CMD_STARTWITH, entity);
            return model.startItem;
        }

        public void Run()
        {
            sendCmd(WorkFlowModel.CMD_START);
        }

        public void Stop()
        {
            sendCmd(WorkFlowModel.CMD_STOP);
        }

        public void Register(Action<bool,string> action)
        {
            sendCmdWithParamter(WorkFlowModel.CMD_REGISTER, action);
        }

        public void UnRegister(Action<bool, string> action)
        {
            sendCmdWithParamter(WorkFlowModel.CMD_UNREGISTER, action);
        }
    }
}
