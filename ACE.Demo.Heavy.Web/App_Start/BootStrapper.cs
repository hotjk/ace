using ACE.Demo.Repositories;
using ACE;
using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Loggers;
using ACE.WS;
using Autofac;

namespace ACE.Demo.Heavy.Web
{
    public static class BootStrapper
    {
        public static Autofac.IContainer Container { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }

        private static ContainerBuilder _builder;

        public static void BootStrap()
        {
            var adapter = new EasyNetQ.DI.AutofacAdapter(new ContainerBuilder());
            Container = adapter.Container;

            RabbitHutch.SetContainerFactory(() => { return adapter; });
            EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(Grit.Configuration.RabbitMQ.ACEQueueConnectionString,
                x => x.Register<IEasyNetQLogger, NullLogger>());

            _builder = new ContainerBuilder();
            BindFrameworkObjects();
            BindBusinessObjects();
            _builder.Update(Container);
        }

        private static void BindFrameworkObjects()
        {
            _builder.RegisterType<ACE.Loggers.Log4NetBusLogger>().As<ACE.Loggers.IBusLogger>().SingleInstance();

            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            _builder.RegisterType<EventBus>().As<IEventBus>().SingleInstance();

            // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
            _builder.RegisterType<ActionBus>().As<IActionBus>().InstancePerDependency();

            IServiceMappingFactory serviceMappingFactory = new ServiceMappingFactory(() => {
                return new Dictionary<Type, ServiceMapping>() {
                     { typeof(ACE.Demo.Contracts.Services.GetInvestmentsRequest), new ServiceMapping("http://localhost:59857", "api/investment/list") },
                     { typeof(ACE.Demo.Contracts.Services.GetInvestmentRequest), new ServiceMapping("http://localhost:59857", "api/investment") }
                };
            });
            _builder.RegisterInstance(serviceMappingFactory).As<IServiceMappingFactory>();
            _builder.RegisterType<ServiceBus>().As<IServiceBus>().SingleInstance();
        }

        private static void BindBusinessObjects()
        {
            _builder.RegisterType<SequenceRepository>().As<ISequenceRepository>().SingleInstance();
            _builder.RegisterType<SequenceService>().As<ISequenceService>().SingleInstance();
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
