using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnityWindow : BaseWindow
{
    //是否是入口
    public bool Entry = false;

    public string ClassName;

    //下一节点
    public BaseWindow next { get; protected set; }

    public override WindowType windowType
    {
        get
        {
            return WindowType.Enity;
        }
    }

    public EnityWindow(Vector2 pos, WFEditorWindow _mainWindow)
        : base(pos, _mainWindow)
    {
        Name = "Enity";
    }

    public EnityWindow(WindowDataEnity data, WFEditorWindow _mainWindow)
        : base(data, _mainWindow)
    {
        if(data.nextEnity!=null)
        {
            BaseWindow n = mainWindow.windowList.Find((BaseWindow win) =>
            {
                return win.Id == data.nextEnity.id;
            });

            if(null == n)
            {
                n = new EnityWindow(data.nextEnity, mainWindow);
                mainWindow.windowList.Add(n);
            }

            next = n;
        }
        else if(data.nextRouter!=null)
        {
            BaseWindow n = mainWindow.windowList.Find((BaseWindow win) =>
            {
                return win.Id == data.nextRouter.id;
            });

            if (null == n)
            {
                n = new RouterWindow(data.nextRouter, mainWindow);
                mainWindow.windowList.Add(n);
            }

            next = n;
        }
    }

    public override object GetData()
    {
        WindowDataEnity dataEnity = new WindowDataEnity();
        dataEnity.x = x;
        dataEnity.y = y;
        dataEnity.name = Name;
        dataEnity.id = Id;

        dataEnity.className = ClassName;

        dataEnity.entry = Entry;

        if(next!=null)
        {
            if(dataEnity.nextEnity!=null || dataEnity.nextRouter!=null)
            {
                //说明已经设置过下一节点等值了
                //这里返回 是避免 死循环等情况出现
                return dataEnity;
            }
            
            if(next.windowType == WindowType.Enity)
            {
                dataEnity.nextEnity = (WindowDataEnity)next.GetData();
            }
            else
            {
                dataEnity.nextRouter = (WindowDataRouter)next.GetData();
            }
        }

        return dataEnity;
    }

    public override void draw()
    {
        base.draw();

        //画线
        if (next != null)
        {
            if (!mainWindow.windowList.Contains(next))
            {
                next = null;
                return;
            }

            DrawArrow(Out, next.In, Color.white);
        }
    }

    protected override void gui(int id)
    {
        if(Entry)
        {
            GUI.contentColor = Color.green;
        }
        else
        {
            GUI.contentColor = Color.white;
        }
        
        base.gui(id);

        int selectindex = -1;

        if (!string.IsNullOrEmpty(ClassName))
        {
            selectindex = mainWindow.allEntityClass.IndexOf(ClassName);
        }

        selectindex = EditorGUILayout.Popup(selectindex, mainWindow.allEntityClass.ToArray(), popupStyle);
        if (selectindex >= 0)
        {
            ClassName = mainWindow.allEntityClass[selectindex];
        }

        GUI.DragWindow();
    }

    public override void rightMouseDraw(Vector2 mouseposition)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Next/New Entity"), false, () =>
        {
            var tempWindow = new EnityWindow(mouseposition, mainWindow);
            mainWindow.windowList.Add(tempWindow);
            next = tempWindow;
        });

        menu.AddItem(new GUIContent("Next/New Condition"), false, () =>
        {
            var tempWindow = new RouterWindow(mouseposition, mainWindow);
            mainWindow.windowList.Add(tempWindow);
            next = tempWindow;
        });

        #region 选择下一个
        List<BaseWindow> selectionList = new List<BaseWindow>();

        foreach (var item in mainWindow.windowList)
        {
            if (item == this)
                continue;
            selectionList.Add(item);
        }

        foreach (var item in selectionList)
        {
            bool select = (next != null) && next.Id == item.Id;
            menu.AddItem(new GUIContent("Next/" + item.Id + " " + item.Name), select, () =>
            {
                next = item;
            });
        }
        #endregion

        menu.AddItem(new GUIContent("Delete Next"), false, () =>
        {
            next = null;
        });

        menu.AddItem(new GUIContent("Delte"), false, () =>
        {
            mainWindow.windowList.Remove(this);
        });

        menu.AddItem(new GUIContent("Entry"), Entry, () =>
        {
            foreach (var item in mainWindow.windowList)
            {
                if (item is EnityWindow)
                {
                    (item as EnityWindow).Entry = false;
                }
            }

            Entry = true;
        });

        menu.ShowAsContext();
    }
}