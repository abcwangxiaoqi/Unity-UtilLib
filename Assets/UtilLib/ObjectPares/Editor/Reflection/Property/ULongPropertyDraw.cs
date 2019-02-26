using System.Reflection;
using UnityEngine;
using ObjectParse;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class ULongPropertyDraw : BasePropertyDraw<ulong>
    {
        public ULongPropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar) { }


        protected override void DrawDetial(ref ulong v)
        {
            base.DrawDetial(ref v);

            long ulongV = EditorGUILayout.LongField((long)v);

            v = ulongV < 0 ? 0 : (ulong)ulongV;
        }
    }
}
