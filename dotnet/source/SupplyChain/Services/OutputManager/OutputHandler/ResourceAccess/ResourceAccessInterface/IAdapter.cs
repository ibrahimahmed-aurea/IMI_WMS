using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.OM.OutputHandler.ResourceAccess
{
    public interface IAdapter
    {
        string AdapterIdentity { get; }
        void InitializeAdapter(Dictionary<string, string> configurationParameters, Dictionary<string, byte[]> reportFilesForAdapter, string uniqueOutputManagerIdentity, out string errorText);
        void UpdateConfiguration(Dictionary<string, string> configurationParameters);
        void UpdateReportFiles(Dictionary<string, byte[]> reportFilesForAdapter, List<string> updatedReports);
        Dictionary<string, object> Execute(OutputDocument document, Dictionary<string, object> namedPassThroughParameters);
    }
}
