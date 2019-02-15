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

    protected override WindowType windowType
    {
        get
        {
            return WindowType.Enity;
        }
    }

    public EnityWindow(Vector2 pos, List<BaseWindow> _windowList, List<string> allEntityClass, List<string> allConditionClass)
        : base(pos, _windowList, allEntityClass, allConditionClass)
    {
        Name = "Enity";
    }

    public override void draw()
    {
        base.draw();

        //画线
        if (next != null)
        {
            DrawArrow(Out, next.In, Color.white);
        }
    }

    protected override void gui(int id)
    {
        base.gui(id);

        int selectindex = -1;

        if (!string.IsNullOrEmpty(ClassName))
        {
            selectindex = allEntityClass.IndexOf(ClassName);
        }

        selectindex = EditorGUILayout.Popup(selectindex, allEntityClass.ToArray(), popupStyle);
        if (selectindex >= 0)
        {
            ClassName = allEntityClass[selectindex];
        }

        GUI.DragWindow();
    }

    public override void rightMouseDraw(Vector2 mouseposition)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Next/New Entity"), false, () =>
        {
            var tempWindow = new EnityWindow(mouseposition, windowList, allEntityClass,allConditionClass);
            windowList.Add(tempWindow);
            next = tempWindow;
        });

        menu.AddItem(new GUIContent("Next/New Condition"), false, () =>
        {
            var tempWindow = new RouterWindow(mouseposition, windowList, allEntityClass, allConditionClass);
            windowList.Add(tempWindow);
            next = tempWindow;
        });

        #region 选择下一个
        List<BaseWindow> selectionList = new List<BaseWindow>();

        foreach (var item in windowList)
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
            windowList.Remove(this);
        });

        menu.AddItem(new GUIContent("Entry"), Entry, () =>
        {
            foreach (var item in windowList)
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