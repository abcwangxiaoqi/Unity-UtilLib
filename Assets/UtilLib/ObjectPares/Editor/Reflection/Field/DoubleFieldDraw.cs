using ObjectParse;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class DoubleFieldDraw : BaseFieldDraw<double>
    {
        public DoubleFieldDraw(FieldInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref double v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.DoubleField(v);
        }
    }
}
