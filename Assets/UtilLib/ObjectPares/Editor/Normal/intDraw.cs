using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ObjectParseEditor.Normal
{
    public class intDraw : baseDraw<int>
    {
        public intDraw(object org) : base(org) { }

        public override object Draw()
        {
            Debug.Log(">>>>>>>dafdafas");
            value = EditorGUILayout.IntField(value);
            return value;
        }
    }
}
