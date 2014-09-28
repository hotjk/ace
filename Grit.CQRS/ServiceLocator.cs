using Grit.CQRS;
using Ninject;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public static class ServiceLocator
    {
        public static IKernel IoCKernel { get; private set; }
        public static IModel MQChannel { get; private set; }
        public static ICommandBus CommandBus { get; private set; }
        public static IEventBus EventBus
        {
            get
            {
                return IoCKernel.GetService(typeof(IEventBus)) as IEventBus;
            }
        }

        public static IActionBus ActionBus
        {
            get
            {
                return IoCKernel.GetService(typeof(IActionBus)) as IActionBus;
            }
        }

        public static string EventBusExchange { get; private set; }
        public static string ActionBusExchange { get; private set; }
        public static string ActionBusQueue { get; private set; }
        public static int ActionResponseTimeoutSeconds { get; private set; }

        private static bool _isInitialized;
        private static readonly object _lockThis = new object();

        private static System.Func<IModel> _resetMQCallback = null;

        public static void Init(IKernel kernel, 
            string eventBusExchange, 
            string actionBusExchange,
            string actionBusQueue, 
            int actionResponseTimeoutSeconds = 10,
            System.Func<IModel> resetMQCallback = null)
        {
            if (!_isInitialized)
            {
                lock (_lockThis)
                {
                    _resetMQCallback = resetMQCallback;
                    IoCKernel = kernel;

                    ResetMQ();

                    EventBusExchange = eventBusExchange;
                    ActionBusExchange = actionBusExchange;
                    ActionBusQueue = actionBusQueue;
                    ActionResponseTimeoutSeconds = actionResponseTimeoutSeconds;

                    IoCKernel.Bind<ICommandHandlerFactory>().To<CommandHandlerFactory>().InSingletonScope();
                    IoCKernel.Bind<ICommandBus>().To<CommandBus>().InSingletonScope();
                    
                    IoCKernel.Bind<IEventHandlerFactory>().To<EventHandlerFactory>().InSingletonScope();
                    // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
                    IoCKernel.Bind<IEventBus>().To<EventBus>().InThreadScope();
                    
                    IoCKernel.Bind<IActionHandlerFactory>().To<ActionHandlerFactory>().InSingletonScope();
                    // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
                    IoCKernel.Bind<IActionBus>().To<ActionBus>().InThreadScope();

                    CommandBus = kernel.Get<ICommandBus>();
                    _isInitialized = true;
                }
            }
        }

        public static void ResetMQ()
        {
            if (_resetMQCallback != null)
            {
                MQChannel = _resetMQCallback();
            }
        }
    }
}
