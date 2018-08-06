using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Configuration;
using System.Web.Configuration;

namespace Tasks.Controllers
{
    public class AdminController : Controller
    {

        [Authorize(Roles ="Admin")]
        // GET: Admin
        public ActionResult EncryptPasswords()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EncryptPasswords(string emailPassword)
        {
            //SecureString secureString = new SecureString();

            //foreach (char c in EmailPassword)
            //{
            //    secureString.AppendChar(c);
            //}

            //IntPtr ptr = Marshal.SecureStringToBSTR(secureString);
            //string result="";
            //try
            //{
            //    result = Marshal.PtrToStringBSTR(ptr);
            //}
            //finally
            //{
            //    Marshal.ZeroFreeBSTR(ptr);
            //}
            byte[] encryptedData = ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(emailPassword),null,DataProtectionScope.LocalMachine);

            string result = Convert.ToBase64String(encryptedData);

            Configuration config = WebConfigurationManager.OpenWebConfiguration("/");

            if (config.AppSettings.Settings["EmailPassword"] != null)
            {
                config.AppSettings.Settings["EmailPassword"].Value = result;
                config.Save();
            }

            return View("Success",model:result);
        }
    }
}