using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.GUI
{
    public class AddPackageSpecificationSchemaWrapper
    {
        public Schema Schema { get; set; }

        public string Name { get; set; }

        public AddPackageSpecificationSchemaWrapper(Schema schema, string name)
        {
            Schema = schema;
            Name = name;
        }

    }
}
