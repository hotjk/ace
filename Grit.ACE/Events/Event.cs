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
        public enum EventDistributionOptions
        {
            BalckHole = 0,
            CurrentThread = 1,
            ThreadPool = 2,
            Queue = 4,
        }

        public Event()
        {
            DistributionOptions = EventDistributionOptions.BalckHole;
        }

        #region Distribution Options

        public EventDistributionOptions DistributionOptions { get; private set; }

        public bool ShouldDistributeInCurrentThread()
        {
            return (this.DistributionOptions & EventDistributionOptions.CurrentThread) == EventDistributionOptions.CurrentThread;
        }
        public bool ShouldDistributeInThreadPool()
        {
            return (this.DistributionOptions & EventDistributionOptions.ThreadPool) == EventDistributionOptions.ThreadPool;
        }
        public bool ShouldDistributeToExternalQueue()
        {
            return (this.DistributionOptions & EventDistributionOptions.Queue) == EventDistributionOptions.Queue;
        }

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

        #endregion
    }
}
