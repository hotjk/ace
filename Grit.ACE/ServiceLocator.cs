using EasyNetQ;
using EasyNetQ.Loggers;
using ACE;
using ACE.Loggers;
using Ninject;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE
{
    public static class ServiceLocator
    {
        public static Ninject.IKernel NinjectContainer { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }
        public static ICommandBus CommandBus { get; private set; }
        public static IEventBus EventBus
        {
            get
            {
                return NinjectContainer.GetService(typeof(IEventBus)) as IEventBus;
            }
        }
        public static IActionBus ActionBus
        {
            get
            {
                return NinjectContainer.GetService(typeof(IActionBus)) as IActionBus;
            }
        }
        public static IBusLogger BusLogger { get; private set; }

        private static bool _isInitialized;
        private static readonly object _lockThis = new object();

        #region distribution options

        public static bool ActionShouldDistributeToExternalQueue { get; private set; }
        public static Event.EventDistributionOptions _eventDistributionOptions;
        
        public static bool EventShouldDistributeInCurrentThread
        {
            get
            {
                return (_eventDistributionOptions & Event.EventDistributionOptions.CurrentThread) == Event.EventDistributionOptions.CurrentThread;
            }
        }
        public static bool EventShouldDistributeInThreadPool
        {
            get
            {
                return (_eventDistributionOptions & Event.EventDistributionOptions.ThreadPool) == Event.EventDistributionOptions.ThreadPool;
            }
        }
        public static bool EventShouldDistributeToExternalQueue
        {
            get
            {
                return (_eventDistributionOptions & Event.EventDistributionOptions.Queue) == Event.EventDistributionOptions.Queue;
            }
        }

        #endregion

        public static void Init(
            IBusLogger logger,
            string queueConnectionString = null,
            bool actionShouldDistributeToExternalQueue = false,
            Event.EventDistributionOptions eventDistributionOptions = Event.EventDistributionOptions.BalckHole)
        {
            if (!_isInitialized)
            {
                lock (_lockThis)
                {
                    ActionShouldDistributeToExternalQueue = actionShouldDistributeToExternalQueue;
                    _eventDistributionOptions = eventDistributionOptions;

                    if (string.IsNullOrEmpty(queueConnectionString))
                    {
                        if (ActionShouldDistributeToExternalQueue || EventShouldDistributeToExternalQueue)
                        {
                            throw new Exception("Queue connection string is required when distribute action/event to queue.");
                        }
                    }

                    BusLogger = logger;
                    NinjectContainer = new StandardKernel();

                    if (!string.IsNullOrEmpty(queueConnectionString))
                    {
                        RabbitHutch.SetContainerFactory(() =>
                        {
                            return new EasyNetQ.DI.NinjectAdapter(NinjectContainer);
                        });
                        EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(queueConnectionString, x => x.Register<IEasyNetQLogger, NullLogger>());
                    }

                    NinjectContainer.Bind<ICommandHandlerFactory>().To<CommandHandlerFactory>().InSingletonScope();
                    NinjectContainer.Bind<ICommandBus>().To<CommandBus>().InSingletonScope();
                    
                    NinjectContainer.Bind<IEventHandlerFactory>().To<EventHandlerFactory>().InSingletonScope();
                    // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
                    NinjectContainer.Bind<IEventBus>().To<EventBus>().InThreadScope();
                    
                    NinjectContainer.Bind<IActionHandlerFactory>().To<ActionHandlerFactory>().InSingletonScope();
                    // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
                    NinjectContainer.Bind<IActionBus>().To<ActionBus>().InThreadScope();

                    CommandBus = NinjectContainer.Get<ICommandBus>();
                    _isInitialized = true;
                }
            }
        }

        public static void Dispose()
        {
            lock (_lockThis)
            {
                if (EasyNetQBus != null)
                {
                    EasyNetQBus.Dispose();
                }
                if (NinjectContainer != null)
                {
                    NinjectContainer.Dispose();
                }
            }
        }
    }
}
