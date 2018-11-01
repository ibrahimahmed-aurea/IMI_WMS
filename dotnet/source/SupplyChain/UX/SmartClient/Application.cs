using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Markup;
using System.Deployment.Application;
using System.Collections.Specialized;
using System.Web;
using System.ServiceModel;
using System.Reflection;
using System.IO;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.UX.Services;
using System.Security.Policy;
using System.Security.Permissions;
using System.Security;
using System.Runtime.InteropServices;
using Imi.SupplyChain.UX.Shell;

namespace Imi.SupplyChain.UX.SmartClient
{
    public class Application
    {
        [System.STAThread]
        private static void Main()
        {
            LoginApplication app = new LoginApplication();
            app.Run();
        }
    }
}
