using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Loggers
{
    public class NullBusLogger : IBusLogger
    {
        public void Sent(DomainMessage message)
        {
            message.MarkAsSent();
        }

        public void Received(DomainMessage message)
        {
            message.MarkAsReceived();
        }

        public void Exception(DomainMessage message, Exception ex)
        {
        }

        public void Debug(string message)
        {
        }
    }
}
