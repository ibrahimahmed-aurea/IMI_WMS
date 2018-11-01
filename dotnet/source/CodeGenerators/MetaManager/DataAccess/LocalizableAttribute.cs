using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, Inherited=true)]
    public class LocalizableAttribute : Attribute
    {
        public LocalizableAttribute()
        {
            
        }
    }
}
