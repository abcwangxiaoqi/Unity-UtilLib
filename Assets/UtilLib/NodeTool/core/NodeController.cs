/*
 * 启动流程 创建
 * 停止流程 销毁
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeTool
{

#if UNITY_EDITOR
    public class RouterPathMessage
    {
        public RouterPathMessage(int _id, int _conditionIndex)
        {
            this.id = _id;
            this.conditionIndex = _conditionIndex;
        }

        public int id { get; private set; }

        public int conditionIndex;
    }
#endif

    [DisallowMultipleComponent]
    public class NodeController : MonoBehaviour
    {

#if UNITY_EDITOR
        public List<int> entityPathMessage { get; private set; }
        public List<RouterPathMessage> routerPathMessage { get; private set; }
#endif

        public event Action<bool> onFinish;

        public NodeCanvasData data;

        public string currentName;

        [HideInInspector]
        public int currentID;

        [HideInInspector]
        public string error;

        //id 是 key
        Dictionary<int, BaseNode> entityMap = new Dictionary<int, BaseNode>();

        //id + className 是key
        Dictionary<string, BaseCondition> conditionMap = new Dictionary<string, BaseCondition>();

        NodeData current;
        SharedData shareData = null;
        // Use this for initialization
        void Awake()
        {

#if UNITY_EDITOR
            entityPathMessage = new List<int>();
            routerPathMessage = new List<RouterPathMessage>();
#endif

            if (!string.IsNullOrEmpty(data.shareData))
            {
                shareData = Activator.CreateInstance(Type.GetType(data.shareData)) as SharedData;
            }
            current = data.GetEntrance();
            moveNext = true;
        }

        Type type;
        BaseNode currentEntity;

        bool moveNext = false;
        public bool finished { get; private set; }

        RouterData tempRouter;
        // Update is called once per frame
        void Update()
        {
            /*
             * 主要是执行下一节点的Action
             */

            if (finished)
                return;

            if (!moveNext)
                return;

            if (current == null)
            {
                //当前实例为空 则流程结束
                finished = true;

                if(onFinish!=null)
                {
                    onFinish.Invoke(true);
                }                
                
                return;
            }

            currentName = current.name;
            currentID = current.id;

            try
            {
                if (!entityMap.ContainsKey(current.id))
                {
                    type = Type.GetType(current.className);
                    currentEntity = Activator.CreateInstance(type, shareData) as BaseNode;
                    entityMap.Add(current.id, currentEntity);
                }
                currentEntity = entityMap[current.id];

                currentEntity.run();
                moveNext = false;
            }
            catch (Exception e)
            {
                finished = true;
                if (onFinish != null)
                {
                    onFinish.Invoke(false);
                }
                error = e.Message;
                throw;
            }
        }

        DataBase tempDataBase;
        BaseCondition tempCondition;
        private void LateUpdate()
        {
            if (finished)
                return;

            if (moveNext)
                return;

            if (currentEntity == null)
                return;

            if (currentEntity.State == EntityState.Finished)
            {

#if UNITY_EDITOR

                if (!entityPathMessage.Contains(current.id))
                {
                    entityPathMessage.Add(current.id);
                }

#endif

                currentEntity.reset();
                moveNext = true;

                tempDataBase = data.Get(current.next);
                if (null == tempDataBase)
                {
                    current = null;
                }
                else if (tempDataBase.type == NodeType.Router)
                {
                    tempRouter = tempDataBase as RouterData;
                    currentID = tempRouter.id;
                    currentName = tempRouter.name;

                    try
                    {
                        for (int i = 0; i < tempRouter.conditions.Count; i++)
                        {
                            RouterConditionData item = tempRouter.conditions[i];

                            string key = string.Format("{0}+{1}", tempRouter.id, item.className);
                            if (!conditionMap.ContainsKey(key))
                            {
                                type = Type.GetType(item.className);

                                tempCondition = Activator.CreateInstance(type, shareData) as BaseCondition;

                                conditionMap.Add(key, tempCondition);
                            }

                            tempCondition = conditionMap[key];

                            if (!tempCondition.justify())
                            {
                                continue;
                            }

#if UNITY_EDITOR
                            addRouterPathMessage(tempRouter, i);
#endif

                            tempDataBase = data.Get(item.entity);
                            if (null == tempDataBase)
                            {
                                current = null;
                            }
                            else
                            {
                                current = tempDataBase as NodeData;
                            }
                            return;
                        }

#if UNITY_EDITOR
                        addRouterPathMessage(tempRouter, -1);
#endif

                        tempDataBase = data.Get(tempRouter.defaultEntity);
                        if (null == tempDataBase)
                        {
                            current = null;
                        }
                        else
                        {
                            current = tempDataBase as NodeData;
                        }
                    }
                    catch (Exception e)
                    {
                        error = e.Message;
                        finished = true;
                        if (onFinish != null)
                        {
                            onFinish.Invoke(false);
                        }                        
                        throw;
                    }
                }
                else //WindowType.Entity
                {
                    current = tempDataBase as NodeData;
                }
            }
        }

#if UNITY_EDITOR
        void addRouterPathMessage(RouterData router, int coditionIndex)
        {
            RouterPathMessage rpm = routerPathMessage.Find((RouterPathMessage m) =>
            {
                return m.id == router.id;
            });

            if (rpm == null)
            {
                routerPathMessage.Add(new RouterPathMessage(router.id, coditionIndex));
            }
            else
            {
                rpm.conditionIndex = coditionIndex;
            }
        }
#endif

        private void OnDestroy()
        {
            if (finished)
                return;

            if (currentEntity != null)
            {
                currentEntity.stop();
            }
        }

        public T LoadShareData<T>()
            where T : SharedData, new()
        {
            if (null == shareData)
                return null;

            return shareData as T;
        }
    }

}