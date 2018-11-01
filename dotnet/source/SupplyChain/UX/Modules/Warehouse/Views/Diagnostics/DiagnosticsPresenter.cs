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
using Imi.SupplyChain.Warehouse.Services.Tracing.ServiceContracts;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.Warehouse.Services.Tracing.DataContracts;

namespace Imi.SupplyChain.UX.Modules.Warehouse.Views
{
    public class DiagnosticsPresenter : Presenter<IDiagnosticsView>
    {
        private static bool _isDbTracingEnabled;
        private static bool _isInterfaceTracingEnabled;

        [WcfServiceDependency]
        public ITracingService TracingService { get; set; }

        public override void OnViewSet()
        {
            base.OnViewSet();
        }

        public override void CloseView()
        {
            base.CloseView();
            View.Close();
        }

        public bool IsDatabaseTracingEnabled
        {
            get
            {
                return _isDbTracingEnabled;
            }
        }

        public bool IsInterfaceTracingEnabled
        {
            get
            {
                return _isInterfaceTracingEnabled;
            }
        }

        public void StartDatabaseTracing()
        {
            EnableDatabaseTracingRequest request = new EnableDatabaseTracingRequest();
            request.EnableTracingParams = new EnableDatabaseTracingParameters();
            request.EnableTracingParams.IsTracingEnabled = true;

            TracingService.EnableDatabaseTracing(request);

            _isDbTracingEnabled = true;
        }

        public void StopDatabaseTracing()
        {
            EnableDatabaseTracingRequest request = new EnableDatabaseTracingRequest();
            request.EnableTracingParams = new EnableDatabaseTracingParameters();
            request.EnableTracingParams.IsTracingEnabled = false;

            TracingService.EnableDatabaseTracing(request);

            _isDbTracingEnabled = false;
        }

        public void StartInterfaceTracing(int duration)
        {
            EnableInterfaceTracingRequest request = new EnableInterfaceTracingRequest();
            request.EnableTracingParams = new EnableInterfaceTracingParameters();
            request.EnableTracingParams.IsTracingEnabled = true;
            request.EnableTracingParams.DurationInSeconds = duration;

            TracingService.EnableInterfaceTracing(request);
        }

        public void StopInterfaceTracing()
        {
            EnableInterfaceTracingRequest request = new EnableInterfaceTracingRequest();
            request.EnableTracingParams = new EnableInterfaceTracingParameters();
            request.EnableTracingParams.IsTracingEnabled = false;
            request.EnableTracingParams.DurationInSeconds = 0;

            TracingService.EnableInterfaceTracing(request);
        }

        public void CheckInterfaceTracingStatus(out bool loggEnabled, out string loggStops)
        {
            loggEnabled = false;
            loggStops = string.Empty;
            _isInterfaceTracingEnabled = false;

            GetInterfaceTracingResponse response = TracingService.GetInterfaceTracingStatus();

            if (response.GetInterfaceTracingResult != null)
            {
                if (response.GetInterfaceTracingResult.LoggIsEnabled)
                {
                    loggEnabled = true;
                    loggStops = response.GetInterfaceTracingResult.LoggStopTime.Value.ToString("g");
                    _isInterfaceTracingEnabled = true;
                }
            }
        }

        public void GetServerInformation(out string serverName, out string loggPathDirectory)
        {
            serverName = string.Empty;
            loggPathDirectory = string.Empty;


            GetServerInformationResponse response = TracingService.GetServerInformation();

            if (response.GetServerInformationResult != null)
            {
                serverName = response.GetServerInformationResult.ServerHost;
                loggPathDirectory = response.GetServerInformationResult.DirectoryPath;

            }
        }


    }
}
