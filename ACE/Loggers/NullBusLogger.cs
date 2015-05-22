using System;

namespace ACE.Loggers
{
    public class NullBusLogger : IBusLogger
    {
        public void Sent(object message)
        {
        }

        public void Received(object message)
        {
        }

        public void Exception(object message, Exception ex)
        {
        }

        public void Debug(string message)
        {
        }
    }
}
