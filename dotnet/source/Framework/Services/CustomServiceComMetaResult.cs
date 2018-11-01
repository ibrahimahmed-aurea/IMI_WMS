using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace Imi.Framework.Services
{
    [DataContract(Name = "ComMetaResult", Namespace = "http://Imi.Framework.Services")]
    public class CustomServiceComMetaResult
    {
        [DataMember(Order = 1)]
        public long TotalCount { get; set; }

        [DataMember(Order = 2)]
        public bool IsLastPartition { get; set; }

        [DataMember(Order = 3)]
        public bool InvalidSession { get; set; }
    }
}
