using System.Reflection;
using UnityEngine;
using ObjectParse;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class ULongFieldDraw : BaseFieldDraw<ulong>
    {
        public ULongFieldDraw(FieldInfo info, NestMap tar) : base(info, tar) { }


        protected override void DrawDetial(ref ulong v)
        {
            base.DrawDetial(ref v);

            long ulongV = EditorGUILayout.LongField((long)v);

            v = ulongV < 0 ? 0 : (ulong)ulongV;
        }
    }
}
