using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.ACE.Loggers
{
    public class NullBusLogger : IBusLogger
    {
        public void ActionSend(Action action)
        {
        }

        public void ActionInvoke(Action action)
        {
        }

        public void CommandSend(Command command)
        {
        }

        public void EventPublish(Event @event)
        {
        }

        public void EventHandle(Event @event)
        {
        }

        public void Exception(DomainMessage message, Exception ex)
        {
        }

        public void Debug(string message)
        {
        }
    }
}
