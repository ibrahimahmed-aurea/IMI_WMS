using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.SupplyChain.UX.Shell.Services;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Shell.Views
{
	/// <summary>
	/// Interaction logic for PopularSettingsxaml.xaml
	/// </summary>
	[SmartPart]
    public partial class DiagnosticsView : UserControl
	{
        [ServiceDependency]
        public IShellModuleService ShellModuleService { get; set; }

        public DiagnosticsView()
		{
			this.InitializeComponent();
            
            bool loaded = false;

            this.Loaded += (s, e) =>
                {
                    if (!loaded)
                    {
                        loaded = true;

                        var m = from x in ShellModuleService.Modules
                                where x is IModuleDiagnostics
                                select x;

                        this.DataContext = m;
                    }
                };
		}

        public void RunButtonClick(object sender, RoutedEventArgs e)
        {
            ((sender as Button).Tag as IModuleDiagnostics).RunDiagnostics();
        }
                
	}
}