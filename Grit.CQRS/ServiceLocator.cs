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
        public static Ninject.IKernel IoCKernel { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }
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

        private static bool _isInitialized;
        private static readonly object _lockThis = new object();

        public static void Init(string queueConnectionString)
        {
            if (!_isInitialized)
            {
                lock (_lockThis)
                {
                    IoCKernel = new StandardKernel();

                    RabbitHutch.SetContainerFactory(() =>
                    {
                        return new EasyNetQ.DI.NinjectAdapter(IoCKernel);
                    });

                    EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(queueConnectionString, x => x.Register<IEasyNetQLogger, NullLogger>());

                    IoCKernel.Bind<ICommandHandlerFactory>().To<CommandHandlerFactory>().InSingletonScope();
                    IoCKernel.Bind<ICommandBus>().To<CommandBus>().InSingletonScope();
                    
                    IoCKernel.Bind<IEventHandlerFactory>().To<EventHandlerFactory>().InSingletonScope();
                    // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
                    IoCKernel.Bind<IEventBus>().To<EventBus>().InThreadScope();
                    
                    IoCKernel.Bind<IActionHandlerFactory>().To<ActionHandlerFactory>().InSingletonScope();
                    // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
                    IoCKernel.Bind<IActionBus>().To<ActionBus>().InSingletonScope();

                    CommandBus = IoCKernel.Get<ICommandBus>();
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
                if (IoCKernel != null)
                {
                    IoCKernel.Dispose();
                }
            }
        }
    }
}
