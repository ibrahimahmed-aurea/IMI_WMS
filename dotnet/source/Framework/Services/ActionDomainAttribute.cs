using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.Services
{
    [global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    public sealed class ActionDomainAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string name;

        // This is a positional argument
        public ActionDomainAttribute(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
    }
}
