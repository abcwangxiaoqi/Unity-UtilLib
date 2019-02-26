using System;
using System.Reflection;
using UnityEditor;
using ObjectParse;

namespace ObjectParseEditor.Reflection
{
    public class EnumPropertyDraw : BasePropertyDraw<Enum>
    {
        public EnumPropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref Enum v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.EnumPopup(v);
        }
    }
}
