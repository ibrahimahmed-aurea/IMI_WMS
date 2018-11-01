using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.UX
{
    public class JumpEventArgs: EventArgs
    {
        public JumpEventArgs(object parameters)
        {
            this.Parameters = parameters;
        }

        public object Parameters { get; private set; }
    }
}
