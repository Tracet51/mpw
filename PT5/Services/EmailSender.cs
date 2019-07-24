using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MPW.Services
{
    public class EmailSender : IEmailSender
    {
        #region Properties
        private string host;
        private int port;
        private bool enableSSL;
        private string userName;
        private string password;
        #endregion


        // Get our parameterized configuration
        public EmailSender(string host, int port, bool enableSSL, string userName, string password)
        {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.userName = userName;
            this.password = password;
        }

        public EmailSender(EmailSenderConfiguation config)
        {
            this.host = config.Host;
            this.port = config.Port;
            this.enableSSL = config.EnableSSL;
            this.userName = config.Username;
            this.password = config.Password;
        }

        // Use our configuration to send the email by using SmtpClient
        public virtual Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var client = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(userName, password),
                    EnableSsl = true
                };

                return client.SendMailAsync(
                    new MailMessage(userName, email, subject, htmlMessage) { IsBodyHtml = true }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.CompletedTask;
            }

        }
    }
}
