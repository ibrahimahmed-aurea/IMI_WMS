using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Imi.SupplyChain.Server.Job.Gateway
{
    public class EdiPartner
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MessageId { get; set; }
        public string MessageVersion { get; set; }
        public string MessageFrom { get; set; }
        public string MessageTo { get; set; }
        public string MessagePassword { get; set; }
        public string MessageTest { get; set; }
        public string ReceiveDirectory { get; set; }
        public string ReceiveDirectorySave { get; set; }
        public string ReceiveDirectoryError { get; set; }
        public string SendDirectory { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            return (Equals(obj as EdiPartner));
        }

        public bool Equals(EdiPartner otherPartner)
        {
            // If parameter is null return false:
            if ((object)otherPartner == null)
            {
                return false;
            }

            return
                (
                    (otherPartner.Id == Id) &&
                    (otherPartner.Name == Name) &&
                    (otherPartner.MessageFrom == MessageFrom) &&
                    (otherPartner.MessageId == MessageId) &&
                    (otherPartner.MessagePassword == MessagePassword) &&
                    (otherPartner.MessageTo == MessageTo) &&
                    (otherPartner.MessageTest == MessageTest) &&
                    (otherPartner.MessageVersion == MessageVersion) &&
                    (otherPartner.ReceiveDirectory == ReceiveDirectory) &&
                    (otherPartner.ReceiveDirectoryError == ReceiveDirectoryError) &&
                    (otherPartner.ReceiveDirectorySave == ReceiveDirectorySave) &&
                    (otherPartner.SendDirectory == SendDirectory)
                    );
        }

        public override int GetHashCode()
        {
            int result = Id.GetHashCode();
            result ^= Name.GetHashCode();
            result ^= MessageFrom.GetHashCode();
            result ^= MessageId.GetHashCode();
            result ^= MessagePassword.GetHashCode();
            result ^= MessageTo.GetHashCode();
            result ^= MessageTest.GetHashCode();
            result ^= MessageVersion.GetHashCode();
            result ^= ReceiveDirectory.GetHashCode();
            result ^= ReceiveDirectoryError.GetHashCode();
            result ^= ReceiveDirectoryError.GetHashCode();
            result ^= ReceiveDirectorySave.GetHashCode();
            result ^= SendDirectory.GetHashCode();

            return result;
        }

        public bool IsValid { get; set; }
    }
}
