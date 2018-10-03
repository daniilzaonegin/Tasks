using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Abstract
{
    public interface IAppSettings
    {
        string EmailAccount { get; }
        SecureString EmailPwd { get; }
        string SmtpServer { get; }
        int SmtpPort { get; }
        int ResetPasswordTokenExpireTime { get; }
    }
}
