using System;

namespace ACE
{
    /// <summary>
    /// ActionStation
    /// </summary>
    public interface IActionStation
    {
        /// <summary>
        /// Invoke action handler method in current thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        void Invoke<T>(T action) where T : IAction;

        /// <summary>
        /// Subscribe action from RabbitMQ.
        /// </summary>
        void Subscribe<T>() where T : class, ACE.IAction;

        /// <summary>
        /// Subscribe action from RabbitMQ in parallel.
        /// </summary>
        /// <param name="capacity">Worker numbers</param>
        void SubscribeInParallel<T>(int capacity) where T : class, ACE.IAction;
    }
}
