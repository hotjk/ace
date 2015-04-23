using System;

namespace ACE.Loggers
{
    public class NullBusLogger : IBusLogger
    {
        public void Sent(QDomainMessage message)
        {
            message.MarkAsSent();
        }

        public void Received(QDomainMessage message)
        {
            message.MarkAsReceived();
        }

        public void Exception(QDomainMessage message, Exception ex)
        {
        }

        public void Debug(string message)
        {
        }
    }
}
