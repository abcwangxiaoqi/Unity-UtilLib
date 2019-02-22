using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NodeTool
{
    public enum State
    {
        Idle,
        Running,
        Error
    }

    public abstract class BaseWindow
    {
        protected static GUIStyle BigLabelStyle;

        static BaseWindow()
        {
            BigLabelStyle = new GUIStyle(UnityEditor.EditorStyles.boldLabel);
            BigLabelStyle.fixedHeight = 25;
            BigLabelStyle.fontSize = 20;
            BigLabelStyle.alignment = TextAnchor.MiddleCenter;
            BigLabelStyle.normal.textColor = Color.green;
        }

        //主要用作 记录路径信息
        protected bool passed = false;

        protected State state = State.Idle;

        protected GUIStyle textStyle = EditorStyles.textField;
        protected GUIStyle buttonStyle = EditorStyles.miniButton;
        protected GUIStyle popupStyle = EditorStyles.popup;

        public Vector2 position { get; protected set; }
        protected float height = 100;
        protected float weight = 150;

        protected Rect windowRect;

        protected List<BaseWindow> windowList;
        public abstract NodeType windowType { get; }

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

        public BaseWindow(Vector2 pos, List<BaseWindow> _windowList)
        {
            position = pos;
            windowList = _windowList;
            //设置id 从0开始 没有使用的就用

            int td = 0;

            while (windowList.FindIndex((BaseWindow w) =>
            {
                return w.Id == td;
            }) >= 0)
            {
                td++;
            }

            Id = td;
        }

        public BaseWindow(DataBase data, List<BaseWindow> _windowList)
        {
            position = data.position;
            windowList = _windowList;

            Id = data.id;
            Name = data.name;
        }

        public virtual void draw()
        {
            windowRect.position = position;
            windowRect.width = weight;
            windowRect.height = height;

            if (Application.isPlaying)
            {
                if (state == State.Running)
                {
                    BigLabelStyle.normal.textColor = Color.green;
                    Rect rect = new Rect(windowRect.position + new Vector2(0, -30), new Vector2(weight, 20));
                    GUI.Label(rect, "Running...", BigLabelStyle);
                }
                else if (state == State.Error)
                {
                    BigLabelStyle.normal.textColor = Color.red;
                    Rect rect = new Rect(windowRect.position + new Vector2(0, -30), new Vector2(weight, 20));
                    GUI.Label(rect, "Error", BigLabelStyle);
                }
            }

            //windowRect = GUI.Window(Id, windowRect, gui, windowType.ToString(), NodeCanvas.Editor.CanvasStyles.window);
            windowRect = GUI.Window(Id, windowRect, gui, windowType.ToString());
        }

        protected virtual void gui(int id)
        {
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            Name = GUILayout.TextField(Name, textStyle);
            EditorGUI.EndDisabledGroup();
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

        public abstract DataBase GetData();

        protected void DrawArrow2(Vector2 from, Vector2 to, Color color)
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

        protected void DrawArrow(Vector2 from, Vector2 to, Color color)
        {
            Vector3 startPos = new Vector3(from.x, from.y, 0);
            Vector3 endPos = new Vector3(to.x, to.y, 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 4);
        }

        public void SetState(State _state)
        {
            state = _state;
        }


        //记录经过路径
        public virtual void Pass(params object[] objs)
        {
            passed = true;
        }
    }
}

