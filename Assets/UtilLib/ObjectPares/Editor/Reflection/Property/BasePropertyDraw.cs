using System.Reflection;
using UnityEngine;
using ObjectParse;

namespace ObjectParseEditor.Reflection
{
    public class BasePropertyDraw<T> : IDraw
    {
        protected PropertyInfo info;
        protected NestMap tar;
        public BasePropertyDraw(PropertyInfo _info, NestMap _tar)
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

            T v = tar.GetValue<T>(info, null);

            DrawDetial(ref v);

            if (info.CanWrite)
            {
                tar.SetValue(info, v, null);
            }

            GUILayout.EndHorizontal();

        }
    }
}
