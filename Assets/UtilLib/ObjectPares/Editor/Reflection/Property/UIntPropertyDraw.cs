using System.Reflection;
using UnityEngine;
using ObjectParse;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class UIntPropertyDraw : BasePropertyDraw<uint>
    {
        public UIntPropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref uint v)
        {
            base.DrawDetial(ref v);

            int uintV = EditorGUILayout.IntField((int)v);
            v = uintV < 0 ? 0 : (uint)uintV;
        }
    }
}
