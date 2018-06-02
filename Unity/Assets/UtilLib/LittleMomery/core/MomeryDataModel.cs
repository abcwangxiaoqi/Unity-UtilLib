using System;
using System.Collections.Generic;
using IMR;

namespace LittleMomery
{
    public class MomeryDataModel : DataModel
    {
        public const string CMD_INSERT = "cmd_insert";
        public const string CMD_DELETE = "cmd_delete";
        public const string CMD_CLEARALL = "cmd_clearall";
        public const string CMD_REGISTER = "cmd_register";

        public Dictionary<Type, Type> MapTable = new Dictionary<Type, Type>();

        public Dictionary<Type, Dictionary<string, MomeryItem>> cache = new Dictionary<Type, Dictionary<string, MomeryItem>>();//缓存容器
    }
}
