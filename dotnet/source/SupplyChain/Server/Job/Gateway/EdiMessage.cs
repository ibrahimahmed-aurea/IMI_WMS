using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Server.Job.Gateway
{
    public class EdiMessage
    {
        private string preString;
        private string postString;
        private string unb;
        private string unh;
        private string unt;
        private string unz;

        public double EdiOutId { get; set; }
        public string EdiMessageId { get; set; }
        public string SendDirectory { get; set; }

        public string PreString
        {
            get
            {
                return preString;
            }

            set
            {
                preString = value;
                unb = string.Empty;
                unh = string.Empty;

                if (!string.IsNullOrEmpty(preString))
                {
                    String[] strings = preString.Split(new char[] { '\n' }, 2);
                    if (strings.Length == 2)
                    {
                        unb = strings[0];
                        unh = strings[1];
                    }
                }

            }
        }

        public string PostString 
        {
            get
            {
                return postString;
            }

            set
            {
                postString = value;
                unt = string.Empty;
                unz = string.Empty;

                if (!string.IsNullOrEmpty(postString))
                {
                    String[] strings = postString.Split(new char[] { '\n' }, 2);
                    if (strings.Length == 2)
                    {
                        unt = strings[0];
                        unz = strings[1];
                    }
                }

            }

        }

        public string Unb
        {
            get
            {
                return unb;
            }
        }

        public string Unh
        {
            get
            {
                return unh;

            }
        }

        public string Unt
        {
            get
            {
                return unt;
            }
        }

        public string Unz
        {
            get
            {
                return unz;
            }
        }

    }
}
