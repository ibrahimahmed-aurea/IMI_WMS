using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.MetaManager.DataAccess.Domain
{
    public class PropertyHint
    {
        public PropertyHint()
        {
        }

        public virtual int Id { get; set; }

        public virtual string Text { get; set; }
    }
}
