
using System.Collections.Generic;
using EditorTools;
using UnityEditor;
using UnityEngine;

namespace NodeTool
{
    public class EditorNodeCanvas : NodeCanvas
    {
        [MenuItem("Assets/Node Canvas/Edit", true)]
        static bool ValidateSelection()
        {
            Object asset = Selection.activeObject;

            return (asset is NodeCanvasData);
        }

        [MenuItem("Assets/Node Canvas/Edit", false, priority = 49)]
        static void Edit()
        {
            if (window != null)
                window.Close();

            window = null;


            Object asset = Selection.activeObject;
            scriptable = new ScriptableItem(AssetDatabase.GetAssetPath(asset));
            window = GetWindow<EditorNodeCanvas>(asset.name);
        }

        [MenuItem("Assets/Node Canvas/Create", false, priority = 49)]
        static void New()
        {
            EditorUtil.CreatAssetCurPath<NodeCanvasData>("NewNodeCanvas",
                (NodeCanvasData data, Dictionary<string, object> dic) =>
                {
                    //默认数据 且是 入口实例
                    NodeData entity = new NodeData();
                    entity.isEntrance = true;
                    entity.id = 0;
                    entity.position = new Vector2(50, 50);
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

            windowList = new List<BaseWindow>();

            wdata = scriptable.Load<NodeCanvasData>();

            generateWindowData(wdata);
        }

        private void playModeStateChanged(PlayModeStateChange obj)
        {
            //开始运行时 关闭窗口
            if (obj == PlayModeStateChange.ExitingEditMode)
            {
                window.Close();
            }
        }

        BaseWindow curSelect = null;
        Event curEvent;
        Vector2 mousePosition;

        //当窗口打开 修改了代码 编译前保存资源的url
        //编译完成后 再次加载资源
        //存储key
        string prefKey = "prefKey";

        protected override void OnGUI()
        {
            if (windowList == null)
                return;
            
            if (EditorApplication.isCompiling)
            {
                ShowNotification(comiling);

                if(!EditorPrefs.HasKey(prefKey))
                {
                    EditorPrefs.SetString(prefKey, scriptable.path);
                }

                return;
            }


            if(EditorPrefs.HasKey(prefKey))
            {
                //窗口没关闭 刚编译完成 回执行这里
                string path = EditorPrefs.GetString("prefKey");
                EditorPrefs.DeleteKey(prefKey);

                scriptable = new ScriptableItem(path);

                Awake();

                Repaint();
            }

            curEvent = Event.current;
            mousePosition = curEvent.mousePosition;

            if (curEvent.button == 1) // 鼠标右键
            {
                ShowMenu();
            }

            // 鼠标左键
            if (curEvent.button == 0 && curEvent.isMouse)
            {
                //判断选择
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

                //窗体移动位置
                if (curSelect != null)
                {
                    curSelect.leftMouseDraw(curEvent);
                }
                else
                {
                    if(this.position.Contains(curEvent.mousePosition))
                    {
                        foreach (var item in windowList)
                        {
                            item.leftMouseDraw(curEvent);
                        }
                    }
                }
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
            // 添加一个新节点
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