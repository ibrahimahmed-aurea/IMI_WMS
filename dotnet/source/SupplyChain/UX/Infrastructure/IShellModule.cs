using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Media;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public interface IShellModule
    {
        string Title { get; }
        string Id { get; }
        ImageSource Icon { get; }
        string Version { get; }
        int OrderIndex { get; set; }
        XmlDocument GetMenu();
    }
}
