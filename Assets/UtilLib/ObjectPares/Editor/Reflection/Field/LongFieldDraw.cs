using ObjectParse;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class LongFieldDraw : BaseFieldDraw<long>
    {
        public LongFieldDraw(FieldInfo info, NestMap tar) : base(info, tar) { }


        protected override void DrawDetial(ref long v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.LongField(v);
        }
    }
}
