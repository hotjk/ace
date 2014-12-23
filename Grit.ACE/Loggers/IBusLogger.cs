using ACE.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Loggers
{
    public interface IBusLogger
    {
        void Sent(DomainMessage message);
        void Received(DomainMessage message);
        void Exception(DomainMessage message, Exception ex);
        void Debug(string message);
    }
}
