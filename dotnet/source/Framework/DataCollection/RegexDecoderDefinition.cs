using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Imi.Framework.DataCollection
{
    public class RegexDecoderDefinition
    {
        public Regex Regex { get; set; }
        public string ApplicationIdentifierName { get; set; }
        public string ApplicationIdentifier { get; set; }
    }
}
