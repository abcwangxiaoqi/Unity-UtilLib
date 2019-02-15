using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum WindowType
{
    Enity,
    Router
}

public abstract class BaseWindow
{
    protected GUIStyle textStyle = EditorStyles.textField;
    protected GUIStyle buttonStyle = EditorStyles.miniButton;
    protected GUIStyle popupStyle = EditorStyles.popup;

    protected Vector2 position;
    protected float height = 100;
    protected float weight = 150;

    protected Rect windowRect;
    protected List<BaseWindow> windowList;

    protected abstract WindowType windowType { get; }

    public int Id { get; private set; }
    public string Name { get; protected set; }

    public Vector2 In
    {
        get
        {
            return position + new Vector2(0, height / 2);
        }
    }
    public Vector2 Out
    {
        get
        {
            return position + new Vector2(weight, height / 2);
        }
    }

    protected List<string> allEntityClass = new List<string>();
    protected List<string> allConditionClass = new List<string>();
    public BaseWindow(Vector2 pos, List<BaseWindow> _windowList, List<string> _allEntityClass, List<string> _allConditionClass)
    {
        position = pos;
        windowList = _windowList;
        allEntityClass = _allEntityClass;
        allConditionClass = _allConditionClass;

        //设置id 从0开始 没有使用的就用

        int td = 0;

        while (_windowList.FindIndex((BaseWindow w) =>
        {
            return w.Id == td;
        }) >= 0)
        {
            td++;
        }

        Id = td;
    }

    public virtual void draw()
    {
        windowRect.x = position.x;
        windowRect.y = position.y;
        windowRect.width = weight;
        windowRect.height = height;

        windowRect = GUI.Window(Id, windowRect, gui, windowType.ToString());
    }

    protected virtual void gui(int id)
    {
        Name = GUILayout.TextField(Name, textStyle);
    }

    public virtual void rightMouseDraw(Vector2 mouseposition)
    {
    }

    public virtual void leftMouseDraw(Event curEvent)
    {
        //窗体移动位置
        if (curEvent.type == EventType.MouseDrag)
        {
            position += curEvent.delta;
        }
    }

    public bool isClick(Vector2 mouseposition)
    {
        return windowRect.Contains(mouseposition);
    }

    protected void DrawArrow(Vector2 from, Vector2 to, Color color)
    {
        Handles.BeginGUI();
        Handles.color = color;
        Handles.DrawAAPolyLine(3, from, to);
        Vector2 v0 = from - to;
        v0 *= 10 / v0.magnitude;
        Vector2 v1 = new Vector2(v0.x * 0.866f - v0.y * 0.5f, v0.x * 0.5f + v0.y * 0.866f);
        Vector2 v2 = new Vector2(v0.x * 0.866f + v0.y * 0.5f, v0.x * -0.5f + v0.y * 0.866f); ;
        Handles.DrawAAPolyLine(3, to + v1, to, to + v2);
        Handles.EndGUI();
    }
}
