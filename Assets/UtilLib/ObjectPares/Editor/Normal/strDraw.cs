using UnityEngine;

namespace ObjectParseEditor.Normal
{
    public class strDraw : baseDraw<string>
    {
        public strDraw(object org) : base(org) { }

        public override object Draw()
        {
            value = GUILayout.TextField(value);
            return value;
        }
    }
}
