using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDT.Common.Plugins.Email
{
    public class EmailConfig
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string From { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public bool WithSSL { get; set; }

    }
}
