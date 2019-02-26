using UnityEditor;
using UnityEngine;

namespace ObjectParseEditor.Normal
{
    public class ulongDraw : baseDraw<ulong>
    {
        public ulongDraw(object org) : base(org) { }

        public override object Draw()
        {
            long ulongV = EditorGUILayout.LongField((long)value);

            value = ulongV < 0 ? 0 : (ulong)ulongV;
            
            return value;
        }
    }
}
