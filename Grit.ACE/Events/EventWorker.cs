using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.ACE.Events
{
    public class EventWorker
    {
        public void Execute(Grit.ACE.Event @event)
        {
            try
            {
                ServiceLocator.EventBus.Invoke(@event);
            }
            catch (Exception ex)
            {
                ServiceLocator.BusLogger.Exception(@event, ex);
            }
        }
    }
}
