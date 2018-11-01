using System;
using System.Collections.Generic;
using System.Text;

using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.SyncWS.Framework
{
    public class WarehouseException : Exception
    {
        private string alarmId;
        private string alarmText;

        public WarehouseException(string alarmId, string alarmText)
        {
            this.alarmId = alarmId;
            this.alarmText = alarmText;
        }

        public WarehouseException(string alarmId, Database database, string localizationCode)
        {
            string nlangcod = "";

            this.alarmId = alarmId;
            alarmText = "";

            // no conversion needed (?), only to uppercase
            nlangcod = localizationCode.ToUpper();

            try
            {
                Wlsystem pkg = new Wlsystem(database);
                pkg.Getalmtxt(alarmId, nlangcod, ref alarmText);
            }
            catch (Exception e)
            {
                alarmText = e.Message;
            }
        }

        public override string Message
        {
            get
            {
                return (string.Format("{0} - {1}", alarmId, alarmText));
            }
        }
    }
}
