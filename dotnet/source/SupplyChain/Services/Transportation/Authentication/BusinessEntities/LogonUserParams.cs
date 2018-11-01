using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessEntities
{
    public class LogonUserParams
    {
        private string userName;
        private string sessionIdentity;
        private string terminalIdentity;
        private string nodeIdentity;
        private string product;


        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string SessionIdentity
        {
            get { return sessionIdentity; }
            set { sessionIdentity = value; }
        }

        public string TerminalIdentity
        {
            get { return terminalIdentity; }
            set { terminalIdentity = value; }
        }

        public string NodeIdentity
        {
            get { return nodeIdentity; }
            set { nodeIdentity = value; }
        }

        public string Product
        {
            get { return product; }
            set { product = value; }
        }
    }
}
