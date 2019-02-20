using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using EditorTools;

using Object = UnityEngine.Object;

public class WFEditorWindow :EditorWindow
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

    public List<string> allEntityClass { get; private set; }
    public List<string> allConditionClass { get; private set; }

    //全部
    public List<BaseWindow> windowList { get; private set; }

    WindowData wdata;

    private void Awake()
    {     
        allEntityClass = new List<string>();
        allConditionClass = new List<string>();
        windowList = new List<BaseWindow>();

        Assembly _assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp.dll");
        Type[] tys = _assembly.GetTypes();
        
        foreach (var item in tys)
        {           
            if (item.IsSubclassOf(typeof(BaseEntity)) && !item.IsInterface && !item.IsAbstract)
            {
                allEntityClass.Add(item.FullName);
            }

            if (item.IsSubclassOf(typeof(BaseCondition)) && !item.IsInterface && !item.IsAbstract)
            {
                allConditionClass.Add(item.FullName);
            }
        }
        
        generateWindowData();
    }    

    BaseWindow curSelect = null;
    Event curEvent;
    Vector2 mousePosition;
    void OnGUI()
    {        
        if (windowList == null)
            return;

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
            if(curSelect!=null)
            {
                curSelect.leftMouseDraw(curEvent);
            }
        }

        if (windowList == null)
            return;

        // 注意：必须在  BeginWindows(); 和 EndWindows(); 之间 调用 GUI.Window 才能显示
        BeginWindows();
        for (int i = 0; i < windowList.Count; i++)
        {
            windowList[i].draw();
        }
        EndWindows();


        GUI.BeginGroup(new Rect(20, 20, 100, 200));

        GUI.Box(new Rect(0, 0, 100, 200), "");
        GUILayout.Label("ShareData");

        GUI.EndGroup();
    }

    //关闭窗口时 保存数据
    private void OnDisable()
    {
        save();
        AssetDatabase.Refresh();
    }

    private void OnLostFocus()
    {
        save();
    }

    private void OnProjectChange()
    {
        save();
        AssetDatabase.Refresh();
    }

    private void ShowMenu()
    {
        // 添加一个新节点
        if (Event.current.type == EventType.MouseDown)
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
                    windowList.Add(new EntityWindow(mousePosition, this));
                });

                menu.AddItem(new GUIContent("Add Rounter"), false, () =>
                {
                    windowList.Add(new RouterWindow(mousePosition, this));
                });
                menu.ShowAsContext();
            }
        }
    }

    void generateWindowData()
    {
        wdata = scriptable.Load<WindowData>();

        foreach (var item in wdata.entitylist)
        {
            windowList.Add(new EntityWindow(item, this));
        }

        foreach (var item in wdata.routerlist)
        {
            windowList.Add(new RouterWindow(item, this));
        }       

        //设置下一级
        foreach (var item in windowList)
        {
            WindowDataBase itemdata = wdata.Get(item.Id);

            if(itemdata.type == WindowType.Entity)
            {
                WindowDataEntity edata = itemdata as WindowDataEntity;
               
                if (edata.next>=0)
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

    void save()
    {
        if (windowList == null)
            return;

        wdata.entitylist.Clear();
        wdata.routerlist.Clear();

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

    void DrawNodeCurve(Rect start, Rect end, Color color)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 4);
    }
}
