using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthorizationParser.Models
{
    /// <summary>
    /// Datastructure representing a Menu Item. Examples: Inbound, Inbound Overview, Outbound
    /// </summary>
    public class MenuItemModel : IComparable
    {
        public List<MenuItemModel> Children { get; set; }
        public List<UXAction> Actions { get; set; }
        public string Caption { get; set; }
        public string Identity { get; set; }
        public int Sequence { get; set; }

        public MenuItemModel()
        {
            Children = new List<MenuItemModel>();
        }

        public int CompareTo(object obj)
        {
            MenuItemModel menuItemModel = (MenuItemModel)obj;
            if (menuItemModel == null)
            {
                return 0;
            }
            if (this.Sequence == menuItemModel.Sequence)
            {
                return 0;
            }
            else if (this.Sequence < menuItemModel.Sequence)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

    }
}
