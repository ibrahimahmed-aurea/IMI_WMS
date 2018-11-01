using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Imi.SupplyChain.UX
{
    public class UXCommands
    {
        private UXCommands()
        { 
        }

        public static RoutedUICommand AddToFavoritesCommand = new RoutedUICommand("Add Search to Favorites Menu", "AddToFavoritesCommand", typeof(UXCommands));
        public static RoutedUICommand CreateHyperlinkCommand = new RoutedUICommand("Create Hyperlink", "CreateHyperlinkCommand", typeof(UXCommands)); 
    }
}
