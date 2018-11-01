using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public interface IWebView
    {
        string menuId { get; set; }
        string menuDescription { get; set; }
        string oneTimePassword { get; set; }
        string logicalUser { get; set; }
        string loginId { get; set; }
    }

}
