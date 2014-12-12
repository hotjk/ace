using Grit.ACE.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grit.ACE
{
    public class Event : DomainMessage, IEvent
    {
        public Event()
        {
            EventId = Guid.NewGuid();
            DistributionOptions = EventDistributionOptions.BalckHole;
        }
        
        public override Guid Id
        {
            get
            {
                return EventId;
            }
        }

        public EventDistributionOptions DistributionOptions { get; private set; }
        public Event DistributeInCurrentThread()
        {
            this.DistributionOptions = this.DistributionOptions | EventDistributionOptions.CurrentThread;
            return this;
        }
        public Event DistributeInThreadPool()
        {
            this.DistributionOptions = this.DistributionOptions | EventDistributionOptions.ThreadPool;
            return this;
        }

        public Event DistributeToExternalQueue()
        {
            this.DistributionOptions = this.DistributionOptions | EventDistributionOptions.Queue;
            return this;
        }

        public Guid EventId { get; set; }
    }
}
