using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Imi.Framework.Wpf.Controls
{
    public class KeyTipActivator
    {
        public IKeyTipElement Parent;
        public IKeyTipElement KeyTipElement;
    }

    public class KeyTipActivationService
    {
        private static Dictionary<string, KeyTipActivator> keyTips = new Dictionary<string, KeyTipActivator>();

        public static void RegisterKeyTips(IKeyTipElement menu, IEnumerable<IKeyTipElement> elements)
        {
            var dropOuts = from KeyTipActivator activator in keyTips.Values
                           where activator.Parent == menu
                           select activator.KeyTipElement.KeyTipAccessText;

            
            if (dropOuts.Count() > 0)
            {
                IEnumerable<string> removers = dropOuts.ToList<string>();

                foreach (string accessText in removers)
                {
                    keyTips.Remove(accessText);
                }
            }

            foreach (IKeyTipElement element in elements)
            {
                keyTips[element.KeyTipAccessText] = new KeyTipActivator
                                                        {
                                                            KeyTipElement = element,
                                                            Parent = menu
                                                        };
            }
        }

        public static void Activate(string accessText)
        {
            if (keyTips.Keys.Contains(accessText))
            {
                KeyTipActivator activator = keyTips[accessText];
                activator.Parent.Activate(activator.KeyTipElement);
            }
        }

        public static bool IsPartialOrFullTip(string accessText)
        {
            if (keyTips.Keys.Contains(accessText))
            {
                return true;
            }

            return (keyTips.Keys.Where(s => s.StartsWith(accessText)).Count() > 0);
        }

        public static bool IsFullTip(string accessText)
        {
            if (keyTips.Keys.Contains(accessText))
            {
                return true;
            }

            return false;
        }

    }
}
