using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.HbmGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ClassStorageHintAttribute : Attribute
    {
        public ClassStorageHintAttribute()
        {
        }

        public string TableName { get; set; }
    }
}
