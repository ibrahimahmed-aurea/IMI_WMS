using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessEntities
{
    public class LogonUserResultParams
    {
        private string cookie;
        private string configMenuAccess;
        private string traceMenuAccess;
        private string employeeAccess;
        private string employeeFileMenuAccess;
        private string almid;
        private string alarmText1;
        private string alarmText2;
        private string alarmText3;

        public string Cookie
        {
            get { return cookie; }
            set { cookie = value; }
        }

        public string ConfigMenuAccess
        {
            get { return configMenuAccess; }
            set { configMenuAccess = value; }
        }

        public string TraceMenuAccess
        {
            get { return traceMenuAccess; }
            set { traceMenuAccess = value; }
        }

        public string EmployeeAccess
        {
            get { return employeeAccess; }
            set { employeeAccess = value; }
        }

        public string EmployeeFileMenuAccess
        {
            get { return employeeFileMenuAccess; }
            set { employeeFileMenuAccess = value; }
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
