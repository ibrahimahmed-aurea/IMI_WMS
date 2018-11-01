using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.Framework.DataAccess
{
    public class InformationMessageResult
    {
        public string Sequence { get; set; }

        public int Count { get; set; }

        public string AlarmId { get; set; }

        public string AlarmText { get; set; }
    }
}
