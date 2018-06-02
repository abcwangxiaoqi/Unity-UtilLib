using System;
using System.Collections.Generic;
using IMR;

namespace SimpleWF
{
    public class WorkFlowModel : DataModel
    {
        public const string CMD_STARTWITH = "cmd_startwith";
        public const string CMD_START = "cmd_start";
        public const string CMD_STOP = "cmd_stop";
        public const string CMD_REGISTER = "cmd_register";
        public const string CMD_UNREGISTER = "cmd_unregister";
        public const string CMD_BINDENITYPARAMS = "cmd_bindenityparams";

        //public Dictionary<string, WFEntity> entities = new Dictionary<string, WFEntity>();
        //public List<IEntity> allsteps = new List<IEntity>();
        public WFEntity entity;
        public WFEntity startItem;
        public IEntity curItem;
        public Action<bool,string> finishAction;

        public bool switchFlag = false;

        public override void Dispose()
        {
            base.Dispose();

            entity = null;
            startItem = null;
            curItem = null;
            finishAction = null;
        }
    }
}
