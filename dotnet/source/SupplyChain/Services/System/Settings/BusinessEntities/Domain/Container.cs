using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Settings.BusinessEntities
{
    public class Container
    {
        public Container()
        {
        }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime LastModified { get; set; }

        public virtual IList<ContainerMetaData> MetaData { get; set; }

        public virtual IList<Blob> Blobs { get; set; }
    }
}
