using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using LCAInterop;
using Imi.SupplyChain.Server.Job.OptimizeFillRate;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Imi.SupplyChain.Server.WorkerProcess
{
    public class OptimizeFillRateWorkerProcessService : IWorkerProcessService
    {
        

        public OptimizeFillRateWorkerProcessService()
        {
            lock (WorkerProcessHost.SynkObj)
            {
                WorkerProcessHost.processing = true;
            }
        }

        

        #region IWorkerProcessService Members

        public string Process(List<string> parameters)
        {
            try
            {
                IList<PBROW> rows = null;
                IList<AISLE_WAYPOINT> waypoints = null;
                LCASettings lcaSettings = null;

                XmlSerializer pbrowsSeri = new XmlSerializer(typeof(List<PBROW>));
                XmlSerializer waypointsSeri = new XmlSerializer(typeof(List<AISLE_WAYPOINT>));
                XmlSerializer lcaSettingsSeri = new XmlSerializer(typeof(LCASettings));

                if (parameters.Count != 3) { return null; }

                try
                {
                    rows = pbrowsSeri.Deserialize(new StringReader(parameters[0])) as IList<PBROW>;
                    waypoints = waypointsSeri.Deserialize(new StringReader(parameters[1])) as IList<AISLE_WAYPOINT>;
                    lcaSettings = lcaSettingsSeri.Deserialize(new StringReader(parameters[2])) as LCASettings;
                }
                catch
                {
                    return null;
                }

                if (rows == null || waypoints == null || lcaSettings == null)
                {
                    return null;
                }


                LCAWrapperResult result = null;

                using (LCAWrapper lca = new LCAWrapper(lcaSettings))
                {
                    if (!string.IsNullOrEmpty(lcaSettings.StrekArea))
                    {
                        lca.AddArea(lcaSettings.StrekArea, lcaSettings.StrekXCoord, lcaSettings.StrekYCoord);
                    }

                    string prevWSID = "";
                    string prevAISLE = "";

                    foreach (AISLE_WAYPOINT waypoint in waypoints)
                    {
                        if (waypoint.WSID != prevWSID)
                        {
                            lca.AddArea(waypoint.WSID, waypoint.WS_XCORD, waypoint.WS_YCORD);
                        }

                        prevWSID = waypoint.WSID;

                        if (waypoint.AISLE != prevAISLE)
                        {
                            lca.AddAisle(waypoint.WSID, waypoint.AISLE, waypoint.AISLE_FROM_XCORD, waypoint.AISLE_FROM_YCORD, waypoint.AISLE_TO_XCORD, waypoint.AISLE_TO_YCORD, waypoint.DIRECTION_PICK);
                        }

                        prevAISLE = waypoint.AISLE;

                        lca.AddAisleWayPoint(waypoint.WSID, waypoint.AISLE, waypoint.WAYPOINT_ID, waypoint.WAYPOINT_XCORD, waypoint.WAYPOINT_YCORD);
                    }

                    foreach (PBROW row in rows)
                    {
                        lca.AddPickBatchLine(row.PBROWID, row.PICKSEQ, row.ARTID, row.COMPANY_ID, row.ORDQTY, row.VOLUME, row.WEIGHT, row.WSID, row.AISLE, row.ARTGROUP, row.CATGROUP, row.XCORD, row.YCORD, row.WPADR);
                    }

                    result = lca.Process();
                }

                

                if (result != null)
                {
                    XmlSerializer resultSeri = new XmlSerializer(typeof(LCAWrapperResultWrapper));

                    XmlWriterSettings xmlsettings = new XmlWriterSettings();
                    xmlsettings.Indent = true;
                    xmlsettings.NewLineOnAttributes = true;

                    StringBuilder builder = new StringBuilder();
                    XmlWriter writer = XmlWriter.Create(builder, xmlsettings);

                    LCAWrapperResultWrapper wrapper = new LCAWrapperResultWrapper(result);

                    resultSeri.Serialize(writer, wrapper);

                    return builder.ToString();
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                WorkerProcessHost.processing = false;
            }
        }

        public void Terminate()
        {
            WorkerProcessHost.StopEvent.Set();
        }

        public bool IsAlive()
        {
            WorkerProcessHost.watchDog.Change(10800000, 10800000);

            lock (WorkerProcessHost.SynkObj)
            {
                try
                {
                    WorkerProcessHost.LastTimerReset = DateTime.Now.Ticks;

                    if (WorkerProcessHost.ShutDown)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                finally
                {
                    WorkerProcessHost.processing = false;
                }
            }
        }

        #endregion
    }
}
