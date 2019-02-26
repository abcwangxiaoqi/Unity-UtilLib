using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectParseEditor.Normal
{
    public interface Idraw
    {
        object Draw();
    }

    public abstract class baseDraw<T> : Idraw
    {
        public baseDraw(object org)
        {
            value = (T)org;
        }

        protected T value;
        public abstract object Draw();
    }
}
