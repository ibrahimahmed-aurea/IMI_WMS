using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.GUI
{
    public class ApplicationWrapper
    {
        public MetaManager.DataAccess.Domain.Application Application { get; set; }

        public string Name { get; set; }

        public ApplicationWrapper(Application application, string name)
        {
            Application = application;
            Name = name;
        }
    }
}
