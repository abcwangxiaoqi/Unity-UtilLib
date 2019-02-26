using ObjectParse;
using ObjectParseEditor.Normal;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace ObjectParseEditor.Reflection
{
    public class ListPropertyDraw : BasePropertyDraw<IList>
    {
        IList list;
        listDraw draw;
        public ListPropertyDraw(PropertyInfo info, NestMap tar) : base(info, tar)
        {
            list = tar.GetValue<IList>(info, null);

            NestMap selfnest = new NestMap(tar, info, list);

            draw = new listDraw(list, info.Name);
        }

        public override void Draw()
        {
            GUILayout.BeginVertical();

            list = (IList)draw.Draw();

            if (info.CanWrite)
            {
                // Draw 直接修改了list 所以这里不用再次赋值
                //tar.SetValue(info, list, null);
            }

            GUILayout.EndVertical();
        }
    }
}
