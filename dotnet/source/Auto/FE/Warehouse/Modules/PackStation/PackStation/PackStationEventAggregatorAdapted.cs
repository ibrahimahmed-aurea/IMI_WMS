// Generated from template: .\UX\Dialog\ViewEventAggregatorTemplate.cst
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Rules;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.Constants;
using Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation.Translators;
using Imi.SupplyChain.UX.Modules.Warehouse.Infrastructure.Services;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation
{
    #region Compiler Settings
	  // Disable assigned property never used
	  #pragma warning disable 414 
	#endregion

    public class PackStationEventAggregator : PackStationEventAggregatorBase
    {
        public override void PackStationSearchPanelViewSearchExecutedEventHandler(object sender, DataEventArgs<PackStationSearchPanelViewResult> eventArgs)
        {
            IPackStationOverviewView packStationOverviewView = WorkItem.SmartParts.FindByType<IPackStationOverviewView>().Last();
            if (eventArgs.Data != null)
            {
                PackStationSearchPanelViewToPackStationOverviewViewTranslator packStationSearchPanelViewToPackStationOverviewViewTranslator = null;

                if (WorkItem.Items.FindByType<PackStationSearchPanelViewToPackStationOverviewViewTranslator>().Count > 0)
                    packStationSearchPanelViewToPackStationOverviewViewTranslator = WorkItem.Items.FindByType<PackStationSearchPanelViewToPackStationOverviewViewTranslator>().Last();
                else
                    packStationSearchPanelViewToPackStationOverviewViewTranslator = WorkItem.Items.AddNew<PackStationSearchPanelViewToPackStationOverviewViewTranslator>();


                PackStationOverviewViewParameters viewParameters = packStationSearchPanelViewToPackStationOverviewViewTranslator.TranslateFromResultToParameters(eventArgs.Data);
                packStationOverviewView.Update(viewParameters);
            }
            else
            {
                PackStationOverviewViewParameters viewParameters = new PackStationOverviewViewParameters();
                packStationOverviewView.Update(viewParameters);
            }
        }

        public override void PackStationOverviewViewUpdatedEventHandler(object sender, DataEventArgs<PackStationOverviewViewResult> eventArgs)
        {
            IPackStationOverviewView packStationOverviewView = WorkItem.SmartParts.FindByType<IPackStationOverviewView>().Last();

            //if (packStationOverviewView.CurrentItem != null)
            //    packStationOverviewView.SetFocus();

            IList<PackStationOverviewViewResult> ViewResults = packStationOverviewView.GetData;


            IDataView packStationFromLCView = WorkItem.Items.Get("Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.IPackStationFromLCView") as IDataView;

            if (packStationFromLCView != null)
            {
                PackStationOverviewViewToPackStationFromLCViewTranslator packStationOverviewViewToPackStationFromLCViewTranslator = null;

                if (WorkItem.Items.FindByType<PackStationOverviewViewToPackStationFromLCViewTranslator>().Count > 0)
                    packStationOverviewViewToPackStationFromLCViewTranslator = WorkItem.Items.FindByType<PackStationOverviewViewToPackStationFromLCViewTranslator>().Last();
                else
                    packStationOverviewViewToPackStationFromLCViewTranslator = WorkItem.Items.AddNew<PackStationOverviewViewToPackStationFromLCViewTranslator>();

                if (packStationFromLCView.IsEnabled)
                {
                    PackStationFromLCViewParameters packStationFromLCViewParameters = packStationOverviewViewToPackStationFromLCViewTranslator.TranslateFromResultToParameters(ViewResults);
                    packStationFromLCView.Update(packStationFromLCViewParameters);
                }
                else
                {
                    packStationFromLCView.Update(null);
                }
            }

            IDataView packStationToLCView = WorkItem.Items.Get("Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.IPackStationToLCView") as IDataView;

            if (packStationToLCView != null)
            {
                PackStationOverviewViewToPackStationToLCViewTranslator packStationOverviewViewToPackStationToLCViewTranslator = null;

                if (WorkItem.Items.FindByType<PackStationOverviewViewToPackStationToLCViewTranslator>().Count > 0)
                    packStationOverviewViewToPackStationToLCViewTranslator = WorkItem.Items.FindByType<PackStationOverviewViewToPackStationToLCViewTranslator>().Last();
                else
                    packStationOverviewViewToPackStationToLCViewTranslator = WorkItem.Items.AddNew<PackStationOverviewViewToPackStationToLCViewTranslator>();

                if (packStationToLCView.IsEnabled)
                {
                    string toLoadCarrierId = string.Empty;
                    if (WorkItem.Items.FindByType<PackStationOverviewPresenter>().Count > 0)
                    {
                        toLoadCarrierId = WorkItem.Items.FindByType<PackStationOverviewPresenter>().Last().ToLoadCarrierId;
                    }
                    PackStationToLCViewParameters packStationToLCViewParameters = packStationOverviewViewToPackStationToLCViewTranslator.TranslateFromResultToParameters(toLoadCarrierId);
                    packStationToLCView.Update(packStationToLCViewParameters);
                }
                else
                {
                    packStationToLCView.Update(null);
                }
            }
        }


    }	
}
