using ObjectParse;
using ObjectParseEditor.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ObjectParseEditor
{
    public interface IDraw
    {
        void Draw();
    }

    public class ClassDraw : IDraw
    {
        static GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
        static ClassDraw()
        {
            labelStyle.fontStyle = FontStyle.Bold;
        }

        protected NestMap nest = null;

        //true展开  false不展开
        //默认为false
        protected bool State = false;        

        public ClassDraw() { }

        //直接有对象 的构造
        public ClassDraw(object org)
        {
            nest = new NestMap(org);
        }

        public void Draw()
        {
            drawClass(nest);
        }

        void drawClass(NestMap tar)
        {
            if (null == tar)
                return;

            Type type = tar.self.GetType();
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();


            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(State ? "-" : "+", GUILayout.Width(20)))
            {
                State = !State;
            }
            GUI.backgroundColor = Color.white;

            GUILayout.Label(type.Name, labelStyle);

            if (tar.field != null)
            {
                GUILayout.Label(tar.field.Name, labelStyle);
            }
            else if (tar.property != null)
            {
                GUILayout.Label(tar.property.Name, labelStyle);
            }

            GUILayout.EndHorizontal();

            if (State)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Space(30);

                GUILayout.BeginVertical();

                MemberInfo[] memberInfos = type.GetMembers();
                foreach (var item in memberInfos)
                {
                    #region BrowsableAttribute 为false 系统默认 不可见 所以要处理
                    var browsableAttribute = (BrowsableAttribute[])item.GetCustomAttributes(typeof(BrowsableAttribute), false);

                    if (browsableAttribute != null &&
                        browsableAttribute.Length > 0 &&
                        browsableAttribute[0].Browsable == false)
                    {
                        continue;
                    }
                    #endregion

                    if (item.MemberType == MemberTypes.Field)
                    {
                        drawField((FieldInfo)item, tar);
                    }
                    else if (item.MemberType == MemberTypes.Property)
                    {
                        drawProperty((PropertyInfo)item, tar);
                    }
                    else
                    {
                        continue;
                    }
                    GUILayout.Space(8);
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        //这个map的主要作用是用来 不会每次都实例化一个class 
        //缓存了class后 同时缓存了 里面member的状态 例如:展开和收起的状态
        protected Dictionary<string, IDraw> drawMap = new Dictionary<string, IDraw>();

        protected virtual void drawField(FieldInfo info, NestMap tar)
        {
            GUILayout.Space(8);

            Type fieldType = info.FieldType;


            string key = string.Format("{0}.{1}", tar.path, info.Name);

            IDraw draw = null;

            if (drawMap.ContainsKey(key))
            {
                drawMap[key].Draw();
                return;
            }

            if (fieldType.Equals(typeof(int)))
            {
                draw = new IntFieldDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(uint)))
            {
                draw = new UIntFieldDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(long)))
            {
                draw = new LongFieldDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(ulong)))
            {
                draw = new ULongFieldDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(string)))
            {
                draw = new StrFieldDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(float)))
            {
                draw = new FloatFieldDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(double)))
            {
                draw = new DoubleFieldDraw(info, tar);
            }
            else if (fieldType.BaseType.Equals(typeof(Enum)))
            {
                draw = new EnumFieldDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(bool)))
            {
                draw = new BooleanFieldDraw(info, tar);
            }
            else if (tar.GetValue(info) is IList)
            {
                draw = new ListFieldDraw(info, tar);
            }
            else if (fieldType.BaseType.Equals(typeof(Array)))
            {
                // 目前不支持Array类型
                // 用list代替Array类型        
                return;
            }
            else
            {
                draw = new FieldClassDraw(info, tar);
            }
            drawMap.Add(key, draw);
            draw.Draw();
        }

        protected virtual void drawProperty(PropertyInfo info, NestMap tar)
        {
            Type fieldType = info.PropertyType;

            string key = string.Format("{0}.{1}", tar.path, info.Name);

            IDraw draw = null;

            if (drawMap.ContainsKey(key))
            {
                drawMap[key].Draw();
                return;
            }

            if (fieldType.Equals(typeof(int)))
            {
                draw = new IntPropertyDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(uint)))
            {
                draw = new UIntPropertyDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(long)))
            {
                draw = new LongPropertyDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(ulong)))
            {
                draw = new ULongPropertyDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(string)))
            {
                draw = new StrPropertyDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(float)))
            {
                draw = new FloatPropertyDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(double)))
            {
                draw = new DoublePropertyDraw(info, tar);
            }
            else if (fieldType.BaseType.Equals(typeof(Enum)))
            {
                draw = new EnumPropertyDraw(info, tar);
            }
            else if (fieldType.Equals(typeof(bool)))
            {
                draw = new BooleanPropertyDraw(info, tar);
            }
            else if (tar.GetValue(info, null) is IList)
            {
                draw = new ListPropertyDraw(info, tar);
            }
            else if (fieldType.BaseType.Equals(typeof(Array)))
            {
                // 目前不支持Array类型
                // 用list代替Array类型
                return;
            }
            else//当做普通class来处理
            {
                draw = new PropertyClassDraw(info, tar);
            }
            drawMap.Add(key, draw);
            draw.Draw();
        }
    }
}
