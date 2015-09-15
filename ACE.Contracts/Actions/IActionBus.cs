using System.Threading.Tasks;

namespace ACE
{
    /// <summary>
    /// ActionBus instance MUST be thread scope, since each thread will keep an anonymous RabbitMQ reply queue.
    /// </summary>
    public interface IActionBus
    {
        /// <summary>
        /// Send to RabbitMQ and waiting for response.
        /// </summary>
        /// <typeparam name="B">The base class of request action class, base class will be the queue name.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        ActionResponse Send<B, T>(T action)
            where B : class, IAction
            where T : B;

        /// <summary>
        /// Send to RabbitMQ and waiting for response in async.
        /// </summary>
        /// <typeparam name="B">The base class of request action class, base class will be the queue name.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<ActionResponse> SendAsync<B, T>(T action)
            where B : class, IAction
            where T : B;

        /// <summary>
        /// Send to RabbitMQ and waiting for response in async with retry.
        /// </summary>
        /// <typeparam name="B">The base class of request action class, base class will be the queue name.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        Task<ActionResponse> SendAsyncWithRetry<B, T>(T action, int retryCount = 2)
            where B : class, IAction
            where T : B;
    }
}
