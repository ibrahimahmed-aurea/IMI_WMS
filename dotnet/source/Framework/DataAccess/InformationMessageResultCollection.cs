using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.Framework.DataAccess
{
    public class InformationMessageResultCollection : List<InformationMessageResult>
    {
        public InformationMessageResultCollection() { }

        public InformationMessageResultCollection(IEnumerable<InformationMessageResult> collection)
            : base(collection)
        {
        }
    }
}
