using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;
using Imi.Framework.Job;
using Imi.Framework.Job.Configuration;
using Imi.Framework.Services;
using System.Security.Policy;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using System.ServiceModel.Description;

namespace Imi.SupplyChain.Server.Job.ServiceHost
{

    public class AutoServiceHostJob : ManagedJob, ISpawn
    {
        public AutoServiceHostJob(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
        }

        public override void Init()
        {
        }

        public override void Execute()
        {
        }

        public override void Stop()
        {
        }
                
        public ManagedJob[] SpawnJobs()
        {
            var jobs = new List<ServiceHostJob>();

            string filter = args["Filter"];

            string useCompression = string.Empty;
            if (args.ContainsKey("UseCompression"))
            {
                useCompression = args["UseCompression"];
            }

            foreach (KeyValuePair<string, Type> service in FindServiceContracts(filter))
            { 
                JobArgumentCollection jobArgs = new JobArgumentCollection();
                jobArgs.Add("type", service.Value.AssemblyQualifiedName);
                if (!string.IsNullOrEmpty(useCompression))
                {
                    jobArgs.Add("UseCompression", useCompression);
                }
                ServiceHostJob job = new ServiceHostJob(service.Key, false, jobArgs);
                jobs.Add(job);
            }

            return jobs.ToArray();
        }
                
        private static Dictionary<string, Type> FindServiceContracts(string filter)
        {
            Uri uri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));

            var fileList = new List<string>();

            foreach (string filterEntry in filter.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries))
            {
                fileList.AddRange(Directory.GetFiles(uri.LocalPath, filterEntry.Trim(), SearchOption.TopDirectoryOnly));
            }

            Dictionary<string, Type> n = new Dictionary<string, Type>();

            foreach (string fileName in fileList)
            {
                FileInfo f = new FileInfo(fileName);

                Assembly assemb = Assembly.LoadFile(f.FullName);

                Type[] assemblyTypes = assemb.GetTypes();

                foreach (Type serviceType in assemblyTypes)
                {
                    if (serviceType.BaseType == typeof(SecurityTokenServiceConfiguration))
                    {
                        n.Add(serviceType.Assembly.GetName().Name, serviceType);
                    }
                    else if (serviceType.Name.EndsWith("Service"))
                    {
                        Type[] typeInterfaces = serviceType.GetInterfaces();
                        string contractName = "";

                        foreach (Type typeInterface in typeInterfaces)
                        {
                            if (typeInterface.Name.EndsWith("Service"))
                            {
                                contractName = typeInterface.FullName;
                                n.Add(contractName, serviceType);
                                break;
                            }
                        }
                    }
                }
            }

            return n;
        }
        
    }

    public class ServiceHostJob : ManagedJob
    {
        private System.ServiceModel.ServiceHost serviceHost;
        private object syncLock;
        private EventWaitHandle waitHandle;
        private string typeName;
        private Type serviceType;

        public ServiceHostJob(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
            typeName = args["type"];
            
            if (string.IsNullOrEmpty(typeName))
            {
                throw new NullReferenceException(string.Format("Error in service {0}. The type parameter is null, must be defined.", name));
            }

            serviceType = Type.GetType(typeName, true);

            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            syncLock = new object();
                        
            CustomServiceHost.ContractNamespace = "ServiceContracts";
            CustomServiceHost.ImplementationNamespace = "ServiceImplementation";
        }

        public override void Init()
        {
            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Enter Init");
        }

        public override void Execute()
        {
            lock (syncLock)
            {
                waitHandle.Reset();

                if (serviceHost == null)
                {
                    Tracing.TraceEvent(TraceEventType.Information, 0, "Hosting service: \"{0}\"...", serviceType.Name);
                                        
                    if (serviceType.BaseType == typeof(SecurityTokenServiceConfiguration))
                    {
                        serviceHost = new WSTrustServiceHost((SecurityTokenServiceConfiguration)Activator.CreateInstance(serviceType));
                        serviceHost.Open();
                    }
                    else
                    {
                        bool useCompression = true;
                        if (args.ContainsKey("UseCompression"))
                        {
                            useCompression = Convert.ToBoolean(args["UseCompression"]);
                        }

                        bool createInstanceAndPassArguments = false;
                        foreach (object attribute in serviceType.GetCustomAttributes(false))
                        {
                            if (attribute.GetType() == typeof(System.ServiceModel.ServiceBehaviorAttribute))
                            {
                                if (((System.ServiceModel.ServiceBehaviorAttribute)attribute).InstanceContextMode == InstanceContextMode.Single)
                                {
                                    if (serviceType.GetMethod("PassArgumentsFromServiceHost") != null)
                                    {
                                        createInstanceAndPassArguments = true;
                                    }
                                }
                            }
                        }

                        if (createInstanceAndPassArguments)
                        {
                            object serviceInstance = Activator.CreateInstance(serviceType);
                            serviceHost = new CustomServiceHost(serviceInstance, useCompression);
                            serviceHost.Open();

                            serviceType.GetMethod("PassArgumentsFromServiceHost").Invoke(serviceInstance, new object[] {serviceHost});
                        }
                        else
                        {
                            serviceHost = new CustomServiceHost(serviceType, useCompression);
                            serviceHost.Open();
                        }
                    }

                    foreach (ServiceEndpoint ep in serviceHost.Description.Endpoints)
                    {
                        Tracing.TraceEvent(TraceEventType.Information, 0, "  Service endpoint: \"{0}\"", ep.ListenUri.ToString());
                    }

                }
            }

            waitHandle.WaitOne();
        }

        public override void Stop()
        {
            lock (syncLock)
            {
                if (serviceHost.State != CommunicationState.Closed)
                {
                    serviceHost.Close();
                    serviceHost = null;
                }
                waitHandle.Set();
            }
        }
    }
}
