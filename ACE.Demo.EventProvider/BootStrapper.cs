using ACE;
using EasyNetQ;
using EasyNetQ.Loggers;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.EventConsumer
{
    public static class BootStrapper
    {
        public static Ninject.IKernel Container { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }
        public static IEventBus EventBus { get; private set; }
        public static void BootStrap()
        {
            Container = new StandardKernel();
            

            Container.Bind<ACE.Loggers.IBusLogger>().To<ACE.Loggers.Log4NetBusLogger>().InSingletonScope();
            RabbitHutch.SetContainerFactory(() => { return new EasyNetQ.DI.NinjectAdapter(Container); });
            EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(Grit.Configuration.RabbitMQ.ACEQueueConnectionString,
                x => x.Register<IEasyNetQLogger, NullLogger>());
            
            BindFrameworkObjects();

            EventBus = Container.GetService(typeof(IEventBus)) as IEventBus;
        }

        private static void BindFrameworkObjects()
        {
            Container.Settings.AllowNullInjection = true;

            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            Container.Bind<IEventBus>().To<ACE.EventBus>()
                .InThreadScope();
        }

        public static void Dispose()
        {
            if (EasyNetQBus != null)
            {
                EasyNetQBus.Dispose();
            }
            if (Container != null)
            {
                Container.Dispose();
            }
        }
    }
}
