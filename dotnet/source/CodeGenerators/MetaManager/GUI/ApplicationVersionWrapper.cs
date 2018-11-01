using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.GUI
{
    public class ApplicationVersionWrapper
    {
        public ApplicationVersion ApplicationVersion { get; set; }

        public string Name { get; set; }

        public ApplicationVersionWrapper(ApplicationVersion applicationVersion, string name)
        {
            ApplicationVersion = applicationVersion;
            Name = name;
        }
    }
}
