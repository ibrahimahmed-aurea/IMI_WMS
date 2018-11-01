using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.BusinessLogic
{
    public class ModelException : Exception
    {
        public ModelException(string message) : base(message) { ;}
        public ModelException(string message, Exception inner) : base(message, inner) { ;}

    }

    public class ModelAggregatedException : ModelException
    {

        public List<string> Ids { get; set; }

        public ModelAggregatedException(List<string> ids, string message) : base(message) { Ids = ids; }
        public ModelAggregatedException(List<string> ids, string message, Exception inner) : base(message, inner) { Ids = ids; }

    }

}
