/*
 * 启动流程 创建
 * 停止流程 销毁
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MainWF : MonoBehaviour
{
    public event Action<bool> onFinish;

    //editor 脚本保证其不能修改 动态绑定
    public WindowData data;

    public string currentName;
    public int currentID; 

    //id 是 key
    Dictionary<int, BaseEntity> entityMap = new Dictionary<int, BaseEntity>();

    //id + className 是key
    Dictionary<string, BaseCondition> conditionMap = new Dictionary<string, BaseCondition>();

    WindowDataEntity current;
    SharedData shareData = null;
    // Use this for initialization
    void Awake()
    {
        if(!string.IsNullOrEmpty(data.shareData))
        {
            shareData = Activator.CreateInstance(Type.GetType(data.shareData)) as SharedData;
        }
        current = data.GetEntrance();
        moveNext = true;
    }

    Type type;
    BaseEntity currentEntity;

    bool moveNext = false;
    public bool finished { get; private set; }

    WindowDataRouter tempRouter;
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
            onFinish?.Invoke(true);
            finished = true;
            return;
        }

        currentName = current.name;
        currentID = current.id;

        try
        {
            if(!entityMap.ContainsKey(current.id))
            {
                type = Type.GetType(current.className);
                currentEntity = Activator.CreateInstance(type, shareData) as BaseEntity;
                entityMap.Add(current.id, currentEntity);
            }
            currentEntity = entityMap[current.id];
         
            currentEntity.run();
            moveNext = false;
        }
        catch (Exception e)
        {
            finished = true;
            onFinish?.Invoke(false);
            throw;
        }
    }

    WindowDataBase tempDataBase;
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
            currentEntity.reset();
            moveNext = true;

            tempDataBase = data.Get(current.next);
            if(null == tempDataBase)
            {
                current = null;
            }
            else if (tempDataBase.type == WindowType.Router)
            {
                tempRouter = tempDataBase as WindowDataRouter;
                foreach (var item in tempRouter.conditions)
                {
                    string key = string.Format("{0}+{1}", tempRouter.id, item.className);
                    if(!conditionMap.ContainsKey(key))
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

                    tempDataBase = data.Get(item.entity);
                    if (null == tempDataBase)
                    {
                        current = null;
                    }
                    else
                    {
                        current = tempDataBase as WindowDataEntity;                       
                    }
                    return;
                }

                tempDataBase = data.Get(tempRouter.defaultEntity);
                if(null == tempDataBase)
                {
                    current = null;
                }
                else
                {
                    current = tempDataBase as WindowDataEntity;
                }                
            }
            else //WindowType.Entity
            {
                current = tempDataBase as WindowDataEntity;
            }
        }
    }

    private void OnDestroy()
    {
        if (finished)
            return;

        if (currentEntity!=null)
        {
            currentEntity.stop();
        }
    }

    public T LoadShareData<T>()
        where T : SharedData,new()
    {
        if (null == shareData)
            return null;

        return shareData as T;
    }
}
