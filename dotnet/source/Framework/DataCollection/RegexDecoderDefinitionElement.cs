using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Imi.Framework.DataCollection
{
    public class RegexDecoderDefinitionElement : ConfigurationElement
    {
        [ConfigurationProperty("applicationIdentifier")]
        public string ApplicationIdentifier
        {
            get { return (string)this["applicationIdentifier"]; }
            set { this["applicationIdentifier"] = value; }
        }

        [ConfigurationProperty("applicationIdentifierName")]
        public string ApplicationIdentifierName
        {
            get { return (string)this["applicationIdentifierName"]; }
            set { this["applicationIdentifierName"] = value; }
        }

        [ConfigurationProperty("expression")]
        public string Expression
        {
            get { return (string)this["expression"]; }
            set { this["expression"] = value; }
        }
    }
}
