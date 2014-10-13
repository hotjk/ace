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
        public Event(bool distributeToQueue = true)
        {
            EventId = Guid.NewGuid();
            this.DistributeToQueue = distributeToQueue;
        }
        
        public bool DistributeToQueue { get; set; }

        public override Guid Id
        {
            get
            {
                return EventId;
            }
        }

        public Guid ActionId { get; set; }
        public Guid CommandId { get; set; }
        public Guid EventId { get; set; }
    }
}
