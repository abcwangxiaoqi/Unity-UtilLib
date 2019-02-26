using ObjectParse;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class FloatFieldDraw : BaseFieldDraw<float>
    {
        public FloatFieldDraw(FieldInfo info, NestMap tar) : base(info, tar) { }


        protected override void DrawDetial(ref float v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.FloatField(v);
        }
    }
}
