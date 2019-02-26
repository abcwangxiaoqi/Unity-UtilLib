using ObjectParse;
using System.Reflection;
using UnityEngine;

namespace ObjectParseEditor.Reflection
{
    public class StrFieldDraw : BaseFieldDraw<string>
    {
        public StrFieldDraw(FieldInfo info, NestMap tar) : base(info, tar) { }


        protected override void DrawDetial(ref string v)
        {
            base.DrawDetial(ref v);

            v = GUILayout.TextField(v);
        }
    }
}
