using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ObjectParse
{
    //记录反射类之间的嵌套关系 数据结构
    public class NestMap
    {
        public NestMap parent { get; private set; }
        public FieldInfo field { get; private set; }
        public PropertyInfo property { get; private set; }
        public object self { get; private set; }
        public string path { get; private set; }

        public NestMap(object _self)
        {
            this.self = _self;
            iniPath();
        }

        public NestMap(NestMap _parent, FieldInfo _field, object _self)
        {
            this.parent = _parent;
            this.field = _field;
            this.self = _self;

            iniPath();
        }

        public NestMap(NestMap _parent, PropertyInfo _property, object _self)
        {
            this.parent = _parent;
            this.property = _property;
            this.self = _self;

            iniPath();
        }

        private void iniPath()
        {
            //parent为null 则field和property都为空
            if (parent == null)
            {
                path = "ROOT";
                return;
            }

            string selfPath = null;

            if (field != null)
            {
                selfPath = field.Name;
            }
            else
            {
                selfPath = property.Name;
            }
            string parentPath = null;

            parentPath = parent.path;

            path = string.Format("{0}.{1}", parentPath, selfPath);            
        }

        #region FieldInfo

        public void SetValue(FieldInfo info, object value)
        {
            if (null == info)
                return;

            info.SetValue(self, value);

            //嵌套循环更新
            if (parent != null)
            {
                parent.SetValue(field, self);
            }
        }

        public T GetValue<T>(FieldInfo info)
        {
            return (T)info.GetValue(self);
        }

        public object GetValue(FieldInfo info)
        {
            return info.GetValue(self);
        }

        #endregion

        #region PropertyInfo
        public void SetValue(PropertyInfo info, object value, object[] index)
        {
            info.SetValue(self, value, index);

            //嵌套循环更新
            if (parent != null)
            {
                parent.SetValue(property, self, null);
            }
        }

        public T GetValue<T>(PropertyInfo info, object[] index)
        {
            return (T)info.GetValue(self, index);
        }

        public object GetValue(PropertyInfo info, object[] index)
        {
            return info.GetValue(self, index);
        }
        #endregion
    }
}
