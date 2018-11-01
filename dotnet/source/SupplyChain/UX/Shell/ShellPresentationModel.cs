using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.SupplyChain.UX.Shell
{
    public class ShellPresentationModel : DependencyObject, IShellPresentationModel
    {
        public IShellModule Module { get; set; }
        private ObservableCollection<ShellDrillDownMenuItem> actions;
                
        public string ContextInfo
        {
            get { return (string)GetValue(ContextInfoProperty); }
            set { SetValue(ContextInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContextInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContextInfoProperty =
            DependencyProperty.Register("ContextInfo", typeof(string), typeof(ShellPresentationModel), new UIPropertyMetadata(""));

        public string InstanceName
        {
            get { return (string)GetValue(InstanceNameProperty); }
            set { SetValue(InstanceNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InstanceName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InstanceNameProperty =
            DependencyProperty.Register("InstanceName", typeof(string), typeof(ShellPresentationModel), new UIPropertyMetadata(""));


        public ShellDrillDownMenuItem FavoritesMenuTopItem
        {
            get { return (ShellDrillDownMenuItem)GetValue(FavoritesMenuTopItemProperty); }
            set { SetValue(FavoritesMenuTopItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FavoritesMenuTopItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FavoritesMenuTopItemProperty =
            DependencyProperty.Register("FavoritesMenuTopItem", typeof(ShellDrillDownMenuItem), typeof(ShellPresentationModel), new UIPropertyMetadata(null));

        public ShellDrillDownMenuItem StartMenuTopItem
        {
            get { return (ShellDrillDownMenuItem)GetValue(StartMenuTopItemProperty); }
            set { SetValue(StartMenuTopItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartMenuTopItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartMenuTopItemProperty =
            DependencyProperty.Register("StartMenuTopItem", typeof(ShellDrillDownMenuItem), typeof(ShellPresentationModel), new UIPropertyMetadata(null));
                        
        public ObservableCollection<ShellDrillDownMenuItem> Actions 
        {
            get
            {
                if (actions == null)
                    actions = new ObservableCollection<ShellDrillDownMenuItem>();

                return actions;
            }
        }

        public bool IsInitialized { get; set; }
    }
}
