/*
 * start the flow when the object is created
 * drop the flow when the object is destroyed
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptNodeFlow
{
    public enum EntityState
    {
        Idle,//idle,can be called
        Wait,// was called but not finish completely 
        Finished//finish completely,and move the next one
    }

    [DisallowMultipleComponent]
    public class NodeController : MonoBehaviour
    {

#if UNITY_EDITOR
        public List<int> nodePathMessage { get; private set; }
        public List<RouterPathMessage> routerPathMessage { get; private set; }
#endif

        public event Action<bool> onFinish;

        public NodeCanvasData nodeFlowData;

        public string currentName;

        [HideInInspector]
        public int currentID;

        [HideInInspector]
        public string error;

        //id = key
        Dictionary<int, Node> entityMap = new Dictionary<int, Node>();

        //id + className = key
        Dictionary<string, RouterCondition> conditionMap = new Dictionary<string, RouterCondition>();

        NodeData current;
        SharedData shareData = null;
        // Use this for initialization
        void Awake()
        {
#if UNITY_EDITOR
            nodePathMessage = new List<int>();
            routerPathMessage = new List<RouterPathMessage>();
#endif

            if (!string.IsNullOrEmpty(nodeFlowData.shareData))
            {
                shareData = Activator.CreateInstance(Type.GetType(nodeFlowData.shareData)) as SharedData;
            }
            current = nodeFlowData.GetEntrance();
            moveNext = true;
        }

        Type type;
        Node currentEntity;

        bool moveNext = false;
        public bool finished { get; private set; }

        RouterData tempRouter;
        // Update is called once per frame
        void Update()
        {
            /*
             * execute the next oen
             */

            if (finished)
                return;

            if (!moveNext)
                return;

            if (current == null)
            {
                //end when current is null
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
                    currentEntity = Activator.CreateInstance(type, shareData) as Node;
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
        RouterCondition tempCondition;
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

                if (!nodePathMessage.Contains(current.id))
                {
                    nodePathMessage.Add(current.id);
                }

#endif

                currentEntity.reset();
                moveNext = true;

                tempDataBase = nodeFlowData.Get(current.next);
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

                                tempCondition = Activator.CreateInstance(type, shareData) as RouterCondition;

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

                            tempDataBase = nodeFlowData.Get(item.entity);
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

                        tempDataBase = nodeFlowData.Get(tempRouter.defaultEntity);
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
                else //NodeType.Node
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
            shareData = null;

#if UNITY_EDITOR
            nodePathMessage = null;
            routerPathMessage = null;
#endif

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
}