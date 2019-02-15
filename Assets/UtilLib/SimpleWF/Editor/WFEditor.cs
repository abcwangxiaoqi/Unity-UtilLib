using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public class WFEditor :EditorWindow
{
    [MenuItem("WF/Open")]
    static void open()
    {
        var window = EditorWindow.GetWindow<WFEditor>("WF Editor");        
    }


    List<string> allEntityClass = new List<string>();
    List<string> allConditionClass = new List<string>();
    private void OnEnable()
    {
        Assembly _assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp.dll");
        Type[] tys = _assembly.GetTypes();
        
        foreach (var item in tys)
        {
            if (typeof(IMMEntity).IsAssignableFrom(item) && !item.IsInterface && !item.IsAbstract)
            {
                allEntityClass.Add(item.FullName);
            }

            if (typeof(ICondition).IsAssignableFrom(item) && !item.IsInterface && !item.IsAbstract)
            {
                allConditionClass.Add(item.FullName);
            }
        }
    }
    
    //全部
    List<BaseWindow> windowList = new List<BaseWindow>();

    static void ShowEditor()
    {
        WFEditor editor = EditorWindow.GetWindow<WFEditor>();
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
                    windowList.Add(new EnityWindow(mousePosition,windowList,allEntityClass,allConditionClass));
                });

                menu.AddItem(new GUIContent("Add Rounter"), false, () =>
                {
                    windowList.Add(new RouterWindow( mousePosition,windowList, allEntityClass, allConditionClass));
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
