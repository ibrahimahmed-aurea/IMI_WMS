using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using Imi.Framework.Services;
using System.ServiceModel.Description;

namespace Imi.SupplyChain.Server.WorkerProcess
{
    public class WorkerProcessHost : MarshalByRefObject
    {
        public static EventWaitHandle StopEvent;

        public static System.Threading.Timer watchDog;
        public static bool processing = false;

        public static object SynkObj = new object();
        public static long LastTimerReset = DateTime.Now.Ticks;
        public static bool ShutDown = false;

        private ServiceHost _serviceHost;

        static WorkerProcessHost()
        {
            StopEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

            watchDog = new System.Threading.Timer(new System.Threading.TimerCallback(WatchDogHandler), null, 10800000, 10800000);
        }

        private static void WatchDogHandler(object state)
        {
            long start = DateTime.Now.Ticks;
            lock (SynkObj)
            {
                if (LastTimerReset < start)
                {
                    if (!processing)
                    {
                        ShutDown = true;
                        StopEvent.Set();
                    }
                }
            }
        }
                
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Run(string workerProcessId, int processingTimeoutInMinutes)
        {
            //ReportEngineSection config = ConfigurationManager.GetSection(ReportEngineSection.SectionKey) as ReportEngineSection;
            //CrystalReportAdapterSection adapterConfig = ConfigurationManager.GetSection(CrystalReportAdapterSection.SectionKey) as CrystalReportAdapterSection;

            EventWaitHandle startedEvent = new EventWaitHandle(false, EventResetMode.ManualReset, workerProcessId);

            NetNamedPipeBinding binding = new NetNamedPipeBinding();
            binding.TransferMode = TransferMode.StreamedRequest;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.Security.Mode = NetNamedPipeSecurityMode.None;
            binding.MaxConnections = 100;
            binding.ReceiveTimeout = new TimeSpan(0, processingTimeoutInMinutes, 0); 
            binding.OpenTimeout = new TimeSpan(0, processingTimeoutInMinutes, 0); 
            binding.SendTimeout = new TimeSpan(0, processingTimeoutInMinutes, 0);
            
            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
            readerQuotas.MaxDepth = 2147483647;
            readerQuotas.MaxStringContentLength = 2147483647;
            readerQuotas.MaxArrayLength = 2147483647;
            readerQuotas.MaxBytesPerRead = 2147483647;
            readerQuotas.MaxNameTableCharCount = 2147483647;
            binding.ReaderQuotas = readerQuotas;
            
            _serviceHost = new ServiceHost(typeof(OptimizeFillRateWorkerProcessService), new Uri[] { new Uri("net.pipe://localhost/" + workerProcessId) });
            _serviceHost.AddServiceEndpoint(typeof(IWorkerProcessService), binding, "");
            
            //Use this code to send exception details back to client
            //=====================================================================================================

            //ServiceDebugBehavior debug = _serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();


            //// if not found - add behavior with setting turned on 
            //if (debug == null)
            //{
            //    _serviceHost.Description.Behaviors.Add(
            //         new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            //}
            //else
            //{
            //    // make sure setting is turned ON
            //    if (!debug.IncludeExceptionDetailInFaults)
            //    {
            //        debug.IncludeExceptionDetailInFaults = true;
            //    }
            //} 

            //======================================================================================================

            _serviceHost.Open();

            startedEvent.Set();
                        
            StopEvent.WaitOne();

            Thread.Sleep(2000);

            _serviceHost.Close();
        }
    }
}
