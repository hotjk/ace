using ACE;
using Autofac;
using EasyNetQ;
using EasyNetQ.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie
{
    public static class BootStrapper
    {
        public static Autofac.IContainer Container { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }
        public static IEventStation EventStation { get; private set; }

        private static ContainerBuilder _builder;

        public static void BootStrap()
        {
            var adapter = new EasyNetQ.DI.AutofacAdapter(new ContainerBuilder());
            Container = adapter.Container;

            RabbitHutch.SetContainerFactory(() => { return adapter; });
            EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(Grit.Configuration.RabbitMQ.ACEQueueConnectionString,
                x => {
                    x.Register<IEasyNetQLogger, NullLogger>();
                });

            _builder = new ContainerBuilder();
            BindFrameworkObjects();
            _builder.Update(Container);

            EventStation = Container.Resolve<IEventStation>();
        }

        private static void BindFrameworkObjects()
        {
            _builder.RegisterType<ACE.Loggers.NullBusLogger>().As<ACE.Loggers.IBusLogger>().SingleInstance();

            _builder.RegisterType<EventStation>().As<IEventStation>().SingleInstance();
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
