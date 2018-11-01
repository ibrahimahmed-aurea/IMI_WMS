using System;
using System.Collections.Generic;

namespace Imi.SupplyChain.Server.Job.OptimizeFillRate
{
    public class PBROW
    {
        public string PBROWID;
        public string ARTID;
        public string COMPANY_ID;
        public string PBROWGRP_ID;
        public int PICKSEQ;
        public double VOLUME;
        public double WEIGHT;
        public string WSID;
        public string AISLE;
        public string ARTGROUP;
        public double XCORD;
        public double YCORD;
        public string WPADR;
        public double ORDQTY;
        public string CATGROUP;
    }

    public class AISLE_WAYPOINT
    {
        public string WSID;
        public string AISLE;
        public string WAYPOINT_ID;
        public double WAYPOINT_XCORD;
        public double WAYPOINT_YCORD;
        public double WS_XCORD;
        public double WS_YCORD;
        public double AISLE_FROM_XCORD;
        public double AISLE_TO_XCORD;
        public double AISLE_FROM_YCORD;
        public double AISLE_TO_YCORD;
        public string DIRECTION_PICK;
    }

    public class LCAWrapperResultWrapper
    {
        public LCAWrapperResultWrapper()
        {
        }

        public LCAWrapperResultWrapper(LCAInterop.LCAWrapperResult lcaWrapperResult)
        {
            Lines = lcaWrapperResult.Lines;

            Errors = new List<LCAWrapperResultErrorWrapper>();

            foreach (KeyValuePair<int, List<string>> kv in lcaWrapperResult.Errors)
            {
                foreach (string errorDesc in kv.Value)
                {
                    Errors.Add(new LCAWrapperResultErrorWrapper() { ErrorNumber = kv.Key, ErrorDescription = errorDesc });
                }
            }
        }

        public List<LCAWrapperResultErrorWrapper> Errors { get; set; }
        public List<LCAInterop.LCAWrapperResultLine> Lines { get; set; }

        public Dictionary<int, List<string>> GetErrorDictionary()
        {
            Dictionary<int, List<string>> resultDict = new Dictionary<int, List<string>>();

            foreach (LCAWrapperResultErrorWrapper error in Errors)
            {
                if (!resultDict.ContainsKey(error.ErrorNumber))
                {
                    resultDict.Add(error.ErrorNumber, new List<string>());
                }

                resultDict[error.ErrorNumber].Add(error.ErrorDescription);
            }

            return resultDict;
        }
    }

    public class LCAWrapperResultErrorWrapper
    {
        public int ErrorNumber;
        public string ErrorDescription;
    }
}
