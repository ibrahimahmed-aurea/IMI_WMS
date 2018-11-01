using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessEntities
{
    public class AuthenticateUserParams
    {
        private string userName;
        private string salt;
        private string data;
        private string nodeIdentity;
        private string product;
        private string terminalIdentity;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Salt
        {
            get { return salt; }
            set { salt = value; }
        }

        public string Data
        {
            get { return data; }
            set { data = value; }
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

        public string TerminalIdentity
        {
            get { return terminalIdentity; }
            set { terminalIdentity = value; }
        }

    }
}
