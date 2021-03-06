using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Utility;
using Imi.Framework.UX;

namespace Imi.SupplyChain.UX.Views
{
    public class SearchProgressPresenter : Presenter<ISearchProgressView>
    {
        public void Cancel(CancelSearchDelegate cancelCallback)
        {
            if (cancelCallback != null)
            {
                cancelCallback();
            }
        }
    }
}
