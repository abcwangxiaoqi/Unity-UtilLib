using UnityEditor;
using UnityEngine;

namespace ObjectParseEditor.Normal
{
    public class doubleDraw : baseDraw<double>
    {
        public doubleDraw(object org) : base(org) { }

        public override object Draw()
        {
            value = EditorGUILayout.DoubleField(value);
            return value;
        }
    }
}
