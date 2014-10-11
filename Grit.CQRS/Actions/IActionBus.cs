using Grit.CQRS.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    /// <summary>
    /// ActionBus instance MUST be thread scope, since each thread will keep an anonymous RabbitMQ reply queue.
    /// </summary>
    public interface IActionBus
    {
        /// <summary>
        /// Invoke action handler method in current thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        void Invoke<T>(T action) where T : Action;

        /// <summary>
        /// Send to RabbitMQ and waiting for response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        ActionResponse Send<T>(T action) where T : Action;

        /// <summary>
        /// Send to RabbitMQ and waiting for response asyn.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<ActionResponse> SendAsync<T>(T action) where T : Action;

        /// <summary>
        /// Handle action from RabbitMQ.
        /// </summary>
        void Subscribe();
        /// <summary>
        /// Handle action from RabbitMQ in parallel.
        /// </summary>
        /// <param name="capacity">Worker numbers</param>
        void SubscribeInParallel(int capacity);
    }
}
