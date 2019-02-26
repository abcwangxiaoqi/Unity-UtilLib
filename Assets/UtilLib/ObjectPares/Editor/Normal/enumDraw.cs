using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace ObjectParseEditor.Normal
{
    public class enumDraw : baseDraw<Enum>
    {
        public enumDraw(object org) : base(org) { }

        public override object Draw()
        {
            value = EditorGUILayout.EnumPopup(value);
            return value;
        }
    }
}
