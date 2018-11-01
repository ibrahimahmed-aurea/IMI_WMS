using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OutputManager.Views.ChooseDefaultOutputManager
{
    public interface IChooseDefaultOutputManagerView : IDataView, IBuilderAware
    {
        bool? ShowDialog();
        void Close(bool result);

        string SelectedOutputManagerId { get; set; }

        void SaveSettings(ChooseDefaultOutputManagerSettingsRepository settings);
        void LoadSettings(ChooseDefaultOutputManagerSettingsRepository settings);
        
    }

}
