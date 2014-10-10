using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS.Events
{
    public class EventWorker
    {
        public void Execute(Grit.CQRS.Event @event)
        {
            try
            {
                ServiceLocator.EventBus.Handle(@event);
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("exception.logger").Error(ex);
            }
        }
    }
}
