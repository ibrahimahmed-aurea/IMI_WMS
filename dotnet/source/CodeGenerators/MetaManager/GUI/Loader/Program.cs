using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Imi.Framework.Shared.Configuration;

namespace Cdc.MetaManager.GUI.Loader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string instanceName = string.Empty;

            Imi.Framework.Shared.CommandLineParser parser = new Imi.Framework.Shared.CommandLineParser(args);

            instanceName = parser.GetParameterValue("Instance");

            if (string.IsNullOrEmpty(instanceName))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                SelectInstance instansSelector = new SelectInstance();
                Application.Run(instansSelector);

                if (!string.IsNullOrEmpty(instansSelector.SelectedInstance))
                {
                    instanceName = instansSelector.SelectedInstance;
                }
            }

            if (!string.IsNullOrEmpty(instanceName))
            {

                try
                {
                    Cdc.MetaManager.GUI.MetaManagerInstance instance = InstanceFactory.CreateInstance<Cdc.MetaManager.GUI.MetaManagerInstance>("Cdc.MetaManager.GUI.MetaManagerGUI", "Cdc.MetaManager.GUI.MetaManagerInstance", instanceName);
                    instance.Run();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Failed to start instance \"{0}\":\n\n{1}", instanceName, ex.ToString()), "Error");
                }
            }
        }
    }
}
