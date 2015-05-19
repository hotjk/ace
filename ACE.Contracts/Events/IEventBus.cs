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
        void Publish<T>(T @event) where T : Event;

        /// <summary>
        /// Flush all cached events.
        /// </summary>
        void Flush();

        /// <summary>
        /// Direct flush an event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        void FlushAnEvent<T>(T @event) where T : Event;

        /// <summary>
        /// Direct handle a event in current thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        void Invoke<T>(T @event) where T : Event;

        /// <summary>
        /// Clear all cached events in thread.
        /// </summary>
        void Purge();

        /// <summary>
        /// Subscribe event from RabbitMQ.
        /// </summary>
        /// <param name="subscriptionId">A unique identifier for the subscription. Two subscriptions with the same subscriptionId and type will get messages delivered in turn. </param>
        /// <param name="topic">RabbitMQ exchange routing key</param>
        void Subscribe(string subscriptionId, string[] topics);

        /// <summary>
        /// Subscribe event from RabbitMQ in parallel.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="topic"></param>
        /// <param name="capacity">Worker numbers</param>
        void SubscribeInParallel(string subscriptionId, string[] topics, int capacity);
    }
}
