using ObjectParse;
using System;
using System.Reflection;

namespace ObjectParseEditor
{
    public class PropertyClassDraw : ClassDraw
    {
        //反射中取得class对象 的构造
        public PropertyClassDraw(PropertyInfo property, NestMap parent)
        {
            object org = parent.GetValue(property, null);
            if (org == null)
            {
                // 对象里面的class对象 为空的话 新建一个对象赋值，使其不为空
                org = Activator.CreateInstance(property.PropertyType);
            }

            nest = new NestMap(parent, property, org);
        }
    }
}
