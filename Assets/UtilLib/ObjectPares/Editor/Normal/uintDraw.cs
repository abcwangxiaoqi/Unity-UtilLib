using UnityEditor;
using UnityEngine;

namespace ObjectParseEditor.Normal
{
    public class uintDraw : baseDraw<uint>
    {
        public uintDraw(object org) : base(org) { }

        public override object Draw()
        {
            int uintV = EditorGUILayout.IntField((int)value);
            value = uintV < 0 ? 0 : (uint)uintV;
            return value;
        }
    }
}
