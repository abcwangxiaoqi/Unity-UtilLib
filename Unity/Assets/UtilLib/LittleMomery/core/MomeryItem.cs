using UnityEngine;
using System;

namespace LittleMomery
{
    public class MomeryItem
    {
        public Type type;
        public string key;
        public object val;

        public virtual void unload()
        {
            val = null;
        }
    }
}
