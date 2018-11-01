using System;
using System.Data;

namespace Imi.Wms.Mobile.Server.Interface
{
    /// <summary>
    /// Summary description for IInstanceCtrl.
    /// </summary>
    public interface IRemoteInterface
    {
        string KillSession(string sessionId);
        string KillAllSessions();
        DateTime Time();
        DataTable GetSessionList();
        StateResponse GetSessionState(string sessionId);
        string SetParameter(string sessionId, string name, string value);
        string GetParameter(string sessionId, string name);
        string SetTraceLevel(string sessionId, string level);
        string GetTraceLevel(string sessionId);
    }
}
