using System;

namespace ACE.Loggers
{
    public interface IBusLogger
    {
        void Sent(QDomainMessage message);
        void Received(QDomainMessage message);
        void Exception(QDomainMessage message, Exception ex);
        void Debug(string message);
    }
}
