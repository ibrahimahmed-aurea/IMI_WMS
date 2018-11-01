using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Imi.SupplyChain.UX.Modules.OrderManagement
{
    public static class OrderManagementIcon
    {
        private static ImageSource imageSource = null;

        public static ImageSource ImageSource
        {
            get
            {
                if (imageSource == null)
                {
                    Uri commonGraphicsUri = new Uri("pack://application:,,,/Imi.SupplyChain.UX.Modules.OrderManagement;component/Resources/Graphics.xaml", UriKind.Absolute);
                    ResourceDictionary dictionary = new ResourceDictionary();
                    dictionary.Source = commonGraphicsUri;
                    
                    imageSource = dictionary["OrderManagementLogo"] as ImageSource;
                }

                return imageSource;
            }
        }
    }
}
