using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public class UXGroupBox : UXContainer, ILocalizable
    {

        public UXGroupBox()
            : base()
        {
        }

        public UXGroupBox(string name)
            : base(name)
        {
        }

        [Localizable]
        public string Caption { get; set; }

        private UXContainer container;

        public UXContainer Container 
        {
            get
            {
                return container;
            }
            set
            {
                if (value != null)
                {
                    value.Parent = this;
                }

                container = value;
            }
        }

        public override UXChildCollection Children
        {
            get
            {
                return null;
            }
        }

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
