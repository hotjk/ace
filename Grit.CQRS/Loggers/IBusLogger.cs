using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS.Loggers
{
    public interface IBusLogger
    {
        void ActionSend(Action action);
        void ActionBeginInvoke(Action action);
        void ActionEndInvoke(Action action);

        void CommandSend(Command command);

        void EventPublish(Event @event);
        void EventHandle(Event @event);

        void ActionException(Action action, Exception ex);
        void CommandException(Command command, Exception ex);
        void EventException(Event @event, Exception ex);

        void Debug(string message);
    }
}
