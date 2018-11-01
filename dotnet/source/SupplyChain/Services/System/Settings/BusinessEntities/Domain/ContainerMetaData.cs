using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Settings.BusinessEntities
{
    public class ContainerMetaData
    {
        public ContainerMetaData()
        {
        }

        public virtual int Id { get; set; }

        public virtual Container Container { get; set; }

        public virtual string Name { get; set; }

        public virtual string Value { get; set; }
    }
}
