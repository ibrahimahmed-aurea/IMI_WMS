using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public static class DashboardIcon
    {
        private static ImageSource imageSource = null;

        public static ImageSource ImageSource
        {
            get
            {
                if (imageSource == null)
                {
                    Uri commonGraphicsUri = new Uri("pack://application:,,,/Imi.SupplyChain.UX.Shell;component/Resources/Graphics.xaml", UriKind.Absolute);
                    ResourceDictionary dictionary = new ResourceDictionary();
                    dictionary.Source = commonGraphicsUri;
                    
                    imageSource = dictionary["DashboardIcon"] as ImageSource;
                }

                return imageSource;
            }
        }
    }
}
