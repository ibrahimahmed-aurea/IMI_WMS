/*
   File         :

   Description  :

   Author       : Olof Laurin, Industri-Matematik International

   Date         :

   Ancestor     :

   Revision         Sign. Date     Note
  ---------------- ----- -------  ---------------------------------------------
   5.0.1            olla  040504   Original version.
   5.1.3            olla  060328   .NET 2.0 conversion

*/

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Configuration;

using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.SyncWS.Framework
{
    public class DBHelper : IDisposable
    {

        private Database db;

        public Database GetDataBase()
        {
            return db;
        }

        public DBHelper(string partnerName)
        {
            string ConnectionString;
            bool dbtrace;

            try
            {
                ConnectionString = GetWebConfig(partnerName, out dbtrace);
            }
            catch (Exception e)
            {
                Exception WebConfigError = new Exception("WebConfigError: File format error", e);
                throw (WebConfigError);
            }

            if (ConnectionString == null)
            {
                Exception WebConfigContentsMissing = new Exception("WebConfigError: No ConnectionString matches Partner Name");
                throw (WebConfigContentsMissing);
            }

            try
            {
                db = new Database(ConnectionString);
            }
            catch (Exception e)
            {
                Exception WebConfigContentsError = new Exception("WebConfigError: Database connection failed", e);
                throw (WebConfigContentsError);
            }

            if (dbtrace)
            {
                try
                {
                    Wlsystem pkg = new Wlsystem(db);
                    pkg.StartLog("syncws", "IIS", "0");
                }
                catch (Exception e)
                {
                    Exception WebConfigContentsError = new Exception("WebConfigError: Failed to start logging", e);
                    throw (WebConfigContentsError);
                }
            }

            return;
        }

        public void Dispose()
        {
            try
            {
                db.Close();
                db.Dispose();
            }
            catch
            {
                // ignore
            }
        }

        private string GetWebConfig(string partnerName, out bool dbtrace)
        {
            WebServiceSection config = ConfigurationManager.GetSection(WebServiceSection.SectionKey) as WebServiceSection;
            string database = null;
            dbtrace = false;

            if (config != null)
            {
                foreach (PartnerElement pt in config.PartnerList)
                {
                    // Find default database
                    if ((pt.Name == "*") && (String.IsNullOrEmpty(database)))
                    {
                        database = pt.Database;
                        dbtrace = (pt.Dbtrace.ToLower() == "true");
                    }

                    // If a specific database is specified for a comm partner
                    if (pt.Name == partnerName)
                    {
                        database = pt.Database;
                        dbtrace = (pt.Dbtrace.ToLower() == "true");
                    }
                }

                if (!String.IsNullOrEmpty(database))
                {
                    foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
                    {
                        if (database == cs.Name)
                        {
                            return (cs.ConnectionString);
                        }
                    }
                }
            }

            return null;
        }
    }
}
