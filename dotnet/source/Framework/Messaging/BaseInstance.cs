using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Imi.Framework.Integration.Engine;

namespace Imi.Framework.Integration
{
    public abstract class BaseInstance : MarshalByRefObject
    {

        private AppDomain appDomain;

        protected BaseInstance()
        {
            appDomain = AppDomain.CurrentDomain;    
        }

        internal AppDomain AppDomain
        {
            get
            {
                return appDomain;
            }
        }
                
        public abstract void Initialize();

        public void Start()
        {
            MessageEngine.Instance.Start();
        }

        public override object InitializeLifetimeService()
        {
            //Infinite lease time, since this object will only be marshalled across AppDomains
            return null;
        }

        public TraceLevel TraceLevel
        {
            set
            {
                MessageEngine.Instance.Tracing.Level = value;
            }
        }
        
        internal void Stop()
        {
            MessageEngine.Instance.Stop(true);
            MessageEngine.Instance.Dispose();
        }
    }
}
