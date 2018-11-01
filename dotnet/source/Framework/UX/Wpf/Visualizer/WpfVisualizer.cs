using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Windows.Controls;
using Imi.Framework.UX.Wpf.Workspaces;

namespace Imi.Framework.UX.Wpf.Visualizer
{
    public class WpfVisualizer : UserControl
    {
        public WpfVisualizer()
        {
            
        }

        [InjectionConstructor]
        public WpfVisualizer(WorkItem workItem)
            : this()
        {
            LoadViewer(workItem.RootWorkItem);
        }

        private void LoadViewer(WorkItem workItem)
        {
            Viewer viewerUserControl = new Viewer(workItem);
            this.Content = new Viewer(workItem);
        }
    }
}