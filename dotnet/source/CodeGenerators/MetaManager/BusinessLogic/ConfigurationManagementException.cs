using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.BusinessLogic
{
    public class ConfigurationManagementException : Exception
    {
        public ConfigurationManagementException(string message) : base(message) { ;}
        public ConfigurationManagementException(string message, Exception inner) : base(message, inner) { ;}

    }
}

