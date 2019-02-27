
using System.Collections.Generic;
using EditorTools;
using UnityEditor;
using UnityEngine;

namespace ScriptNodeFlow
{
    public class EditorNodeCanvas : NodeCanvas
    {
        [MenuItem("Assets/Script Node Flow/Edit", true)]
        static bool ValidateSelection()
        {
            Object asset = Selection.activeObject;

            return (asset is NodeCanvasData);
        }

        [MenuItem("Assets/Script Node Flow/Edit", false, priority = 49)]
        static void Edit()
        {
            if (window != null)
                window.Close();

            window = null;


            Object asset = Selection.activeObject;
            scriptable = new ScriptableItem(AssetDatabase.GetAssetPath(asset));
            window = GetWindow<EditorNodeCanvas>(asset.name);
        }

        [MenuItem("Assets/Script Node Flow/Create", false, priority = 49)]
        static void New()
        {
            EditorUtil.CreatAssetCurPath<NodeCanvasData>("New Node Canvas",
                (NodeCanvasData data, Dictionary<string, object> dic) =>
                {
                    //create a default Node
                    NodeData entity = new NodeData();
                    entity.isEntrance = true;
                    entity.id = 0;
                    entity.position = new Vector2(300, 300);
                    entity.name = "Entrance Node";
                    data.nodelist.Add(entity);
                });
        }

        public static void Open(Object obj)
        {
            if (window != null)
                window.Close();

            window = null;

            scriptable = new ScriptableItem(AssetDatabase.GetAssetPath(obj));
            window = GetWindow<EditorNodeCanvas>(obj.name);
        }

        static ScriptableItem scriptable;

        static GUIContent addNode = new GUIContent("Add Node");
        static GUIContent addRouter = new GUIContent("Add Router");
        static GUIContent comiling = new GUIContent("...Comiling...");

        NodeCanvasData wdata;

        
        protected override void Awake()
        {
            EditorApplication.playModeStateChanged -= playModeStateChanged;
            EditorApplication.playModeStateChanged += playModeStateChanged;
           
            wdata = scriptable.Load<NodeCanvasData>();

            generateWindowData(wdata);
        }

        //close window when playModeStateChanged
        private void playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingEditMode)
            {
                window.Close();
            }
        }

        BaseWindow curSelect = null;
        Event curEvent;
        Vector2 mousePosition;
             

        // the key of the asset's path
        // need be saved when compiling
        string nodeAssetPath = "NODEASSETPATH";

        protected override void OnGUI()
        {           
            if (EditorApplication.isCompiling)
            {
                ShowNotification(comiling);

                if(!EditorPrefs.HasKey(nodeAssetPath))
                {
                    EditorPrefs.SetString(nodeAssetPath, scriptable.path);
                }

                return;
            }

            if(EditorPrefs.HasKey(nodeAssetPath))
            {
                // once compiled
                string path = EditorPrefs.GetString(nodeAssetPath);
                EditorPrefs.DeleteKey(nodeAssetPath);

                scriptable = new ScriptableItem(path);

                Awake();

                Repaint();
            }

            if (windowList == null)
                return;

            curEvent = Event.current;
            mousePosition = curEvent.mousePosition;

            if (curEvent.button == 1) // mouse right key
            {
                ShowMenu();
            }

            // mouse left key
            if (curEvent.button == 0 && curEvent.isMouse)
            {
                //a window is whether selected
                if (curEvent.type == EventType.MouseDown)
                {
                    curSelect = windowList.Find((BaseWindow w) =>
                    {
                        return w.isClick(mousePosition);
                    });
                }
                else if (curEvent.type == EventType.MouseUp)
                {
                    curSelect = null;
                }
                
                if (curSelect != null)
                {
                    curSelect.leftMouseDraw(curEvent);
                }
                //else
                //{
                //    if (this.position.Contains(curEvent.mousePosition))
                //    {
                //        //drag the panel
                //        foreach (var item in windowList)
                //        {
                //            item.leftMouseDraw(curEvent);
                //        }
                //    }
                //}
            }

            base.OnGUI();
        }

        protected override void OnLostFocus()
        {
            save(false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            save(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            save(true);
        }

        private void ShowMenu()
        {
            // add a new Node
            if (curEvent.type == EventType.MouseDown)
            {
                curSelect = windowList.Find((BaseWindow w) =>
                {
                    return w.isClick(mousePosition);
                });

                if (curSelect != null)
                {
                    curSelect.rightMouseDraw(mousePosition);
                }
                else
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(addNode, false, () =>
                    {
                        windowList.Add(new NodeWindow(mousePosition, windowList));
                    });

                    menu.AddItem(addRouter, false, () =>
                    {
                        windowList.Add(new RouterWindow(mousePosition, windowList));
                    });
                    menu.ShowAsContext();
                }
            }
        }

        void save(bool import)
        {
            if (windowList == null)
                return;

            wdata.nodelist.Clear();
            wdata.routerlist.Clear();


            wdata.shareData = fixedWindow.shareData;


            for (int i = 0; i < windowList.Count; i++)
            {
                if (windowList[i].windowType == NodeType.Node)
                {
                    wdata.nodelist.Add((NodeData)windowList[i].GetData());
                }
                else
                {
                    wdata.routerlist.Add((RouterData)windowList[i].GetData());
                }
            }

            if(import)
            {
                scriptable.SaveAsset(wdata);
            }
        }
    }   
}