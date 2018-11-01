using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects
{
    public class TagText
    {
        public string Text { get; set; }

        public TagText(string text)
        {
            Text = text;
        }
    }
}
