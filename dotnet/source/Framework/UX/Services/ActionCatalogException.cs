using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.UX.Services
{
    [Serializable]
    public class ActionCatalogException : Exception
    {
        public ActionCatalogException()
            : base()
        {
        }

        public ActionCatalogException(string message)
            : base(message)
        {

        }

        public ActionCatalogException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
