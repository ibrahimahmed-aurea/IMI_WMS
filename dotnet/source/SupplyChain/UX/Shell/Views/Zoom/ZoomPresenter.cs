using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX;
using System.Diagnostics;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI;
using System.Windows;
using System.Reflection;
using System.IO;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class ZoomPresenter : Presenter<IZoomView>
    {
        public override void OnViewSet()
        {
            base.OnViewSet();
        }

        public override void CloseView()
        {
            base.CloseView();
            View.Close();
        }

    }
}
