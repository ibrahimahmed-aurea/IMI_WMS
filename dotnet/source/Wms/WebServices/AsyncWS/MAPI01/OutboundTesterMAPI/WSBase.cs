/*
   File         :

   Description  :

   Author       : Olof Laurin, Industri-Matematik International

   Date         :

   Ancestor     :

   Revision         Sign. Date     Note
  ---------------- ----- -------  ---------------------------------------------
   5.0.1            olla  040504   Original version.

*/

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Configuration;
using Imi.Framework.Shared;
using Imi.Framework.Shared.Configuration;
using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.OutboundTesterMAPI
{
    public class WSBase : System.Web.Services.WebService
    {
        private Database db;

        public WSBase()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();
        }

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        public Database GetDataBase()
        {
            return db;
        }

        protected MessageTransaction BeginWebmethod(string MapiId, string TransactionId, string HAPIObjectName)
        {
            string ConnectionString;

            try
            {
                ConnectionString = GetWebConfig(MapiId);
            }
            catch (Exception e)
            {
                Exception WebConfigError = new Exception("WebConfigError: File format error", e);
                throw (WebConfigError);
            }

            if (ConnectionString == null)
            {
                Exception WebConfigContentsMissing = new Exception("WebConfigError: No ConnectionString matches ExternalId");
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

            db.StartTransaction();

            MessageTransaction mt = new MessageTransaction(MapiId, TransactionId, HAPIObjectName, db);

            return mt;
        }

        protected void EndWebmethod()
        {
            try
            {
                db.Rollback();
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
