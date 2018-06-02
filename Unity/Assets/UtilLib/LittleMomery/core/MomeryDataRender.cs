using UnityEngine;
using IMR;
using System;
using System.Collections.Generic;

namespace LittleMomery
{
    public class MomeryDataRender : DataRender<MomeryDataModel>
    {
        public override void start()
        {
            base.start();

            //registerCmd(MomeryDataModel.CMD_INSERT, insert);
            //registerCmd(MomeryDataModel.CMD_DELETE, destroy);
            //registerCmd(MomeryDataModel.CMD_CLEARALL, clearall);
            //registerCmd(MomeryDataModel.CMD_REGISTER, register);
        }

        public override void excuteCmd(string cmd)
        {
            base.excuteCmd(cmd);

            if(cmd== MomeryDataModel.CMD_CLEARALL)
            {
                clearall();
            }
        }

        public override void excuteCmdWithParamters<T, T1, T2>(string cmd, T t, T1 t1, T2 t2)
        {
            base.excuteCmdWithParamters(cmd, t, t1, t2);

            if(cmd== MomeryDataModel.CMD_INSERT)
            {
                insert(t as string, t1 as Type, t2 as object);
            }
        }

        public override void excuteCmdWithParamters<T, T1>(string cmd, T t, T1 t1)
        {
            base.excuteCmdWithParamters(cmd, t, t1);

            if(cmd== MomeryDataModel.CMD_DELETE)
            {
                destroy(t as string,t1 as Type);
            }
            else if(cmd== MomeryDataModel.CMD_REGISTER)
            {
                register(t as Type,t1 as Type);
            }
        }

        void clearall()
        {
            model.cache.Foreach((Type type, Dictionary<string, MomeryItem> dic) =>
            {
                dic.Foreach((string key, MomeryItem item) =>
                {
                    if (model.MapTable.ContainsKey(type))
                    {
                        item = Activator.CreateInstance(model.MapTable[type]) as MomeryItem;
                    }
                    item.unload();
                    return true;
                });
                dic.Clear();
                dic = null;
                return true;
            });
            model.cache.Clear();
        }

        void destroy(string key,Type type)
        {
            if (!model.cache.ContainsKey(type))
            {
                return;
            }

            if (!model.cache[type].ContainsKey(key))
            {
                return;
            }

            model.cache[type][key].unload();
            model.cache[type].Remove(key);
        }

        void insert(string key,Type type,object v)
        {
            Dictionary<string, MomeryItem> dic = null;

            if (model.cache.ContainsKey(type))
            {
                dic = model.cache[type];
            }
            else
            {
                dic = new Dictionary<string, MomeryItem>();
                model.cache.Add(type, dic);
            }

            if (dic.ContainsKey(key))
            {
                Debug.LogWarning("key=" + key + ",type=" + type.Name + ",had exist you still tried to insert!!");
            }
            else
            {
                MomeryItem item = null;
                if (model.MapTable.ContainsKey(type))
                {
                    item = Activator.CreateInstance(model.MapTable[type]) as MomeryItem;
                }
                else
                {
                    item = new MomeryItem();
                }

                item.type = type;
                item.key = key;
                item.val = v;

                dic.Add(key, item);
            }
        }

        void register(Type t1 ,Type t2)
        {
            model.MapTable.Add(t1, t2);
        }
    }
}
