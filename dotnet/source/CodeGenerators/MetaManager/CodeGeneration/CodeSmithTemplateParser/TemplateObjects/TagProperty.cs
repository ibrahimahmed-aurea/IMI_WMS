using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects
{
    public class TagProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Optional { get; set; }
    }
}
