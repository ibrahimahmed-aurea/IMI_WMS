using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Wms.Mobile.UI.Configuration;

namespace Imi.Wms.Mobile.UI
{
    public class ModifyServerPresenter
    {
        private ModifyServerForm _form;

        public ModifyServerPresenter(ModifyServerForm form)
        {
            _form = form;
        }

        public void Save(ServerElement serverElement)
        {
            UISection config = ConfigurationManager.LoadConfiguration();
                        
            foreach (ServerElement se in config.ServerCollection)
            {
                if (serverElement.Default && se.Name != serverElement.Name)
                {
                    se.Default = false;
                }
            }
            
            var server = (from ServerElement element in config.ServerCollection
                       where element.Name == serverElement.Name
                       select element).LastOrDefault();

            if (server != null)
            {
                config.ServerCollection.Remove(server);
            }

            config.ServerCollection.Add(serverElement);
                                    
            ConfigurationManager.SaveConfiguration(config);
        }
    }
}
