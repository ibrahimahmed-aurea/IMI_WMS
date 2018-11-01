using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX
{
    public class RowTracker
    {
        public string RowIdentity { get; set; }
    }
    
    public class Bookmark : RowTracker
    {
        public int RowNumber { get; set; }

        public List<Bookmark> MultipleSelection { get; set; }
    }
}
