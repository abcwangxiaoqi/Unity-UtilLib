using UnityEditor;
using UnityEngine;

namespace ObjectParseEditor.Normal
{
    public class longDraw : baseDraw<long>
    {
        public longDraw(object org) : base(org) { }

        public override object Draw()
        {
            value = EditorGUILayout.LongField(value);
            return value;
        }
    }
}
