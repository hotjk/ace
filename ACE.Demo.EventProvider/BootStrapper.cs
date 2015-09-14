using ACE;
using EasyNetQ;
using EasyNetQ.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace ACE.Demo.EventConsumer
{
    public static class BootStrapper
    {
        public static Autofac.IContainer Container { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }
        public static IEventBus EventBus { get; private set; }

        private static ContainerBuilder _builder;

        public static void BootStrap()
        {
            _builder = new ContainerBuilder();
            Container = _builder.Build();

            RabbitHutch.SetContainerFactory(() => { return new EasyNetQ.DI.AutofacAdapter(_builder); });
            EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(Grit.Configuration.RabbitMQ.ACEQueueConnectionString,
                x => x.Register<IEasyNetQLogger, NullLogger>());
            
            BindFrameworkObjects();

            EventBus = Container.Resolve<IEventBus>();
        }

        private static void BindFrameworkObjects()
        {
            //Container.Settings.AllowNullInjection = true;
            _builder.RegisterType<ACE.Loggers.Log4NetBusLogger>().As<ACE.Loggers.IBusLogger>().SingleInstance();

            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            _builder.RegisterType<EventBus>().As<IEventBus>().InstancePerRequest();
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
