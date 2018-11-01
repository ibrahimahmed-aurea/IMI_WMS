using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Imi.Framework.DataCollection
{
    public class RegexBarcodeDecoderSection : ConfigurationSection
    {
        public const string SectionKey = "imi.framework.datacollection.regexbarcodedecoder";

        [ConfigurationProperty("fnc1", IsRequired = true)]
        public int FNC1
        {
            get { return (int)this["fnc1"]; }
            set { this["fnc1"] = value; }
        }

        [ConfigurationProperty("decoderDefinitions", IsRequired = false)]
        public RegexDocoderDefinitionCollection DecoderDefinitions
        {
            get { return (RegexDocoderDefinitionCollection)this["decoderDefinitions"]; }
        }
    }

    public class RegexDocoderDefinitionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RegexDecoderDefinitionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            RegexDecoderDefinitionElement e = (RegexDecoderDefinitionElement)element;

            return e.ApplicationIdentifier + e.Expression;
        }
    }
        
}
