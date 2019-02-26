using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ObjectParseEditor.Normal
{
    public class listDraw : baseDraw<IList>
    {
        //true展开 false不展开
        //默认 false
        bool State = false;

        string name;
        public listDraw(object org, string _name) : base(org)
        {
            name = _name;
        }

        public override object Draw()
        {
            if (null == value)
            {
                value = Activator.CreateInstance(typeof(IList)) as IList;
            }

            //获取泛型T的类型
            Type itemType = value.GetType().GetGenericArguments()[0];

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();


            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(State ? "-" : "+", GUILayout.Width(20)))
            {
                State = !State;
            }
            GUI.backgroundColor = Color.white;

            GUILayout.Label(typeof(IList).Name, GUILayout.Width(80));

            if (!string.IsNullOrEmpty(name))
            {
                GUILayout.Label(name, GUILayout.Width(80));
            }

            GUILayout.Label(value.Count.ToString(), GUILayout.Width(80));
            GUILayout.Label(itemType.Name, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            if (State)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                GUILayout.BeginVertical();

                for (int i = 0; i < value.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    DrawListItem(value, i, itemType);


                    GUI.color = Color.red;
                    if (GUILayout.Button("Del", EditorStyles.miniButton, GUILayout.Width(30)))
                    {
                        value.RemoveAt(i);
                        drawMap.RemoveAt(i);
                        i--;
                    }
                    GUI.color = Color.white;

                    GUILayout.EndHorizontal();
                }

                GUI.color = Color.green;
                if (GUILayout.Button("Add", EditorStyles.miniButton))
                {
                    object newObject;
                    if (itemType == typeof(string))
                    {
                        // string没有无参数的默认构造函数 所以这里要特殊处理
                        newObject = Activator.CreateInstance(itemType, new char[] { });
                    }
                    else
                    {
                        newObject = Activator.CreateInstance(itemType);
                    }
                    value.Add(newObject);
                }
                GUI.color = Color.white;

                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();

            return value;
        }

        List<Idraw> drawMap = new List<Idraw>();

        void DrawListItem(IList list, int index, Type itemType)
        {
            GUILayout.Label(index.ToString(), GUILayout.Width(80));

            object item = list[index];

            Idraw draw = null;
            if (drawMap.Count > 0 && drawMap.Count > index)
            {
                draw = drawMap[index];
            }

            if (draw != null)
            {
                list[index] = draw.Draw();
                return;
            }

            if (itemType.Equals(typeof(string)))
            {
                draw = new strDraw(list[index]);
            }
            else if (itemType.Equals(typeof(int)))
            {
                draw = new intDraw(list[index]);
            }
            else if (itemType.Equals(typeof(uint)))
            {
                draw = new uintDraw(list[index]);
            }
            else if (itemType.Equals(typeof(long)))
            {
                draw = new longDraw(list[index]);
            }
            else if (itemType.Equals(typeof(ulong)))
            {
                draw = new ulongDraw(list[index]);
            }
            else if (itemType.Equals(typeof(float)))
            {
                draw = new floatDraw(list[index]);
            }
            else if (itemType.Equals(typeof(double)))
            {
                draw = new doubleDraw(list[index]);
            }
            else if (itemType.Equals(typeof(bool)))
            {
                draw = new booleanDraw(list[index]);
            }
            else if (itemType.Equals(typeof(Enum)))
            {
                draw = new enumDraw(list[index]);
            }
            else if (list[index] is IList)
            {
                draw = new listDraw(list[index], null);
            }
            else//当做普通class来处理
            {
                draw = new classDraw(list[index]);
            }

            list[index] = draw.Draw();
            drawMap.Add(draw);
        }
    }
}
