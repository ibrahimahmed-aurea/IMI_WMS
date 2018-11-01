using System;
using System.Security.Cryptography;

namespace Imi.Framework.HashLib
{
    public static class Hashing
    {
        public static string Get(Hash.HashTypes hashType, string originalString)
        {
            Hash pHash = new Hash();

            return pHash.CreateHash(originalString, hashType);
        }

        public static string Get(Hash.HashTypes hashType, string originalString, string saltString)
        {
            Hash pHash = new Hash();

            return pHash.CreateHash(originalString, hashType, saltString);
        }

        public static bool Verify(Hash.HashTypes hashType, string checkString, string hashValue)
        {
            Hash pHash = new Hash();

            string theHash = pHash.CreateHash(checkString, hashType);

            return (theHash == hashValue);
        }

        public static bool Verify(Hash.HashTypes hashType, string checkString, string hashValue, string saltString)
        {
            Hash pHash = new Hash();

            string theHash = pHash.CreateHash(checkString, hashType, saltString);

            return (theHash == hashValue);
        }
    }

    public class Hash
    {
        /***********************************************
          Private Variables
        ***********************************************/
        private HashTypes mbytHashType;
        private string mstrOriginalString;
        private string mstrHashString;
        private HashAlgorithm mhash;
        bool mboolUseSalt;
        string mstrSaltValue = String.Empty;
        short msrtSaltLength = 8;

        public enum HashTypes : byte
        {
            MD5,
            SHA1
        }

        #region "Public Properties"

        public HashTypes HashType
        {
            get 
            {
                return mbytHashType;
            }
            set
            {
                if (mbytHashType != value) 
                {
                    mbytHashType = value;
                    mstrOriginalString = String.Empty;
                    mstrHashString = String.Empty;

                    this.SetEncryptor();
                }
            }
        }

        public string SaltValue
        {
            get { return mstrSaltValue; }
            set { mstrSaltValue = value; }
        }

        public bool UseSalt
        {
            get { return mboolUseSalt; }
            set { mboolUseSalt = value; }
        }

        public short SaltLength
        {
            get { return msrtSaltLength; }
            set { msrtSaltLength = value; }
        }

        public string OriginalString
        {
            get { return mstrOriginalString; }
            set { mstrOriginalString = value; }
        }

        public string HashString
        {
            get { return mstrHashString; }
            set { mstrHashString = value; }
        }

        #endregion

        #region "Constructors"

        public Hash()
        {
            mbytHashType = HashTypes.SHA1;
        }

        public Hash(HashTypes HashType)
        {
            mbytHashType = HashType;
        }

        public Hash(HashTypes HashType, string OriginalString)
        {
            mbytHashType = HashType;
            mstrOriginalString = OriginalString;
        }

        public Hash(HashTypes HashType, string OriginalString, bool UseSalt, string SaltValue)
        {
            mbytHashType = HashType;
            mstrOriginalString = OriginalString;
            mboolUseSalt = UseSalt;
            mstrSaltValue = SaltValue;
        }

        #endregion

        #region "SetEncryptor() Method"

        private void SetEncryptor()
        {
            switch(mbytHashType)
            {
                case HashTypes.MD5:
                    mhash = new MD5CryptoServiceProvider();
                    break;
                case HashTypes.SHA1:
                    mhash = new SHA1CryptoServiceProvider();
                    break;
            }
        }

        #endregion

        #region "CreateHash() Methods"

        public string CreateHash()
        {
            byte[] bytValue;
            byte[] bytHash;

            // Create New Crypto Service Provider Object
            this.SetEncryptor();

            // Check to see if we will Salt the value
            if (mboolUseSalt)
                if (mstrSaltValue.Length == 0)
                    mstrSaltValue = this.CreateSalt();

            // Convert the original string to array of Bytes
            bytValue = System.Text.Encoding.UTF8.GetBytes(mstrSaltValue + mstrOriginalString);

            // Compute the Hash, returns an array of Bytes  
            bytHash = mhash.ComputeHash(bytValue);

            // Return a base 64 encoded string of the Hash value
            return Convert.ToBase64String(bytHash);
        }

        public string CreateHash(string OriginalString)
        {
            mstrOriginalString = OriginalString;
          
            return this.CreateHash();
        }

        public string CreateHash(string OriginalString, HashTypes HashType)
        {
            mstrOriginalString = OriginalString;
            mbytHashType = HashType;

            return this.CreateHash();
        }

        public string CreateHash(string OriginalString, bool UseSalt)
        {
            mstrOriginalString = OriginalString;
            mboolUseSalt = UseSalt;

            return this.CreateHash();
        }

        public string CreateHash(string OriginalString, HashTypes HashType, string SaltValue)
        {
            mstrOriginalString = OriginalString;
            mbytHashType = HashType;
            mstrSaltValue = SaltValue;

            return this.CreateHash();
        }

        public string CreateHash(string OriginalString, string SaltValue)
        {
            mstrOriginalString = OriginalString;
            mstrSaltValue = SaltValue;

            return this.CreateHash();
        }

        #endregion

        #region "Misc. Routines"

        public void Reset()
        {
            mstrSaltValue = String.Empty;
            mstrOriginalString = String.Empty;
            mstrHashString = String.Empty;
            mboolUseSalt = false;
            mbytHashType = HashTypes.SHA1;

            mhash = null;
        }

        public string CreateSalt()
        {
            byte[] bytSalt = new byte[8];
            RNGCryptoServiceProvider rng;

            rng = new RNGCryptoServiceProvider();

            rng.GetBytes(bytSalt);

            return Convert.ToBase64String(bytSalt);
        }

#endregion

    }
}
