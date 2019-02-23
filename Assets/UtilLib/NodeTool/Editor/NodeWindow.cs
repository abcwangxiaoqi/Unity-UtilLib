using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NodeTool
{
    public class NodeWindow : BaseWindow
    {
        static List<string> allEntityClass = new List<string>();

        static NodeWindow()
        {
            Assembly _assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp.dll");
            Type[] tys = _assembly.GetTypes();

            foreach (var item in tys)
            {
                if (item.IsSubclassOf(typeof(BaseNode)) && !item.IsInterface && !item.IsAbstract)
                {
                    allEntityClass.Add(item.FullName);
                }
            }
        }

        //是否是入口
        public bool isEntrance { get; private set; }

        protected string ClassName { get; private set; }

        //下一节点
        public BaseWindow next { get; protected set; }

        public override NodeType windowType
        {
            get
            {
                return NodeType.Node;
            }
        }

        public NodeWindow(Vector2 pos, List<BaseWindow> _windowList)
            : base(pos, _windowList)
        {
            Name = "Node";
        }

        public NodeWindow(NodeData itemData, List<BaseWindow> _windowList)
            : base(itemData, _windowList)
        {
            ClassName = itemData.className;
            isEntrance = itemData.isEntrance;
        }

        public void SetNext(BaseWindow entity)
        {
            next = entity;
        }

        public override DataBase GetData()
        {
            NodeData dataEntity = new NodeData();
            dataEntity.position = position;
            dataEntity.name = Name;
            dataEntity.id = Id;

            dataEntity.className = ClassName;

            dataEntity.isEntrance = isEntrance;

            if (next != null)
            {
                dataEntity.next = next.Id;
            }

            return dataEntity;
        }

        public override void draw()
        {
            base.draw();

            //画线
            if (next != null)
            {
                if (!windowList.Contains(next))
                {
                    next = null;
                    return;
                }

                Color color = Color.white;

                if (Application.isPlaying && passed)
                {
                    color = Color.green;
                }

                DrawArrow(Out, next.In, color);
            }
        }

        protected override void gui(int id)
        {
            base.gui(id);

            int classIndex = -1;
            classIndex = allEntityClass.IndexOf(ClassName);

            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            classIndex = EditorGUILayout.Popup(classIndex, allEntityClass.ToArray(), popupStyle);
            EditorGUI.EndDisabledGroup();

            if (classIndex >= 0)
            {
                ClassName = allEntityClass[classIndex];
            }

            if (isEntrance)
            {
                GUILayout.Space(5);
                GUILayout.Label("Entrance", BigLabelStyle);
            }

            GUI.DragWindow();
        }

        public override void rightMouseDraw(Vector2 mouseposition)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Next/New Entity"), false, () =>
            {
                var tempWindow = new NodeWindow(mouseposition, windowList);
                windowList.Add(tempWindow);
                next = tempWindow;
            });

            menu.AddItem(new GUIContent("Next/New Condition"), false, () =>
            {
                var tempWindow = new RouterWindow(mouseposition, windowList);
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
                    if (select)
                    {
                        next = null;
                    }
                    else
                    {
                        next = item;
                    }
                });
            }
            #endregion

            if (isEntrance)
            {//入口函数不能删除
                menu.AddDisabledItem(new GUIContent("Delte"));
            }
            else
            {
                menu.AddItem(new GUIContent("Delte"), false, () =>
                {
                    windowList.Remove(this);
                });
            }

            menu.AddItem(new GUIContent("Set Entrance"), isEntrance, () =>
            {
                foreach (var item in windowList)
                {
                    if (item is NodeWindow)
                    {
                        (item as NodeWindow).isEntrance = false;
                    }
                }

                isEntrance = true;
            });

            menu.ShowAsContext();
        }
    }
}
