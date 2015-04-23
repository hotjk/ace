using System;

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
