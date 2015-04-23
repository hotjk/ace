using log4net;
using Newtonsoft.Json;
using System;

namespace ACE.Loggers
{
    public class Log4NetBusLogger : IBusLogger
    {
        public ILog MessageLogger { get; private set; }
        public ILog ExceptionLogger { get; private set; }
        public ILog DebugLogger { get; private set; }

        public Log4NetBusLogger()
        {
            MessageLogger = log4net.LogManager.GetLogger("message.logger");
            ExceptionLogger = log4net.LogManager.GetLogger("exception.logger");
            DebugLogger = log4net.LogManager.GetLogger("debug.logger");
        }

        public void Sent(QDomainMessage message)
        {
            message.MarkAsSent();
            if (MessageLogger.IsInfoEnabled)
            {
                MessageLogger.Info(JsonConvert.SerializeObject(message));
            }
        }

        public void Received(QDomainMessage message)
        {
            message.MarkAsReceived();
            if (MessageLogger.IsInfoEnabled)
            {
                MessageLogger.Info(JsonConvert.SerializeObject(message));
            }
        }

        public void Exception(QDomainMessage message, Exception ex)
        {
            ExceptionLogger.Error(JsonConvert.SerializeObject(message), ex);
        }

        public void Debug(string message)
        {
            DebugLogger.Debug(message);
        }
    }
}
