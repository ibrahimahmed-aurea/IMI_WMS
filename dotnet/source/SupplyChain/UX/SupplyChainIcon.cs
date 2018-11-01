using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Imi.SupplyChain.UX
{
    public static class SupplyChainIcon
    {
        private static ImageSource imageSource = null;

        public static ImageSource ImageSource
        {
            get
            {
                if (imageSource == null)
                {
                    Uri commonGraphicsUri = new Uri("pack://application:,,,/Imi.SupplyChain.UX;component/Resources/Common/Graphics.xaml", UriKind.Absolute);
                    ResourceDictionary dictionary = new ResourceDictionary();
                    dictionary.Source = commonGraphicsUri;


                    imageSource = dictionary["supplyChainLogo"] as ImageSource;
                }

                return imageSource;
            }
        }
    }
}
