using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ObjectParseEditor.Normal
{
    public class floatDraw : baseDraw<float>
    {
        public floatDraw(object org) : base(org) { }

        public override object Draw()
        {
            value = EditorGUILayout.FloatField(value);
            return value;
        }
    }
}
