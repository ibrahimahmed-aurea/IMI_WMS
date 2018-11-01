using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    public enum PropertyMapType { Request, Response, Other };
    public enum SetTargetChoice { Yes, No};

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyMapAttribute : Attribute
    {
        public PropertyMapAttribute()
        {
            Type = PropertyMapType.Other;
            SetTarget = SetTargetChoice.Yes;
        }

        public PropertyMapType Type;
        public SetTargetChoice SetTarget;
    }

}
