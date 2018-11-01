using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Wms.Mobile.UI.Configuration;

namespace Imi.Wms.Mobile.UI
{
    public class OptionsPresenter
    {
        private OptionsForm _form;

        public OptionsPresenter(OptionsForm form)
        {
            _form = form;
        }

        public void Save(UISection config)
        {
            Logger.IsEnabled = config.LogEnabled;
            ConfigurationManager.SaveConfiguration(config);
        }
    }
}
