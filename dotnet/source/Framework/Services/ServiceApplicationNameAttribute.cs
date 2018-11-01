using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.Services
{
    [global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class ServiceApplicationNameAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string applicationName;

        // This is a positional argument
        public ServiceApplicationNameAttribute(string applicationName)
        {
            this.applicationName = applicationName;
        }

        public string ApplicationName
        {
            get { return applicationName; }
        }
    }
}
