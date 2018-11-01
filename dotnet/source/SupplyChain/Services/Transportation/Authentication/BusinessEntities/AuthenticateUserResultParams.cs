using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessEntities
{
    public class AuthenticateUserResultParams
    {
        private string sessionData;
        private string almid;
        private string alarmText1;
        private string alarmText2;
        private string alarmText3;

        public string SessionData
        {
            get { return sessionData; }
            set { sessionData = value; }
        }

        public string Almid
        {
            get { return almid; }
            set { almid = value; }
        }

        public string AlarmText1
        {
            get { return alarmText1; }
            set { alarmText1 = value; }
        }

        public string AlarmText2
        {
            get { return alarmText2; }
            set { alarmText2 = value; }
        }

        public string AlarmText3
        {
            get { return alarmText3; }
            set { alarmText3 = value; }
        }
    }
}
