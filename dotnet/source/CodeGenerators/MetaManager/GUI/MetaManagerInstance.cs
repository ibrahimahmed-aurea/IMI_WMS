using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Imi.Framework.Shared.Configuration;

namespace Cdc.MetaManager.GUI
{
    public class MetaManagerInstance : ApplicationInstance
    {

        public MetaManagerInstance(string instanceName)
            : base(instanceName)
        {
        }

        public void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Cdc.MetaManager.GUI.Main());
        }
    }
}
