using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public class UXExpander : UXContainer, ILocalizable
    {
        public UXExpander()
            : base()
        {
            IsExpanded = true;
        }

        public UXExpander(string name)
            : base(name)
        {
            IsExpanded = true;
        }

        [Localizable]
        public string Caption { get; set; }

        public bool IsExpanded { get; set; }

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
