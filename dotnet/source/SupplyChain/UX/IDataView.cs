using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX
{
    public interface IDataView
    {
        void PresentData(object data);
        void Update(object parameters);
        bool IsVisible { get; set; }
        bool IsEnabled { get; set; }
        bool IsDetailView { get; set; }
        bool RefreshDataOnShow { get; set; }
        string Title { get; set; }
        void SetFocus();
        void UpdateProgress(int progressPercentage);
    }
}
