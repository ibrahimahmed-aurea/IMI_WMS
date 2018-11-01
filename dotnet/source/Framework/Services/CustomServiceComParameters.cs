using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.Framework.Services
{
    public enum SortDirections
    {
        None,
        Ascending,
        Descending
    }

    public struct SortParameter
    {
        public string PropertyName { get; set; }
        public SortDirections SortDirection { get; set; }
    }


    [DataContract(Name = "ComParameters", Namespace = "http://Imi.Framework.Services")]
    public class CustomServiceComParameters
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public bool Abort { get; set; }

        [DataMember(Order = 3)]
        public bool SequentialRequest { get; set; }

        [DataMember(Order = 4)]
        public List<SortParameter> SortParameters { get; set; }

        [DataMember(Order = 5)]
        public bool WaitForCount { get; set; }
    }
}
