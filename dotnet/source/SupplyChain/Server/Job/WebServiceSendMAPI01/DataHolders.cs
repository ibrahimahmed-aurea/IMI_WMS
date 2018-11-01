using System;

namespace Imi.SupplyChain.Server.Job.WebServiceSendMAPI
{
  /// <summary>
  /// Summary description for DatasHolders.
  /// </summary>
  public class MAPI_OUT
  {
    public string   MAPI_OUT_ID;
    public string   MHID;
    public string   MSG_ID;
    public string   MAPI_OUT_STAT;
    public string   SNDERRCODE;
    public string   SNDERRMSG;
    public Nullable<DateTime> CREATEDTM;
    public Nullable<DateTime> FIRSTSNDDTM;
    public Nullable<DateTime> LASTSNDDTM;
    public int      NOSNDS;
    public int      MAPI_OUT_ID_ORDER_BY;
    public string   URL;
  }
    
}
