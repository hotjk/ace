using System;

namespace ACE.Loggers
{
    public interface IBusLogger
    {
        void Sent(object message);
        void Received(object message);
        void Exception(object message, Exception ex);
        void Debug(string message);
    }
}
