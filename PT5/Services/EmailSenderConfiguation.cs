using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPW.Services
{
    public class EmailSenderConfiguation
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public EmailSenderConfiguation(string host, int port, bool enableSSL, string userName, string password)
        {
            this.Host = host;
            this.Port = port;
            this.EnableSSL = enableSSL;
            this.Username = userName;
            this.Password = password;
        }
    }
}
