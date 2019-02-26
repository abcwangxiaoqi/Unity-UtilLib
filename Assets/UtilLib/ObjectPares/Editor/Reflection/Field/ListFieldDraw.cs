using ObjectParse;
using ObjectParseEditor.Normal;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace ObjectParseEditor.Reflection
{
    public class ListFieldDraw : BaseFieldDraw<IList>
    {
        IList list;
        listDraw draw;
        public ListFieldDraw(FieldInfo info, NestMap tar) : base(info, tar)
        {
            list = tar.GetValue<IList>(info);

            NestMap selfnest = new NestMap(tar, info, list);

            draw = new listDraw(list, info.Name);
        }

        public override void Draw()
        {
            GUILayout.BeginVertical();

            list = (IList)draw.Draw();

            // Draw 直接修改了list 所以这里不用再次赋值 因为引用类型
            // tar.SetValue(info, list);

            GUILayout.EndVertical();
        }
    }
}
