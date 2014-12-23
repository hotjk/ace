using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.ACE.Loggers
{
    public class NullBusLogger : IBusLogger
    {
        public void Sent(DomainMessage message)
        {
            message.Sent();
        }

        public void Received(DomainMessage message)
        {
            message.Recevied();
        }

        public void Exception(DomainMessage message, Exception ex)
        {
        }

        public void Debug(string message)
        {
        }
    }
}
