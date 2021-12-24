﻿using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BoggleService.Utils
{
    internal class EmailSender
    {
        private readonly SmtpClient client;
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly string host = ConfigurationManager.AppSettings["SmtpServer"];
        private readonly int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        private readonly string addressFrom = ConfigurationManager.AppSettings["AddressFrom"];
        private readonly string addressTo;

        public EmailSender(string addressTo)
        {
            log.Debug("Starting email sender");
            this.addressTo = addressTo;
            client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,

                Credentials = new NetworkCredential
                {
                    UserName = addressFrom,
                    Password = ConfigurationManager.AppSettings["PasswordFrom"]
                }
            };
        }

        public void SendEmail(string subject, string body)
        {
            MailMessage mail = new MailMessage(addressFrom, addressTo, subject, body)
            {
                IsBodyHtml = false,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8
            };
            try
            {
                client.Send(mail);
                log.Info("Email sent");
            }
            catch (SmtpException smtpException)
            {
                log.Error(smtpException.Message, smtpException);
            }
        }
    }
}
