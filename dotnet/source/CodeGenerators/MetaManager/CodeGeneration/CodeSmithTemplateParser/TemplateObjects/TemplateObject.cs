using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects
{
    public class TemplateObject
    {
        public Type TagType { get; set; }
        public object Object { get; set; }

        public TemplateObject(Type tagType, object tagObject)
        {
            TagType = tagType;
            Object = tagObject;
        }
    }
}
