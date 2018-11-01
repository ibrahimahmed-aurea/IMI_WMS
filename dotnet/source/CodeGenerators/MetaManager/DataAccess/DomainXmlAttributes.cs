using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DomainXmlIgnoreAttribute : System.Attribute
    {
        public DomainXmlIgnoreAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DomainXmlByIdAttribute : System.Attribute
    {
        public DomainXmlByIdAttribute() { }
    }
}
