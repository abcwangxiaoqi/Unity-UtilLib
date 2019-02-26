using System.Reflection;
using UnityEngine;
using ObjectParse;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class DoublePropertyDraw : BasePropertyDraw<double>
    {
        public DoublePropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref double v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.DoubleField(v);
        }
    }
}
