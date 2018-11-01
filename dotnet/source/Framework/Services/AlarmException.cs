using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Services
{
    [global::System.Serializable]
    public class AlarmException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        private string alarmId;
        private int? position;
        private string additionalInformation;

        public AlarmException() { }
        public AlarmException(string alarmId, string message, int? position) : this(alarmId, message, position,null) { }
        public AlarmException(string alarmId, string message, int? position, Exception inner)
            : base(message, inner)
        {
            this.alarmId = alarmId;
            this.position = position;
        }
        public AlarmException(string alarmId, string message, int? position, string additional, Exception inner)
            : base(message, inner)
        {
            this.alarmId = alarmId;
            this.position = position;
            this.additionalInformation = additional;
        }

        protected AlarmException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public string AlarmId
        {
            get
            {
                return alarmId;
            }
        }

        public int? Position
        {
            get
            {
                return position;
            }
        }

        public string AdditionalInformation
        {
            get
            {
                return additionalInformation;
            }
        }

        public override string ToString()
        {
            string str = AdditionalInformation;
            if (!String.IsNullOrEmpty(str))
            {
                str = str + " " + base.ToString();
            }
            else
            {
                str = base.ToString();
            }
            return str;

        }
    }
}
