using System.Reflection;
using UnityEngine;
using ObjectParse;

namespace ObjectParseEditor.Reflection
{
    public class StrPropertyDraw : BasePropertyDraw<string>
    {
        public StrPropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar) { }


        protected override void DrawDetial(ref string v)
        {
            base.DrawDetial(ref v);

            v = GUILayout.TextField(v);
        }
    }
}
