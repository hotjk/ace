using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
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
        /// Direct handle a event in current thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        void Handle<T>(T @event) where T : Event;

        /// <summary>
        /// Clear all cached events in thread.
        /// </summary>
        void Purge();
        Type GetType(string name);
    }
}
