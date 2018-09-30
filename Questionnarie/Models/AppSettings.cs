using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using Tasks.Abstract;

namespace Tasks.Models
{
    public class AppSettings : IAppSettings
    {
        public string EmailAccount {
            get { return ConfigurationManager.AppSettings["EmailAccount"]; }
        }
        public SecureString EmailPwd {
            get {
                if (ConfigurationManager.AppSettings["EmailPassword"] == null)
                    return null;
                try
                {
                    byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(ConfigurationManager.AppSettings["EmailPassword"]),
                        null,DataProtectionScope.LocalMachine);
                    return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
                }
                catch (CryptographicException)
                {
                    return null;
                }
            }
        }

        private SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public string SmtpServer {
            get { return ConfigurationManager.AppSettings["SmtpServer"]; }
        }

        public int SmtpPort {
            get {
                int port;
                if (int.TryParse(ConfigurationManager.AppSettings["SmtpServerPort"], out port))
                    return port;
                else
                    return -1;
            }
        }

    }
}