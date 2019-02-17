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

    public float x;
    public float y;
    protected float height = 100;
    protected float weight = 150;

    protected Rect windowRect;

    protected WFEditorWindow mainWindow;
    public abstract WindowType windowType { get; }

    public int Id { get; private set; }
    public string Name { get; protected set; }

    public Vector2 In
    {
        get
        {
            return new Vector2(x,y) + new Vector2(0, height / 2);
        }
    }
    public Vector2 Out
    {
        get
        {
            return new Vector2(x, y) + new Vector2(weight, height / 2);
        }
    }

    public BaseWindow(Vector2 pos, WFEditorWindow _mainWindow)
    {
        x = pos.x;
        y = pos.y;
        mainWindow = _mainWindow;

        //设置id 从0开始 没有使用的就用

        int td = 0;

        while (mainWindow.windowList.FindIndex((BaseWindow w) =>
        {
            return w.Id == td;
        }) >= 0)
        {
            td++;
        }

        Id = td;
    }

    public BaseWindow(WindowDataBase data, WFEditorWindow _mainWindow)
    {
        x = data.x;
        y = data.y;
        mainWindow = _mainWindow;


        Id = data.id;
        Name = data.name;
    }

    public virtual void draw()
    {
        windowRect.x = x;
        windowRect.y = y;
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
            x += curEvent.delta.x;
            y += curEvent.delta.y;
        }
    }

    public bool isClick(Vector2 mouseposition)
    {
        return windowRect.Contains(mouseposition);
    }

    public abstract object GetData();


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
