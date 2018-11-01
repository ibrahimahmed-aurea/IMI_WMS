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
using Imi.SupplyChain.OutputManager.Services.Tracing.ServiceContracts;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.OutputManager.Services.Tracing.DataContracts;

namespace Imi.SupplyChain.UX.Modules.OutputManager.Views
{
    public class DiagnosticsPresenter : Presenter<IDiagnosticsView>
    {
        private static bool _isDbTracingEnabled;

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
