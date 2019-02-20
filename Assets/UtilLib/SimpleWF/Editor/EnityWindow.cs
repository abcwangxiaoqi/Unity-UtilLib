using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityWindow : BaseWindow
{
    static GUIStyle EntranceStyle;

    static EntityWindow()
    {
        EntranceStyle = new GUIStyle(UnityEditor.EditorStyles.boldLabel);
        EntranceStyle.fixedHeight = 25;
        EntranceStyle.fontSize = 20;
        EntranceStyle.alignment = TextAnchor.MiddleCenter;
        EntranceStyle.normal.textColor = Color.green;        
    }

    //是否是入口
    public bool isEntrance { get; private set; }

    public string ClassName { get; private set; }

    //下一节点
    public BaseWindow next { get; protected set; }

    public override WindowType windowType
    {
        get
        {
            return WindowType.Entity;
        }
    }

    public EntityWindow(Vector2 pos, WFEditorWindow _mainWindow)
        : base(pos, _mainWindow)
    {
        Name = "Entity";
    }

    public EntityWindow(WindowDataEntity itemData, WFEditorWindow _mainWindow)
        : base(itemData, _mainWindow)
    {
        ClassName = itemData.className;
        isEntrance = itemData.isEntrance;
    }
    
    public void SetNext(BaseWindow entity)
    {
        next = entity;
    }

    public override WindowDataBase GetData()
    {
        WindowDataEntity dataEntity = new WindowDataEntity();
        dataEntity.position = position;
        dataEntity.name = Name;
        dataEntity.id = Id;

        dataEntity.className = ClassName;

        dataEntity.isEntrance = isEntrance;

        if(next!=null)
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
            if (!mainWindow.windowList.Contains(next))
            {
                next = null;
                return;
            }

            DrawArrow(Out, next.In, Color.white);
        }
    }

    protected override void gui(int id)
    {        
        base.gui(id);

        int selectindex = -1;

        if (!string.IsNullOrEmpty(ClassName))
        {
            selectindex = mainWindow.allEntityClass.IndexOf(ClassName);
        }

        selectindex = EditorGUILayout.Popup(selectindex, mainWindow.allEntityClass.ToArray(), popupStyle);
        if (selectindex >= 0)
        {
            ClassName = mainWindow.allEntityClass[selectindex];
        }

        if (isEntrance)
        {
            GUILayout.Space(5);
            GUILayout.Label("Entrance", EntranceStyle);
        }

        GUI.DragWindow();
    }

    public override void rightMouseDraw(Vector2 mouseposition)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Next/New Entity"), false, () =>
        {
            var tempWindow = new EntityWindow(mouseposition, mainWindow);
            mainWindow.windowList.Add(tempWindow);
            next = tempWindow;
        });

        menu.AddItem(new GUIContent("Next/New Condition"), false, () =>
        {
            var tempWindow = new RouterWindow(mouseposition, mainWindow);
            mainWindow.windowList.Add(tempWindow);
            next = tempWindow;
        });

        #region 选择下一个
        List<BaseWindow> selectionList = new List<BaseWindow>();

        foreach (var item in mainWindow.windowList)
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
                if(select)
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
        
        if(isEntrance)
        {//入口函数不能删除
            menu.AddDisabledItem(new GUIContent("Delte"));
        }
        else
        {
            menu.AddItem(new GUIContent("Delte"), false, () =>
            {
                mainWindow.windowList.Remove(this);
            });
        } 

        menu.AddItem(new GUIContent("Set Entrance"), isEntrance, () =>
        {
            foreach (var item in mainWindow.windowList)
            {
                if (item is EntityWindow)
                {
                    (item as EntityWindow).isEntrance = false;
                }
            }

            isEntrance = true;
        });

        menu.ShowAsContext();
    }
}