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
using System.Windows.Shapes;
using ActiproSoftware.Windows.Controls.Ribbon;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.ComponentModel;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public partial class LoginWindow : RibbonWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
            Topmost = true;
        }
               
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Topmost = false;
        }

        private void WindowClosingEventHandler(object sender, CancelEventArgs e)
        { 
        
        }
    }
}
