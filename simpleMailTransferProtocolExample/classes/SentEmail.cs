using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleMailTransferProtocolExample.classes
{
    public class SentEmail
    {
        public string MessageId { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DateSent { get; set; }
    }
}
