/**
 * Copyright (c) 2012, Yubico AB.  All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * * Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 *  CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 *  INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 *  MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 *  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS
 *  BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
 *  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 *  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 *  ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
 *  TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF
 *  THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 *  SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Imi.SupplyChain.Services.IdentityModel.Yubico
{
    public class YubicoResponseImpl : YubicoResponse
    {
        private string _h;
        private string _t;
        private YubicoResponseStatus _status;
        private int _timestamp;
        private int _sessionCounter;
        private int _useCounter;
        private string _sync;
        private string _otp;
        private string _nonce;
        private SortedDictionary<string, string> _responseMap;

        public YubicoResponseImpl(string response)
        {
            StringReader reader = new StringReader(response);
            string line;
            _responseMap = new SortedDictionary<string, string>();
            
            while ((line = reader.ReadLine()) != null)
            {
                bool unhandled = false;
                string[] parts = line.Split(new char[] { '=' }, 2);
                switch (parts[0])
                {
                    case "h":
                        _h = parts[1];
                        break;
                    case "t":
                        _t = parts[1];
                        break;
                    case "status":
                        _status = (YubicoResponseStatus)Enum.Parse(typeof(YubicoResponseStatus), parts[1], true);
                        break;
                    case "timestamp":
                        _timestamp = int.Parse(parts[1]);
                        break;
                    case "sessioncounter":
                        _sessionCounter = int.Parse(parts[1]);
                        break;
                    case "sessionuse":
                        _useCounter = int.Parse(parts[1]);
                        break;
                    case "sl":
                        _sync = parts[1];
                        break;
                    case "otp":
                        _otp = parts[1];
                        break;
                    case "nonce":
                        _nonce = parts[1];
                        break;
                    default:
                        unhandled = true;
                        break;
                }

                if (!unhandled)
                {
                    _responseMap.Add(parts[0], parts[1]);
                }
            }

            if (_status == YubicoResponseStatus.EMPTY)
            {
                throw new YubicoValidationException("Response is malformed.");
            }
        }

        public string GetH()
        {
            return _h;
        }

        public string GetT()
        {
            return _t;
        }

        public YubicoResponseStatus GetStatus()
        {
            return _status;
        }

        public int GetTimestamp()
        {
            return _timestamp;
        }

        public int GetSessionCounter()
        {
            return _sessionCounter;
        }

        public int GetUseCounter()
        {
            return _useCounter;
        }

        public string GetSync()
        {
            return _sync;
        }

        public string GetOtp()
        {
            return _otp;
        }

        public string GetNonce()
        {
            return _nonce;
        }

        public SortedDictionary<string, string> GetResponseMap()
        {
            return _responseMap;
        }

        public string GetPublicId()
        {
            if (_otp == null || !YubicoClient.IsOtpValidFormat(_otp))
            {
                return null;
            }

            return _otp.Substring(0, _otp.Length - 32);
        }
    }
}
