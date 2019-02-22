﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace NodeTool
{
    public class RouterCondition
    {
        public string className;
        public NodeWindow entity;
        public Vector2 drawPos;
    }

    public class RouterWindow : BaseWindow
    {
        static List<string> allConditionClass = new List<string>();
        static GUIStyle linkStyle;
        static RouterWindow()
        {
            linkStyle = new GUIStyle(UnityEditor.EditorStyles.boldLabel);
            linkStyle.fixedHeight = 10;
            linkStyle.fontSize = 8;
            linkStyle.alignment = TextAnchor.MiddleCenter;
            linkStyle.normal.textColor = Color.green;
            linkStyle.fixedWidth = 10;

            Assembly _assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp.dll");
            Type[] tys = _assembly.GetTypes();

            foreach (var item in tys)
            {
                if (item.IsSubclassOf(typeof(BaseCondition)) && !item.IsInterface && !item.IsAbstract)
                {
                    allConditionClass.Add(item.FullName);
                }
            }
        }

        List<RouterCondition> conditions = new List<RouterCondition>();

        public override NodeType windowType
        {
            get
            {
                return NodeType.Router;
            }
        }

        //runtime 模式下 条件通过的index
        int runtimePassedConditionIndex = -1;

        NodeWindow defaultEntity = null;
        Vector2 defaultPos;

        public RouterWindow(Vector2 pos, List<BaseWindow> _windowList)
            : base(pos, _windowList)
        {
            Name = "Router";

            addHeight = buttonStyle.lineHeight + 8;

        }

        public RouterWindow(RouterData itemData, List<BaseWindow> _windowList)
            : base(itemData, _windowList)
        {
            addHeight = buttonStyle.lineHeight + 8;
        }

        public void SetDefault(NodeWindow defEntity)
        {
            defaultEntity = defEntity;
        }

        public void SetConditions(List<RouterCondition> conditionEntities)
        {
            conditions = conditionEntities;
            height += addHeight * conditionEntities.Count;
        }

        public override DataBase GetData()
        {
            RouterData data = new RouterData();
            data.id = Id;
            data.name = Name;
            data.position = position;

            foreach (var item in conditions)
            {
                RouterConditionData cond = new RouterConditionData();

                cond.className = item.className;

                if (item.entity != null)
                {
                    cond.entity = item.entity.Id;
                }

                data.conditions.Add(cond);
            }

            if (defaultEntity != null)
            {
                data.defaultEntity = defaultEntity.Id;
            }

            return data;
        }

        Color color;

        public override void draw()
        {
            base.draw();

            //画线

            #region condition list
            for (int i = 0; i < conditions.Count; i++)
            {
                RouterCondition item = conditions[i];

                if (item.entity == null)
                    continue;

                if (!windowList.Contains(item.entity))
                {
                    item.entity = null;
                    continue;
                }

                if (item.drawPos == Vector2.zero)
                    continue;

                color = Color.white;

                if (Application.isPlaying && passed && runtimePassedConditionIndex == i)
                {
                    color = Color.green;
                }

                DrawArrow(item.drawPos + position, item.entity.In, color);
            }
            #endregion

            #region default
            if (defaultEntity == null)
                return;

            if (!windowList.Contains(defaultEntity))
            {
                defaultEntity = null;
                return;
            }
            if (defaultPos == Vector2.zero)
                return;

            color = Color.white;

            if (Application.isPlaying && passed && runtimePassedConditionIndex == -1)
            {
                color = Color.green;
            }

            DrawArrow(defaultPos + position, defaultEntity.In, color);
            #endregion
        }


        float addHeight;
        Rect rect;
        protected override void gui(int id)
        {
            base.gui(id);

            EditorGUI.BeginDisabledGroup(Application.isPlaying);

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

            EditorGUI.EndDisabledGroup();

            GUI.DragWindow();
        }

        void drawConditions()
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                RouterCondition rc = conditions[i];
                GUILayout.BeginHorizontal();

                string c = rc.className;
                int selectindex = allConditionClass.IndexOf(c);
                selectindex = EditorGUILayout.Popup(selectindex, allConditionClass.ToArray(), popupStyle);
                if (selectindex >= 0)
                {
                    conditions[i].className = allConditionClass[selectindex];
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
                        var tempWindow = new NodeWindow(new Vector2(50, 50) + position, windowList);
                        windowList.Add(tempWindow);
                        rc.entity = tempWindow;
                    });

                    List<NodeWindow> selectionList = new List<NodeWindow>();
                    foreach (var item in windowList)
                    {
                        if (item.windowType == NodeType.Entity)
                        {
                            selectionList.Add(item as NodeWindow);
                        }
                    }

                    foreach (var item in selectionList)
                    {
                        bool select = (rc.entity != null) && rc.entity.Id == item.Id;
                        menu.AddItem(new GUIContent(item.Id + " " + item.Name), select, () =>
                        {
                            if (select)
                            {
                                rc.entity = null;
                            }
                            else
                            {
                                rc.entity = item;
                            }
                        });
                    }

                    menu.ShowAsContext();
                }


                GUI.color = Color.white;

                if (rc.entity == null)
                {
                    linkStyle.normal.textColor = Color.gray;
                }
                else
                {
                    linkStyle.normal.textColor = Color.green;
                }

                MyEditorLayout.Label("o", linkStyle, out rect);

                //有的时候 rect会为0，0，1，1
                if (rect.position != Vector2.zero)
                {
                    rc.drawPos.x = rect.position.x + rect.width;
                    rc.drawPos.y = rect.position.y + rect.height / 2;
                }

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
                    var tempWindow = new NodeWindow(new Vector2(50, 50) + position, windowList);
                    windowList.Add(tempWindow);
                    defaultEntity = tempWindow;
                });

                List<BaseWindow> selectionList = new List<BaseWindow>();
                foreach (var item in windowList)
                {
                    if (item is NodeWindow)
                    {
                        selectionList.Add(item);
                    }
                }

                foreach (var item in selectionList)
                {
                    bool select = (defaultEntity != null) && defaultEntity.Id == item.Id;
                    menu.AddItem(new GUIContent(item.Id + " " + item.Name), select, () =>
                    {
                        if (select)
                        {
                            defaultEntity = null;
                        }
                        else
                        {
                            defaultEntity = item as NodeWindow;
                        }
                    });
                }

                menu.ShowAsContext();
            }

            if (defaultEntity == null)
            {
                linkStyle.normal.textColor = Color.gray;
            }
            else
            {
                linkStyle.normal.textColor = Color.green;
            }

            MyEditorLayout.Label("o", linkStyle, out rect);

            //有的时候 rect会为0，0，1，1
            if (rect.position != Vector2.zero)
            {
                defaultPos.x = rect.position.x + rect.width;
                defaultPos.y = rect.position.y + rect.height / 2;
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

        public override void Pass(params object[] objs)
        {
            base.Pass(objs);

            runtimePassedConditionIndex = (int)objs[0];
        }
    }
}


public static class MyEditorLayout
{
    public static void Label(string txt,out Rect rect)
    {
        GUIContent content = new GUIContent(txt);
        rect = GUILayoutUtility.GetRect(content, EditorStyles.label);
        GUI.Label(rect, txt, EditorStyles.label);
    }

    public static void Label(string txt, GUIStyle style, out Rect rect)
    {
        GUIContent content = new GUIContent(txt);
        rect = GUILayoutUtility.GetRect(content, style);
        GUI.Label(rect, txt, style);
    }

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