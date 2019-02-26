using ObjectParse;
using System.Reflection;
using UnityEngine;

namespace ObjectParseEditor.Reflection
{
    public class BooleanFieldDraw : BaseFieldDraw<bool>
    {
        public BooleanFieldDraw(FieldInfo info, NestMap tar) : base(info, tar) { }

        protected override void DrawDetial(ref bool v)
        {
            base.DrawDetial(ref v);

            v = GUILayout.Toggle(v, info.Name);
        }
    }
}
