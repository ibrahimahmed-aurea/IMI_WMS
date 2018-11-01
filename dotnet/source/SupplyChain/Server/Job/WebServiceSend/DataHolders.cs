using System;

namespace Imi.SupplyChain.Server.Job.WebServiceSend
{
  /// <summary>
  /// Summary description for DatasHolders.
  /// </summary>
  public class HAPITRANS
  {
    public string   HAPITRANS_ID;
    public string   COMPANY_ID;
    public string   HAPIOBJECTNAME;
    public string   HAPISTAT;
    public string   HAPIERRCOD;
    public string   HAPIERRMSG;
    public Nullable<DateTime> CREATEDTM;
    public Nullable<DateTime> FIRSTSNDDTM;
    public Nullable<DateTime> LASTSNDDTM;
    public int      NOSNDS;
    public int      HAPITRANS_ID_ORDER_BY;
    public string   WEBS_PROFILE_ID;
    public string   CHANNEL_ID;
  }
    
  public class WEBS_PROFILE
  {
    public string WEBS_PROFILE_ID;
    public string WEBS_PROFILE_NAME;
    public string HAPIOBJECTNAME;
    public string DESTINATION_URL;
  }
}
