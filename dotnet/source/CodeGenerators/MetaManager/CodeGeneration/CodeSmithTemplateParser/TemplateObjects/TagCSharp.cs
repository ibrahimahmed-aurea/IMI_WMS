using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects
{
    public class TagCSharp
    {
        public string Code { get; set; }

        public TagCSharp(string code)
        {
            Code = code;
        }
    }
}
