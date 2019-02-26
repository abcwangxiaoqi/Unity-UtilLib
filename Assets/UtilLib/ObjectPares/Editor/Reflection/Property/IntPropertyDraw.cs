using System.Reflection;
using UnityEngine;
using ObjectParse;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class IntPropertyDraw : BasePropertyDraw<int>
    {
        public IntPropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref int v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.IntField(v);
        }
    }
}
