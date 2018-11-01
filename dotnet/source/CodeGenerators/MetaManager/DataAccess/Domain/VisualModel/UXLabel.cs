using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [VisualDesigner("Label", Height=13)]
    public class UXLabel : UXComponent, ILocalizable
    {
        public UXLabel()
            : base()
        {
        }

        public UXLabel(string name)
            : base(name)
        {
        }
        
        [Localizable]
        public string Caption { get; set;}
                
        #region ILocalizable Members
        
        private string metaId;

        public string MetaId
        {
            get 
            {
                if (string.IsNullOrEmpty(metaId))
                    metaId = Guid.NewGuid().ToString();

                return metaId;
            }
            set
            {
                metaId = value;
            }
        }

        #endregion
    }
}
