using System.Reflection;
using UnityEngine;
using ObjectParse;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class LongPropertyDraw : BasePropertyDraw<long>
    {
        public LongPropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar) { }


        protected override void DrawDetial(ref long v)
        {
            base.DrawDetial(ref v);

            v = EditorGUILayout.LongField(v);
        }
    }
}
