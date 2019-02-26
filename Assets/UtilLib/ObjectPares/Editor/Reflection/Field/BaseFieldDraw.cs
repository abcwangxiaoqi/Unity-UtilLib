using ObjectParse;
using System.Reflection;
using UnityEngine;

namespace ObjectParseEditor.Reflection
{
    public class BaseFieldDraw<T> : IDraw
    {
        protected FieldInfo info;
        protected NestMap tar;
        public BaseFieldDraw(FieldInfo _info, NestMap _tar)
        {
            this.info = _info;
            this.tar = _tar;
        }

        protected virtual void DrawDetial(ref T v)
        {
        }

        public virtual void Draw()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(typeof(T).Name, GUILayout.Width(80));
            GUILayout.Label(info.Name, GUILayout.Width(80));

            T v = tar.GetValue<T>(info);

            DrawDetial(ref v);

            tar.SetValue(info, v);

            GUILayout.EndHorizontal();

        }
    }
}
