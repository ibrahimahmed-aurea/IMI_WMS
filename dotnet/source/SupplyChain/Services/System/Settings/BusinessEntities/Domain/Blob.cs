using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Settings.BusinessEntities
{
    public class Blob
    {
        public Blob()
        {
        }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime LastModified { get; set; }

        public virtual Container Container { get; set; }

        public virtual IList<BlobMetaData> MetaData { get; set; }

        public virtual string Data { get; set; }
    }
}
