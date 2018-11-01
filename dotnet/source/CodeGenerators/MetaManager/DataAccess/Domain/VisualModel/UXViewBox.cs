using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    /* Supported by code generation only, not by visual designer */
    public class UXViewBox : UXComponent
    {

        public UXViewBox()
            : base()
        {
        }

        public UXViewBox(string name)
            : base(name)
        {
        }

        public View View { get; set; }
               
    }
}
