namespace ACE
{
    /// <summary>
    /// EventBus instance MUST be thread scope, since published event should be cached in thread variable and flushed when UnitOfWork completed
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publish event will cache it in thread until UnitOfWork completed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        void Publish<T>(T @event) where T : IEvent;

        /// <summary>
        /// Flush all cached events.
        /// </summary>
        void Flush();

        /// <summary>
        /// Direct flush an event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        void FlushAnEvent<T>(T @event) where T : IEvent;

        /// <summary>
        /// Clear all cached events in thread.
        /// </summary>
        void Purge();
    }
}
