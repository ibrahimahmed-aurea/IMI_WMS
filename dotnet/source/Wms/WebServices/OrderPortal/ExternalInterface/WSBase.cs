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
using System.Web.Services;
using System.Configuration;
using Imi.Wms.WebServices.OrderPortal.ExternalInterface.Configuration;
using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.OrderPortal.ExternalInterface
{
    public class WSBase : System.Web.Services.WebService
    {
        protected Database db;

        public WSBase()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            //InitializeComponent();
        }

        public Database GetDataBase()
        {
            return db;
        }

        protected void BeginWebmethod(string ChannelId)
        {
            string ConnectionString;

            try
            {
                ConnectionString = GetWebConfig(ChannelId);
            }
            catch (Exception e)
            {
                Exception WebConfigError = new Exception("WebConfigError: File format error", e);
                throw (WebConfigError);
            }

            if (ConnectionString == null)
            {
                Exception WebConfigContentsMissing = new Exception("WebConfigError: No ConnectionString matches ChannelId");
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

            return;
        }

        protected void EndWebmethod()
        {
            try
            {
                db.Close();
            }
            catch
            {
                // ignore
            }
        }

        private string GetWebConfig(string ChannelId)
        {
            WebServiceSection config = ConfigurationManager.GetSection(WebServiceSection.SectionKey) as WebServiceSection;
            string database = null;

            if (config != null)
            {
                foreach (PartnerElement pt in config.PartnerList)
                {
                    // Find default database
                    if ((pt.Name == "*") && (String.IsNullOrEmpty(database)))
                    {
                        database = pt.Database;
                    }

                    // If a specific database is specified for a comm partner
                    if (pt.Name == ChannelId)
                    {
                        database = pt.Database;
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
