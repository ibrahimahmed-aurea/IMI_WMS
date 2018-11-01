using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public class UXSearchPanel : UXContainer
    {
        public UXSearchPanel()
            : this(null)
        {
        }

        public UXSearchPanel(string name)
            : base(name)
        { 
        }
    }
}
