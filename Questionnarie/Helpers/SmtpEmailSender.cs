﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Tasks.Abstract;

namespace Tasks.Helpers
{
    public class SmtpEmailSender : IEmailSender
    {
        private IAppSettings appSettings;
        public SmtpEmailSender(IAppSettings settings)
        {
            appSettings = settings;
        }
        public void SendEmail(string to, string subject, string body)
        {
            if (appSettings.SmtpPort == -1)
            {
                return;
            }
            SmtpClient client = new SmtpClient(appSettings.SmtpServer, appSettings.SmtpPort);

            client.Credentials = new NetworkCredential(appSettings.EmailAccount, appSettings.EmailPwd); //);
            client.EnableSsl = true;

            client.Send("Tasks4uApp " + appSettings.EmailAccount, to, subject, body);
        }

    }
}