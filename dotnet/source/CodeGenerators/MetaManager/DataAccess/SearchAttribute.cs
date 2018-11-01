using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    public enum SearchTypes { FreeText };

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SearchAttribute : Attribute
    {
        public SearchAttribute()
        {
            SearchType = SearchTypes.FreeText;
        }

        public SearchTypes SearchType;
    }
}
