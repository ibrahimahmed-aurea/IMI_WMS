using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX.Wpf.Activation;
using Imi.Framework.UX.Wpf.BuilderStrategies;

namespace Imi.Framework.UX.Wpf
{
    public abstract class WpfShellApplication<TWorkItem, TShell> : CabShellApplication<TWorkItem, TShell>
        where TWorkItem : WorkItem, new()
    {

        protected override void AddBuilderStrategies(Microsoft.Practices.ObjectBuilder.Builder builder)
        {
            base.AddBuilderStrategies(builder);

            builder.Strategies.AddNew<WindowsServiceStrategy>(BuilderStage.Initialization);
            builder.Strategies.AddNew<FrameworkElementStrategy>(BuilderStage.Initialization);
        }

        protected override void AddServices()
        {
            base.AddServices();

            //RootWorkItem.Services.AddNew<FrameworkElementActivationService, IFrameworkElementActivationService>();
        }

        public WpfShellApplication()
        {
            Application app = new Application();
        }
        
    }
    
    
}
