using Grit.ACE.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.ACE.Loggers
{
    public interface IBusLogger
    {
        void ActionSend(Action action);
        void ActionBeginInvoke(Action action);
        void ActionEndInvoke(Action action);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">Command is a big object, do not serialize it.</param>
        void CommandSend(Command command);

        void EventPublish(Event @event);
        void EventHandle(Event @event);

        void Exception(DomainMessage message, Exception ex);

        void Debug(string message);
    }
}
