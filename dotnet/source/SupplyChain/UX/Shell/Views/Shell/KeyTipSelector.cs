using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Wpf.Controls;
using System.Collections;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class KeyTipSelector : IKeyTipSelector
    {
        public string SelectorPrefix { get; set; }
        private const string alphaBeta = "123456789ABCDEGHIJKLMNOPQRSTUVWXYZ";
        private const int alphaBase = 34;

        private Dictionary<int, List<string>> alphaDictionary = new Dictionary<int, List<string>>();

        public void SetKeyTips(IEnumerable menuItems)
        {
            IEnumerable<IKeyTipElement> elementList = menuItems.Cast<IKeyTipElement>();

            int numberOfElements = elementList.Count();

            string keyTip = GetFirstKeyTip(numberOfElements);

            foreach (IKeyTipElement element in elementList)
            {
                element.KeyTipAccessText = string.Format("{0}{1}", SelectorPrefix, keyTip);
                keyTip = GetNextKeyTip(keyTip);
            }

        }

        private static string GetFirstKeyTip(int numberOfElements)
        {
            int numberOfCharacters = 1;
            int divider = alphaBase;

            while (divider < numberOfElements)
            {
                numberOfCharacters++;
                divider *= alphaBase;
            }

            return string.Empty.PadLeft(numberOfCharacters, alphaBeta[0]);
        }

        private string GetNextKeyTip(string keyTip)
        {
            int currPos = keyTip.Length - 1;

            string result = string.Empty;

            while (currPos >= 0)
            {
                int currCharVal = alphaBeta.IndexOf(keyTip[currPos]);

                if (currCharVal == alphaBeta.Length - 1)
                {
                    result += alphaBeta[0];
                }
                else
                {
                    result = alphaBeta[currCharVal + 1] + result;
                    break;
                }

                currPos--;
            }

            if(result.Length < keyTip.Length)
            {
                result = keyTip.Substring(0, keyTip.Length - result.Length) + result;
            }

            return result;
        }

    }
}
