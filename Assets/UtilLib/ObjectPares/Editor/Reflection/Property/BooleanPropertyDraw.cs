using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using ObjectParse;

namespace ObjectParseEditor.Reflection
{
    public class BooleanPropertyDraw : BasePropertyDraw<bool>
    {
        public BooleanPropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref bool v)
        {
            base.DrawDetial(ref v);

            v = GUILayout.Toggle(v, info.Name);
        }
    }
}
