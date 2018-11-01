using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [Serializable]
    public class UXLayoutGridCell
    {
        public UXLayoutGridCell()
        {
            ColumnSpan = 1;
        }

        private UXComponent component;

        public UXComponent Component
        {
            get
            {
                return component;
            }
            set
            {
                component = value;
            }

        }

        [Browsable(false)]
        public int Row{ get; set;}

        [Browsable(false)]
        public int Column{ get; set;}

        public int ColumnSpan { get; set; }
    }
}
