using EditorTools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class WFWindow: EditorWindow
{
    //全部
    protected List<BaseWindow> windowList=new List<BaseWindow>();

    protected FixedWindow fixedWindow;

    protected virtual void Awake()
    {

    }

    protected virtual void OnGUI()
    {
        if (windowList == null)
            return;

        // 注意：必须在  BeginWindows(); 和 EndWindows(); 之间 调用 GUI.Window 才能显示
        BeginWindows();

        fixedWindow.draw();

        for (int i = 0; i < windowList.Count; i++)
        {
            windowList[i].draw();
        }

        EndWindows();
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void OnLostFocus()
    {

    }

    protected virtual void OnProjectChange()
    {

    }

    BaseWindow FindWindow(int id)
    {
        BaseWindow res = windowList.Find((BaseWindow w) =>
        {
            return w.Id == id;
        });
        return res;
    }

    T FindWindow<T>(int id)
        where T : BaseWindow
    {
        BaseWindow res = windowList.Find((BaseWindow w) =>
        {
            return w.Id == id;
        });

        if (res == null)
            return null;

        return res as T;
    }

    protected void generateWindowData(WindowData windowData)
    {
        fixedWindow = new FixedWindow(windowData.shareData);

        foreach (var item in windowData.entitylist)
        {
            windowList.Add(new EntityWindow(item, windowList));
        }

        foreach (var item in windowData.routerlist)
        {
            windowList.Add(new RouterWindow(item, windowList));
        }

        //设置下一级
        foreach (var item in windowList)
        {
            WindowDataBase itemdata = windowData.Get(item.Id);

            if (itemdata.type == WindowType.Entity)
            {
                WindowDataEntity edata = itemdata as WindowDataEntity;

                if (edata.next >= 0)
                {
                    BaseWindow next = FindWindow(edata.next);

                    (item as EntityWindow).SetNext(next);
                }
            }
            else
            {
                WindowDataRouter edata = itemdata as WindowDataRouter;
                RouterWindow win = item as RouterWindow;

                //设置默认
                if (edata.defaultEntity >= 0)
                {
                    EntityWindow def = FindWindow<EntityWindow>(edata.defaultEntity);
                    win.SetDefault(def);
                }

                //设置条件
                List<RouterCondition> conditions = new List<RouterCondition>();
                foreach (var con in edata.conditions)
                {
                    RouterCondition rcon = new RouterCondition();
                    rcon.className = con.className;
                    rcon.entity = FindWindow<EntityWindow>(con.entity);
                    conditions.Add(rcon);
                }
                win.SetConditions(conditions);
            }
        }
    }
}

public class WFEditorWindow : WFWindow
{
    [MenuItem("Assets/EditorWF", true)]
    static bool ValidateSelection()
    {
        Object asset = Selection.activeObject;

        return (asset is WindowData);
    }

    [MenuItem("Assets/EditorWF", false, priority = 49)]
    static void Edit()
    {
        Object asset = Selection.activeObject;
        scriptable = new ScriptableItem(AssetDatabase.GetAssetPath(asset));
        var window = EditorWindow.GetWindow<WFEditorWindow>(asset.name);
    }

    [MenuItem("Assets/NewWF", false, priority = 49)]
    static void New()
    {
        EditorUtil.CreatAssetCurPath<WindowData>("NewWf",
            (WindowData data,Dictionary<string,object> dic) => 
            {
                //默认数据 且是 入口实例
                WindowDataEntity entity = new WindowDataEntity();
                entity.isEntrance = true;
                entity.id = 0;
                entity.position = new Vector2(50, 50);
                entity.name = "Entity";
                data.entitylist.Add(entity);
            });   
    }

    public static void Open(Object obj)
    {
        scriptable = new ScriptableItem(AssetDatabase.GetAssetPath(obj));
        var window = EditorWindow.GetWindow<WFEditorWindow>(obj.name);
    }

    static ScriptableItem scriptable;

    WindowData wdata;

    protected override void Awake()
    {       
        windowList = new List<BaseWindow>();

        wdata = scriptable.Load<WindowData>();

        generateWindowData(wdata);
    }    

    BaseWindow curSelect = null;
    Event curEvent;
    Vector2 mousePosition;
    protected override void OnGUI()
    {
        curEvent = Event.current;
        mousePosition = curEvent.mousePosition;

        if (curEvent.button == 1) // 鼠标右键
        {
            ShowMenu();
        }

        // 鼠标左键
        if (curEvent.button == 0 && curEvent.isMouse)
        {
            //判断选择
            if (curEvent.type == EventType.MouseDown)
            {
                curSelect = windowList.Find((BaseWindow w) =>
                {
                    return w.isClick(mousePosition);
                });
            }

            //窗体移动位置
            if (curSelect != null)
            {
                curSelect.leftMouseDraw(curEvent);
            }
        }

        base.OnGUI();
    }

    //关闭窗口时 保存数据
    protected override void OnDisable()
    {
        save();
        AssetDatabase.Refresh();
    }

    protected override void OnLostFocus()
    {
        save();
    }

    protected override void OnProjectChange()
    {
        save();
        AssetDatabase.Refresh();
    }

    private void ShowMenu()
    {
        // 添加一个新节点
        if (curEvent.type == EventType.MouseDown)
        {
            curSelect = windowList.Find((BaseWindow w) =>
            {
                return w.isClick(mousePosition);
            });

            if (curSelect != null)
            {
                curSelect.rightMouseDraw(mousePosition);
            }
            else
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add Enity"), false, () =>
                {
                    windowList.Add(new EntityWindow(mousePosition, windowList));
                });

                menu.AddItem(new GUIContent("Add Rounter"), false, () =>
                {
                    windowList.Add(new RouterWindow(mousePosition, windowList));
                });
                menu.ShowAsContext();
            }
        }
    }    

    void save()
    {
        if (windowList == null)
            return;

        wdata.entitylist.Clear();
        wdata.routerlist.Clear();
        wdata.shareData = fixedWindow.shareData;

        for (int i = 0; i < windowList.Count; i++)
        {
            if (windowList[i].windowType == WindowType.Entity)
            {
                wdata.entitylist.Add((WindowDataEntity)windowList[i].GetData());
            }
            else
            {
                wdata.routerlist.Add((WindowDataRouter)windowList[i].GetData());
            }
        }

        scriptable.SaveAsset(wdata);
    }
}
