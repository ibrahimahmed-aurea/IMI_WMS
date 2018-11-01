using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imi.Framework.Wpf.Controls;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class ApplicationBarCommands
    {
        public static RoutedCommand Cut = new RoutedCommand("Cut", typeof(ApplicationBarCommands));
        public static RoutedCommand Copy = new RoutedCommand("Copy", typeof(ApplicationBarCommands));
        public static RoutedCommand Paste = new RoutedCommand("Paste", typeof(ApplicationBarCommands));
        public static RoutedCommand Delete = new RoutedCommand("Delete", typeof(ApplicationBarCommands));
        public static RoutedCommand Rename = new RoutedCommand("Rename", typeof(ApplicationBarCommands));
        public static RoutedCommand AddFolder = new RoutedCommand("AddFolder", typeof(ApplicationBarCommands));
        public static RoutedCommand AddToFavorites = new RoutedCommand("AddToFavorites", typeof(ApplicationBarCommands));
        public static RoutedCommand OpenInNewWindow = new RoutedCommand("OpenInNewWindow", typeof(ApplicationBarCommands));
        public static RoutedCommand AddToDashboard = new RoutedCommand("AddToDashboard", typeof(ApplicationBarCommands));
    }
}
