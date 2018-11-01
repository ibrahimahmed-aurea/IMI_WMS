using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Imi.Framework.Wpf.Controls
{
    public enum SearchAction
    { 
        Search,
        Clear
    }

    public class SearchExecutedEventArgs : RoutedEventArgs
    {
        private SearchAction action;

        public SearchExecutedEventArgs(RoutedEvent routedEvent, SearchAction action)
            : base(routedEvent)
        {
            this.action = action;
        }

        public SearchAction Action
        {
            get
            {
                return action;
            }
        }
    }

    public delegate void SearchExecutedEventHandler(object sender, SearchExecutedEventArgs e);

}
