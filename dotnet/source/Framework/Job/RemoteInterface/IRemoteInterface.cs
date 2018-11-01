using System;

namespace Imi.Framework.Job.RemoteInterface
{
    /// <summary>
    /// Summary description for IInstanceCtrl.
    /// </summary>
    public interface IRemoteInterface
    {
        string StartJob(string name);
        string StopJob(string name);
        string StartAll();
        string StopAll();
        DateTime Time();  // just to determine if server is alive ?
        string Ps();
        string SetParameter(string name, string id, string nuValue);
        string GetParameter(string name, string id);
        string SetTraceLevel(string name, string level);
        string GetTraceLevel(string name);
    }
}
