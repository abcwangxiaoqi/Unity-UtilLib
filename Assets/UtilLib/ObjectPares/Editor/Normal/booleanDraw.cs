using UnityEngine;

namespace ObjectParseEditor.Normal
{
    public class booleanDraw : baseDraw<bool>
    {
        public booleanDraw(object org) : base(org) { }

        public override object Draw()
        {
            value = GUILayout.Toggle(value, "");
            return value;
        }
    }
}
