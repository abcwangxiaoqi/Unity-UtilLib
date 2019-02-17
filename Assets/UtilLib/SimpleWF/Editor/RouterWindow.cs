using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RouterWindow : BaseWindow
{
    class RouterCondition
    {
        public string className;
        public BaseWindow enity;

        [System.NonSerialized]
        public Vector2 drawPos;
    }

    List<RouterCondition> conditions = new List<RouterCondition>();

    public override WindowType windowType
    {
        get
        {
            return WindowType.Router;
        }
    }

    EnityWindow defaultEnity = null;
    Vector2 defaultPos;

    public RouterWindow(Vector2 pos, WFEditorWindow _mainWindow)
        :base(pos, _mainWindow)
    {
        Name = "Router";
        
        addHeight = buttonStyle.lineHeight + 8;
    
    }

    public RouterWindow(WindowDataRouter data, WFEditorWindow _mainWindow)
        : base(data, _mainWindow)
    {
        addHeight = buttonStyle.lineHeight + 8;

        if(data.conditions!=null)
        {
            foreach (var item in data.conditions)
            {
                RouterCondition c = new RouterCondition();
                c.className = item.className;

                if(item.enity!=null)
                {
                    BaseWindow enity = mainWindow.windowList.Find((BaseWindow w) => 
                    {
                        return w.Id == item.enity.id;
                    });

                    if(null == enity)
                    {
                        enity = new EnityWindow(item.enity, mainWindow);
                        mainWindow.windowList.Add(enity);
                    }
                    c.enity = enity;
                }

                conditions.Add(c);
            }
        }

        if(data.defaultEnity !=null)
        {
            BaseWindow enity = mainWindow.windowList.Find((BaseWindow w) =>
            {
                return w.Id == data.defaultEnity.id;
            });

            if (null == enity)
            {
                enity = new EnityWindow(data.defaultEnity, mainWindow);
                mainWindow.windowList.Add(enity);
            }
            defaultEnity = enity as EnityWindow;
        }
    }

    public override object GetData()
    {
        WindowDataRouter data = new WindowDataRouter();
        data.id = Id;
        data.name = Name;
        data.x = x;
        data.y = y;

        foreach (var item in conditions)
        {
            WindowDataCondition cond = new WindowDataCondition();

            cond.className = item.className;

            if(item.enity!=null)
            {
                cond.enity = (WindowDataEnity)item.enity.GetData();
            }

            data.conditions.Add(cond);
        }

        if(defaultEnity!=null)
        {
            data.defaultEnity = (WindowDataEnity)defaultEnity.GetData();
        }

        return data;
    }

    public override void draw()
    {
        base.draw();

        //画线

        #region condition list
        for (int i = 0; i < conditions.Count; i++)
        {
            RouterCondition item = conditions[i];

            if (item.enity == null)
                continue;

            if (!mainWindow.windowList.Contains(item.enity))
            {
                item.enity = null;
                continue;
            }

            DrawArrow(item.drawPos + new Vector2(x,y), item.enity.In, Color.green);
        }
        #endregion

        #region default
        if (defaultEnity == null)
            return;

        if (!mainWindow.windowList.Contains(defaultEnity))
        {
            defaultEnity = null;
            return;
        }

        DrawArrow(defaultPos + new Vector2(x, y), defaultEnity.In, Color.green);
        #endregion
    }    


    float addHeight;
    Rect rect;
    protected override void gui(int id)
    {
        base.gui(id);

        GUI.color = Color.green;
        if (GUILayout.Button("Add", buttonStyle))
        {
            conditions.Add(new RouterCondition());
            
            height += addHeight;
        }
        GUI.color = Color.white;

        GUILayout.Space(10);

        drawConditions();

        GUILayout.Space(10);

        drawDefualt();

        GUI.DragWindow();
    }

    void drawConditions()
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            RouterCondition rc = conditions[i];
            GUILayout.BeginHorizontal();

            string c = rc.className;
            int selectindex = mainWindow.allConditionClass.IndexOf(c);
            selectindex = EditorGUILayout.Popup(selectindex, mainWindow.allConditionClass.ToArray(), popupStyle);
            if (selectindex >= 0)
            {
                conditions[i].className = mainWindow.allConditionClass[selectindex];
            }

            //删除
            GUI.color = Color.red;
            if (GUILayout.Button("-", buttonStyle))
            {
                conditions.RemoveAt(i);
                i--;
                height -= addHeight;
            }
            GUI.color = Color.white;

            //连接选择
            GUI.color = Color.green;
            if (MyEditorLayout.Button("L", buttonStyle, out rect))
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("New Entity"), false, () =>
                {
                    var tempWindow = new EnityWindow(new Vector2(x+50, y+50), mainWindow);
                    mainWindow.windowList.Add(tempWindow);
                    rc.enity = tempWindow;
                });

                List<BaseWindow> selectionList = new List<BaseWindow>();
                foreach (var item in mainWindow.windowList)
                {
                    if (item is EnityWindow)
                    {
                        selectionList.Add(item);
                    }
                }

                foreach (var item in selectionList)
                {
                    bool select = (rc.enity != null) && rc.enity.Id == item.Id;
                    menu.AddItem(new GUIContent(item.Id + " " + item.Name), select, () =>
                    {
                        rc.enity = item;
                    });
                }

                menu.ShowAsContext();

                rc.drawPos = rect.position + new Vector2(rect.width, rect.height / 2);
            }
            GUI.color = Color.white;

            GUILayout.EndHorizontal();
        }
    }

    void drawDefualt()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("deault", EditorStyles.boldLabel);

        //连接选择
        GUI.color = Color.green;
        if (MyEditorLayout.Button("L", buttonStyle, out rect))
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("New Entity"), false, () =>
            {
                var tempWindow = new EnityWindow(new Vector2(x+50, y+50), mainWindow);
                mainWindow.windowList.Add(tempWindow);
                defaultEnity = tempWindow;
            });

            List<BaseWindow> selectionList = new List<BaseWindow>();
            foreach (var item in mainWindow.windowList)
            {
                if (item is EnityWindow)
                {
                    selectionList.Add(item);
                }
            }

            foreach (var item in selectionList)
            {
                bool select = (defaultEnity != null) && defaultEnity.Id == item.Id;
                menu.AddItem(new GUIContent(item.Id + " " + item.Name), select, () =>
                {
                    defaultEnity = item as EnityWindow;
                });
            }

            menu.ShowAsContext();
            defaultPos = rect.position + new Vector2(rect.width, rect.height / 2);
        }
        GUI.color = Color.white;
        GUILayout.EndHorizontal();
    }

    public override void rightMouseDraw(Vector2 mouseposition)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Delte"), false, () =>
        {
            mainWindow.windowList.Remove(this);
        });

        menu.ShowAsContext();
    }
}

public static class MyEditorLayout
{
    //封装一下这样可以拿到button的Rect
    public static bool Button(string txt, GUIStyle style, out Rect rect)
    {
        GUIContent content = new GUIContent(txt);
        rect = GUILayoutUtility.GetRect(content, style);
        return GUI.Button(rect, content, style);
    }

    public static bool Button(string txt, out Rect rect)
    {
        GUIContent content = new GUIContent(txt);
        rect = GUILayoutUtility.GetRect(content, EditorStyles.miniButton);
        return GUI.Button(rect, content, EditorStyles.miniButton);
    }
}