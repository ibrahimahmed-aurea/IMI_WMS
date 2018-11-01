using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.SmartParts;
using dvSlave;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public interface IDialogView
    {
        void Show(object smartPart);
        void Show(object smartPart, ISmartPartInfo smartPartInfo);
        object TrimControl { get; }
        void OKToClose();
        bool IsOKToClose();
        bool IsWorkbenchParent { get; set; }
        IList<IDialogView> GetChildPrograms();
        void AddChildProgram(IDialogView childProgram);
        void RemoveChildProgram(IDialogView childProgram);
        IDialogView ParentProgram { get; set; }
        string ProgramName { get; set; }
        string ProgramDescription { get; set; }
        string StartParameters { get; set;  }
        void ShowActions(string actionString);
        int Id { get; set; }
        void SaveSeach(string searchStr);
        void SetFocus();
        void SaveInDashboard(string searchStr);
    }
}
