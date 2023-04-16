using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleMailTransferProtocolExample
{
    class messageList
    {
        public int id { get; set; }
        public string from { get; set; }

        public string subject { get; set; }

        public string body { get; set; }

        public MimeMessage message { get; set; }
    }
}
