using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizationParser.Models;

namespace AuthorizationParser.Utilities
{
    public static class MenuSequenceSorter
    {
        /// <summary>
        /// Sort all nodes based on sequence.
        /// </summary>
        /// <param name="anApplications"></param>
        public static void Sort(Dictionary<string, ApplicationXMLModel> anApplications)
        {
            foreach (ApplicationXMLModel application in anApplications.Values)
            {
                SortMenuItemChildren(application.MenuItems);
            }
        }

        private static void SortMenuItemChildren(MenuItemModel aMenuItemModel)
        {
            if (aMenuItemModel != null)
            {
                aMenuItemModel.Children.Sort();
                foreach (MenuItemModel child in aMenuItemModel.Children)
                {
                    SortMenuItemChildren(child);
                }
            }
        }
    }
}
