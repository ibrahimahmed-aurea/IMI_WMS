using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Imi.Framework.Shared.Security
{
    public class AuthenticationSession
    {
        private string sessionId;
        private string schemaUser;
        private string schemaPassword;

        public string SchemaPassword
        {
            get { return schemaPassword; }
            set { schemaPassword = value; }
        }
	
        public string SchemaUser
        {
            get { return schemaUser; }
            set { schemaUser = value; }
        }
	
        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
	
    }

    public class AuthenticationProvider
    {
        private string hashHex;
        private string saltHex;
        private byte[] iv;
        private byte[] key;
        private TripleDESCryptoServiceProvider provider;

        public AuthenticationProvider()
        {
            provider = new TripleDESCryptoServiceProvider();
            provider.Padding = PaddingMode.PKCS7;
            provider.Mode = CipherMode.CBC;
            provider.KeySize = 192;
            
        }

        private void GenerateIV()
        {
            string ivData = "0123456789ABCDEF";

            iv = new byte[8];

            for (int i = 0; i < ivData.Length; i += 2)
            {
                iv[i / 2] = Convert.ToByte(ivData.Substring(i, 2), 16);
            }
        }

        public void Initialize(string password)
        {
            GenerateIV();

            byte[] salt = new byte[16];

            //Fill data with random bytes
            new Random(unchecked((int)DateTime.Now.Ticks)).NextBytes(salt);

            for (int i = 0; i < 16; i++)
                salt[i] = 0;

            saltHex = ByteToHex(salt);

            //Zero pad password
            byte[] passBytes = ASCIIEncoding.ASCII.GetBytes(password.PadRight(16, '0'));

            MD5 md5 = MD5.Create();

            //Hash password
            byte[] passwordHash = md5.ComputeHash(passBytes);

            //Convert password hash to hex
            hashHex = ByteToHex(passwordHash);

            key = GenerateKey(passwordHash, salt);
        }

        public AuthenticationSession DecryptSession(string sessionData)
        {
            byte[] data = HexToByte(sessionData);
            
            data = provider.CreateDecryptor(key, iv).TransformFinalBlock(data, 0, data.Length);

            string plainText = ASCIIEncoding.ASCII.GetString(data);

            AuthenticationSession session = new AuthenticationSession();
            session.SchemaUser = plainText.Split('\t')[0];
            session.SchemaPassword = plainText.Split('\t')[1];
            session.SessionId = plainText.Split('\t')[2];

            return session;
        }

        public string SessionData
        {
            get
            {
                byte[] data = GetBytes(hashHex);

                byte[] cipher = provider.CreateEncryptor(key, iv).TransformFinalBlock(data, 0, data.Length);

                return ByteToHex(cipher);
            }
        }

        private string ByteToHex(byte[] data)
        {
            string hex = "";

            //Convert bytes to hex
            foreach (byte b in data)
            {
                hex += Convert.ToString(b, 16).PadLeft(2, '0');
            }

            return hex.ToUpper();
        }

        private byte[] HexToByte(string hex)
        {
            byte[] data = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length; i += 2)
            {
                data[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return data;
        }

        private byte[] GetBytes(string plainText)
        {
            byte[] data = ASCIIEncoding.ASCII.GetBytes(plainText);

            /*
            int l = 8 - data.Length % 8;

            byte[] padBytes = new byte[l];

            for (int i = 0; i < l; i++)
                padBytes[i] = (byte)l;
            
            byte[] result = new byte[data.Length + l];

            data.CopyTo(result, 0);
            Array.Copy(padBytes, 0, result, data.Length, l);

            return result;
            */

            return data;
        }
                
        public string Salt
        {
            get
            {
                return saltHex;
            }
        }

        private byte[] GenerateKey(byte[] passwordHash, byte[] salt)
        {
            byte[] data = new byte[32];
            byte[] key = new byte[24];
            Array.Copy(passwordHash, 0, data, 0, 16);
            Array.Copy(salt, 0, data, 16, 16);

            MD5 md5 = MD5.Create();

            for (int i = 1; i <= 10000; i++)
            {
                data = md5.ComputeHash(data);
            }

            Array.Copy(data, key, 16);

            data = md5.ComputeHash(data);

            Array.Copy(data, 0, key, 16, 8);

            return key;
        }
        
    }
}
