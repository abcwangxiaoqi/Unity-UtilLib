using System.Reflection;
using UnityEngine;
using ObjectParse;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class FloatPropertyDraw : BasePropertyDraw<float>
    {
        public FloatPropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar) { }


        protected override void DrawDetial(ref float v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.FloatField(v);
        }
    }
}
