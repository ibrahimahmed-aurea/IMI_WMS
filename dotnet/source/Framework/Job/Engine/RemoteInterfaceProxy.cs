using System;

namespace Imi.Framework.Job.RemoteInterface
{
    /// <summary>
    /// Summary description for RemoteInstanceCtrl.
    /// </summary>
    public class RemoteInterfaceProxy : MarshalByRefObject, IRemoteInterface
    {
        IRemoteInterface remoteInterface;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public RemoteInterfaceProxy()
        {
            remoteInterface = null;
        }

        public RemoteInterfaceProxy(IRemoteInterface remoteInterface)
        {
            this.remoteInterface = remoteInterface;
        }

        public string StartJob(string name)
        {
            return remoteInterface.StartJob(name);
        }

        public string StopJob(string name)
        {
            return remoteInterface.StopJob(name);
        }

        public string StartAll()
        {
            return remoteInterface.StartAll();
        }

        public string StopAll()
        {
            return remoteInterface.StopAll();
        }

        public string Ps()
        {
            return remoteInterface.Ps();
        }

        public DateTime Time()
        {
            return remoteInterface.Time();
        }

        public string SetParameter(string name, string parameterName, string parameterValue)
        {
            return remoteInterface.SetParameter(name, parameterName, parameterValue);
        }

        public string GetParameter(string name, string parameterName)
        {
            return remoteInterface.GetParameter(name, parameterName);
        }

        public string SetTraceLevel(string name, string level)
        {
            return remoteInterface.SetTraceLevel(name, level);
        }

        public string GetTraceLevel(string name)
        {
            return remoteInterface.GetTraceLevel(name);
        }

    }
}
