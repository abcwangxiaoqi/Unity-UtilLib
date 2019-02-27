using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ScriptNodeFlow
{
    public class NodeWindow : BaseWindow
    {
        static List<string> allEntityClass = new List<string>();

        static GUIContent nextNewNodeContent = new GUIContent("Next/New Node");
        static GUIContent nextNewRouterContent = new GUIContent("Next/New Router");
        static GUIContent deleteContent = new GUIContent("Delte");
        static GUIContent entranceContent = new GUIContent("Set Entrance");
        static string separator = "Next/";

        static NodeWindow()
        {
            Assembly _assembly = Assembly.LoadFile("Library/ScriptAssemblies/Assembly-CSharp.dll");
            Type[] tys = _assembly.GetTypes();

            foreach (var item in tys)
            {
                if (item.IsSubclassOf(typeof(Node)) && !item.IsInterface && !item.IsAbstract)
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

        Vector2 _size = new Vector2(150, 100);
        protected override Vector2 size
        {
            get
            {
                return _size;
            }
        }


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

            classIndex = allEntityClass.IndexOf(ClassName);
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

            //draw line
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

        int classIndex = -1;

        protected override void gui(int id)
        {
            base.gui(id);

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

        GenericMenu menu;

        public override void rightMouseDraw(Vector2 mouseposition)
        {
            GenericMenu menu = new GenericMenu();


            menu.AddItem(nextNewNodeContent, false, () =>
            {
                var tempWindow = new NodeWindow(mouseposition, windowList);
                windowList.Add(tempWindow);
                next = tempWindow;
            });

            menu.AddItem(nextNewRouterContent, false, () =>
            {
                var tempWindow = new RouterWindow(mouseposition, windowList);
                windowList.Add(tempWindow);
                next = tempWindow;
            });

            menu.AddSeparator(separator);

            #region select the next one
            List<BaseWindow> selectionList = new List<BaseWindow>();

            foreach (var item in windowList)
            {
                if (item.Id == Id)
                    continue;
                selectionList.Add(item);
            }

            foreach (var item in selectionList)
            {
                bool select = (next != null) && next.Id == item.Id;

                menu.AddItem(new GUIContent(string.Format("Next/[{0}][{1}] {2}", item.Id, item.windowType, item.Name))
                             , select, () =>
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
            {//the Node is Entrance can not be delete
                menu.AddDisabledItem(deleteContent);
            }
            else
            {
                menu.AddItem(deleteContent, false, () =>
                {
                    windowList.Remove(this);
                });
            }

            menu.AddItem(entranceContent, isEntrance, () =>
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
