using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS.Loggers
{
    public class Log4NetBusLogger : IBusLogger
    {
        public ILog CommandLogger { get; private set; }
        public ILog EventLogger { get; private set; }
        public ILog ActionLogger { get; private set; }
        public ILog ExceptionLogger { get; private set; }
        public ILog Logger { get; private set; }

        public Log4NetBusLogger()
        {
            CommandLogger = log4net.LogManager.GetLogger("command.logger");
            EventLogger = log4net.LogManager.GetLogger("event.logger");
            ActionLogger = log4net.LogManager.GetLogger("action.logger");
            ExceptionLogger = log4net.LogManager.GetLogger("exception.logger");
            Logger = log4net.LogManager.GetLogger("exception.logger");
        }

        public void ActionSend(Action action)
        {
            if (ActionLogger.IsInfoEnabled)
            {
                ActionLogger.Info(
                    string.Format("Action Send {0} {1}", action,
                    JsonConvert.SerializeObject(action)));
            }
        }

        public void ActionBeginInvoke(Action action)
        {
            ActionLogger.Info(
                    string.Format("Action BeginInvoke {0}", action.Id));
        }

        public void ActionEndInvoke(Action action)
        {
            ActionLogger.Info(
                        string.Format("Action EndInvoke {0}", action.Id));
        }

        public void CommandSend(Command command)
        {
            if (CommandLogger.IsInfoEnabled)
            {
                CommandLogger.Info(
                    string.Format("Command Send {0} {1}",
                    command,
                    JsonConvert.SerializeObject(command))); ;
            }
        }

        public void EventPublish(Event @event)
        {
            if (EventLogger.IsInfoEnabled)
            {
                EventLogger.Info(
                    string.Format("Event Publish {0} {1}",
                    @event,
                    JsonConvert.SerializeObject(@event)));
            }
        }

        public void EventHandle(Event @event)
        {
            EventLogger.Info(
                string.Format("Event Handle {0}", @event.Id));
        }

        public void Exception(DomainMessage message, Exception ex)
        {
            ExceptionLogger.Error(
                                new Exception(string.Format("{0} {1}",
                                    message.Type,
                                    message.Id),
                                    ex));
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }
    }
}
