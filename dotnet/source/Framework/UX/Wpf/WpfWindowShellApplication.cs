using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Reflection;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.UIElements;

namespace Imi.Framework.UX.Wpf
{

    public abstract class WpfWindowShellApplication<TWorkItem, TShell> : WpfShellApplication<TWorkItem, TShell>
        where TWorkItem : WorkItem, new()
        where TShell : Window
    {
        public WpfWindowShellApplication() : base()
        {
            
        }

        protected override void Start()
        {
            Application.Current.Run(Shell);
        }
        
        protected override void AfterShellCreated()
        {
            base.AfterShellCreated();
        }
    }
}
