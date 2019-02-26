using System.Reflection;
using UnityEngine;
using ObjectParse;
using UnityEditor;

namespace ObjectParseEditor.Reflection
{
    public class UIntFieldDraw : BaseFieldDraw<uint>
    {
        public UIntFieldDraw(FieldInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref uint v)
        {
            base.DrawDetial(ref v);

            int uintV = EditorGUILayout.IntField((int)v);
            v = uintV < 0 ? 0 : (uint)uintV;
        }
    }
}
