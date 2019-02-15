using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RouterWindow : BaseWindow
{
    class RouterCondition
    {
        public string condition;
        public BaseWindow enity;

        [System.NonSerialized]
        public Vector2 drawPos;
    }

    List<RouterCondition> conditions = new List<RouterCondition>();

    protected override WindowType windowType
    {
        get
        {
            return WindowType.Router;
        }
    }

    EnityWindow defaultEnity = null;
    Vector2 defaultPos;

    public RouterWindow(Vector2 pos, List<BaseWindow> _windowList, List<string> allEntityClass, List<string> allConditionClass)
        :base(pos, _windowList, allEntityClass, allConditionClass)
    {
        Name = "Router";
        
        addHeight = buttonStyle.lineHeight + 8;
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

            if (!windowList.Contains(item.enity))
            {
                item.enity = null;
                continue;
            }

            DrawArrow(item.drawPos + position, item.enity.In, Color.green);
        }
        #endregion

        #region default
        if (defaultEnity == null)
            return;

        if (!windowList.Contains(defaultEnity))
        {
            defaultEnity = null;
            return;
        }

        DrawArrow(defaultPos + position, defaultEnity.In, Color.green);
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

            string c = rc.condition;
            int selectindex = allConditionClass.IndexOf(c);
            selectindex = EditorGUILayout.Popup(selectindex, allConditionClass.ToArray(), popupStyle);
            if (selectindex >= 0)
            {
                conditions[i].condition = allConditionClass[selectindex];
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
                    var tempWindow = new EnityWindow(position + new Vector2(50, 50), windowList, allEntityClass, allConditionClass);
                    windowList.Add(tempWindow);
                    rc.enity = tempWindow;
                });

                List<BaseWindow> selectionList = new List<BaseWindow>();
                foreach (var item in windowList)
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
                var tempWindow = new EnityWindow(position + new Vector2(50, 50), windowList, allEntityClass, allConditionClass);
                windowList.Add(tempWindow);
                defaultEnity = tempWindow;
            });

            List<BaseWindow> selectionList = new List<BaseWindow>();
            foreach (var item in windowList)
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
            windowList.Remove(this);
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