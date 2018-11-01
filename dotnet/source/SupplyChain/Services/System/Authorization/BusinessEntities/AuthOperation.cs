using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Authorization.BusinessEntities
{
    public class AuthOperation
    {
        public string Operation { get; set; }

        public bool IsAuthorized { get; set; }

        public AuthOperation() : this(string.Empty) { }

        public AuthOperation(string operation)
        {
            Operation = operation;
        }
    }
}
