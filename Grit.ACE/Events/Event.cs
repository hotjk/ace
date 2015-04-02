namespace ACE
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
            _options = EventDistributionOptions.BalckHole;
        }

        #region Distribution Options

        public EventDistributionOptions _options { get; private set; }

        internal bool ShouldDistributeInCurrentThread()
        {
            return (this._options & EventDistributionOptions.CurrentThread) == EventDistributionOptions.CurrentThread;
        }
        internal bool ShouldDistributeInThreadPool()
        {
            return (this._options & EventDistributionOptions.ThreadPool) == EventDistributionOptions.ThreadPool;
        }
        internal bool ShouldDistributeToExternalQueue()
        {
            return (this._options & EventDistributionOptions.Queue) == EventDistributionOptions.Queue;
        }

        public Event InCurrentThread()
        {
            this._options = this._options | EventDistributionOptions.CurrentThread;
            return this;
        }
        public Event InThreadPool()
        {
            this._options = this._options | EventDistributionOptions.ThreadPool;
            return this;
        }
        public Event ToExternalQueue()
        {
            this._options = this._options | EventDistributionOptions.Queue;
            return this;
        }

        #endregion
    }
}
