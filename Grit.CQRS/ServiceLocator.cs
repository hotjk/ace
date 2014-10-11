using EasyNetQ;
using EasyNetQ.Loggers;
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

        private static bool _isInitialized;
        private static readonly object _lockThis = new object();

        public static log4net.ILog CommandLogger { get; private set; }
        public static log4net.ILog EventLogger { get; private set; }
        public static log4net.ILog ActionLogger { get; private set; }
        public static log4net.ILog ExceptionLogger { get; private set; }

        public static void Init(string queueConnectionString)
        {
            if (!_isInitialized)
            {
                lock (_lockThis)
                {
                    CommandLogger = log4net.LogManager.GetLogger("command.logger");
                    EventLogger = log4net.LogManager.GetLogger("event.logger");
                    ActionLogger = log4net.LogManager.GetLogger("action.logger");
                    ExceptionLogger = log4net.LogManager.GetLogger("exception.logger");

                    NinjectContainer = new StandardKernel();

                    RabbitHutch.SetContainerFactory(() =>
                    {
                        return new EasyNetQ.DI.NinjectAdapter(NinjectContainer);
                    });

                    EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(queueConnectionString, x => x.Register<IEasyNetQLogger, NullLogger>());

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
