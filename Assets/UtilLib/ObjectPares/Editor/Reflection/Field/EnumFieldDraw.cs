using ObjectParse;
using System;
using System.Reflection;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class EnumFieldDraw : BaseFieldDraw<Enum>
    {
        public EnumFieldDraw(FieldInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref Enum v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.EnumPopup(v);
        }
    }
}
