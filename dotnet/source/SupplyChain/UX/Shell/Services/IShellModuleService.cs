using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Shell.Services
{
    public interface IShellModuleService
    {
        event EventHandler<DataEventArgs<IShellModule>> ModuleActivated;
        event EventHandler<DataEventArgs<IShellModule>> ModuleLoaded;
        IShellPresentationModel GetPresentationModel(IShellModule module);
        ReadOnlyCollection<IShellPresentationModel> PresentationModels { get; }
        ReadOnlyCollection<IShellModule> Modules { get; }
        void RegisterModule(IShellModule module, WorkItem workItem);
        IShellModule ActiveModule { get; set; }
        WorkItem GetWorkItem(IShellModule module);
    }
}
