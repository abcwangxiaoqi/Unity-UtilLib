using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public class WFEditorWindow :EditorWindow
{
    [MenuItem("WF/Open")]
    static void open()
    {
        var window = EditorWindow.GetWindow<WFEditorWindow>("WF Editor");        
    }


    public List<string> allEntityClass { get; private set; }
    public List<string> allConditionClass { get; private set; }

    //全部
    public List<BaseWindow> windowList { get; private set; }

    private void Awake()
    {
        windowList = new List<BaseWindow>();

        Assembly _assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp.dll");
        Type[] tys = _assembly.GetTypes();
        
        foreach (var item in tys)
        {
           
            if (item.IsSubclassOf(typeof(BaseMMEntity)) && !item.IsInterface && !item.IsAbstract)
            {
                allEntityClass.Add(item.FullName);
            }

            if (item.IsSubclassOf(typeof(MCondition)) && !item.IsInterface && !item.IsAbstract)
            {
                allConditionClass.Add(item.FullName);
            }
        }
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

            if(curSelect!=null)
            {
                curSelect.rightMouseDraw(mousePosition);
            }
            else
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add Enity"), false, () =>
                {
                    windowList.Add(new EnityWindow(mousePosition,this));
                });

                menu.AddItem(new GUIContent("Add Rounter"), false, () =>
                {
                    windowList.Add(new RouterWindow( mousePosition,this));
                });
                menu.ShowAsContext();
            }
        }   
    }
    
    BaseWindow curSelect = null;
    Event curEvent;
    Vector2 mousePosition;
    void OnGUI()
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
            if(curSelect!=null)
            {
                curSelect.leftMouseDraw(curEvent);
            }
        }


        // 注意：必须在  BeginWindows(); 和 EndWindows(); 之间 调用 GUI.Window 才能显示
        BeginWindows();
        for (int i = 0; i < windowList.Count; i++)
        {
            windowList[i].draw();
        }
        EndWindows();

        if(GUI.changed)
        {
            
        }

        if(GUILayout.Button("save"))
        {
            save();
        }

    }

    void save()
    {
        WindowData data = new WindowData();
        for (int i = 0; i < windowList.Count; i++)
        {
            if(windowList[i].windowType == WindowType.Enity)
            {
                data.enitylist.Add((WindowDataEnity)windowList[i].GetData());
            }
            else
            {
                data.routerlist.Add((WindowDataRouter)windowList[i].GetData());
            }
        }

        //产生编辑窗口数据
        generateWindowData(data);


        //=================================================

        //生产运行时数据

        //入口
        WindowDataEnity entry = data.enitylist.Find((WindowDataEnity e) => 
        {
            return e.entry == true;
        });


    }

    void generateWindowData(WindowData data)
    {
        List<BaseWindow> list = new List<BaseWindow>();

        foreach (var item in data.enitylist)
        {
            //为什么要判断是否存在
            //因为在构造函数中 回创建所有相关等实例
            bool exist = windowList.Exists((BaseWindow w) =>
            {
                return w.Id == item.id;
            });

            if (exist)
                continue;

            windowList.Add(new EnityWindow(item, this));
        }

        foreach (var item in data.routerlist)
        {
            //为什么要判断是否存在
            //因为在构造函数中 回创建所有相关等实例
            bool exist = windowList.Exists((BaseWindow w) =>
            {
                return w.Id == item.id;
            });

            if (exist)
                continue;

            windowList.Add(new RouterWindow(item, this));
        }
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
