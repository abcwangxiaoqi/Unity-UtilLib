using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectParseEditor.Normal
{
    public class classDraw : baseDraw<object>
    {
        ClassDraw listDraw = null;
        public classDraw(object org)
            : base(org)
        {
            listDraw = new ClassDraw(org);
        }

        public override object Draw()
        {
            listDraw.Draw();
            return value;
        }
    }
}
