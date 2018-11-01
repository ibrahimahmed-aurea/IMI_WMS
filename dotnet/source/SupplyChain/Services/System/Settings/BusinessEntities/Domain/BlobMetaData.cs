using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Settings.BusinessEntities
{
    public class BlobMetaData
    {
        public BlobMetaData()
        {
        }

        public virtual int Id { get; set; }

        public virtual Blob Blob { get; set; }

        public virtual string Name { get; set; }

        public virtual string Value { get; set; }
    }
}
