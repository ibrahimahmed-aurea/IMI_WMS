using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects
{
    public class TagImport
    {
        public string Namespace { get; set; }

        public TagImport()
        {
            Namespace = string.Empty;
        }

        public TagImport(string nameSpace)
        {
            Namespace = nameSpace;
        }
    }
}
