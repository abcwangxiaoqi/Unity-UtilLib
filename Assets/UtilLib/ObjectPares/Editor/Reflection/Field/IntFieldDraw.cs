using ObjectParse;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class IntFieldDraw : BaseFieldDraw<int>
    {
        public IntFieldDraw(FieldInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref int v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.IntField(v);
        }
    }
}
