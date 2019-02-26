using ObjectParse;
using System;
using System.Reflection;

namespace ObjectParseEditor
{
    public class FieldClassDraw : ClassDraw
    {
        //反射中取得class对象 的构造
        public FieldClassDraw(FieldInfo field, NestMap parent)
        {
            object org = parent.GetValue(field);
            if (org == null)
            {
                // 对象里面的class对象 为空的话 新建一个对象赋值，使其不为空
                org = Activator.CreateInstance(field.FieldType);
            }

            nest = new NestMap(parent, field, org);
        }
    }
}
