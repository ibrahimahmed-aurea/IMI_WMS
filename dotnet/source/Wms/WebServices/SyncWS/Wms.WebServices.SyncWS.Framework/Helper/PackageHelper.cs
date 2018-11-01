using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.SyncWS.Framework
{
    public delegate ISyncWSResult SearchDelegate(Database database, ISyncWSParameter dataParameter);
    public delegate ISyncWSResult TestDelegate(ISyncWSParameter dataParameter);

    public class PackageHelper
    {
        static public ISyncWSResult GetResult(ISyncWSParameter dataParameter, SearchDelegate searchDelegate, TestDelegate testDelegate)
        {
            string PartnerName = ""; // unknown = default
            /*
            if ((PartnerName == "") || (PartnerName == null))
            {
                try
                {
                    return testDelegate(dataParameter);
                }
                catch
                {
                    return null;
                }
            }*/

            ISyncWSResult res = null;

            using (DBHelper dbHelper = new DBHelper(PartnerName))
            {
                try
                {
                    dbHelper.GetDataBase().StartTransaction();
                    res = searchDelegate(dbHelper.GetDataBase(), dataParameter);
                    dbHelper.GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        dbHelper.GetDataBase().Rollback();
                    }
                    catch (Exception)
                    {
                        // ignore exceptions at rollback in exception handler
                    }

                    string almId = "";

                    // sample:
                    //ORA-20100: ART001 - The Product does not exist.
                    if (e.Message.StartsWith("ORA-20100"))
                    {
                        string[] delimiters = { ": ", " - " };
                        string[] messageParts = e.Message.Split(delimiters, System.StringSplitOptions.None);
                        if (messageParts.GetLength(0) > 1)
                        {
                            almId = messageParts[1];
                        }
                    }

                    Exception exception;
                        
                    if (string.IsNullOrEmpty(almId))
                    {
                        exception = new WarehouseException("XXX001", e.Message);
                    }
                    else
                    {
                        try
                        {
                            exception = new WarehouseException(almId, dbHelper.GetDataBase(), "en");
                        }
                        catch
                        {
                            exception = new Exception("DataError: Error processing data", e);
                        }
                    }
                    throw (exception);
                }
            }
            return res;
        }
        static public string DBExceptionParser(string message)
        {
            // sample:
            //ORA-20100: ART001 - The Product does not exist.
            //ORA-06512: at "OWUSER.WEB_SERVICES_3PL", line 32
            //ORA-06512: at "OWUSER.WEB_SERVICES_3PL", line 86
            //ORA-06512: at "OWUSER.WEB_SERVICES_3PL", line 1074
            //ORA-06512: at line 1
            if (message.StartsWith("ORA-20100"))
            {
                string[] delimiters = { ": ", " - " };
                string[] messageParts = message.Split(delimiters,System.StringSplitOptions.None);
                if (messageParts.GetLength(0) > 1)
                {
                    return messageParts[1];
                }
            }
            else
            {

            }
            return "";
        }
    }
}
