using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects
{
    public class TagParameter
    {
        public string Parameter { get; set; }

        public TagParameter(string parameter)
        {
            Parameter = parameter;
        }
    }
}
