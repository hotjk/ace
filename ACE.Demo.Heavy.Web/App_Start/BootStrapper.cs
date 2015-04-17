using ACE.Demo.Model;
using ACE.Demo.Model.Accounts;
using ACE.Demo.Model.Investments;
using ACE.Demo.Model.Projects;
using ACE.Demo.Repositories;
using ACE;
using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Loggers;
using ACE.WS;

namespace ACE.Demo.Heavy.Web
{
    public static class BootStrapper
    {
        public static Ninject.IKernel Container { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }
        public static void BootStrap()
        {
            Container = new StandardKernel();

            RabbitHutch.SetContainerFactory(() => { return new EasyNetQ.DI.NinjectAdapter(Container); });
            EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(Grit.Configuration.RabbitMQ.ACEQueueConnectionString,
                x => x.Register<IEasyNetQLogger, NullLogger>());

            BindFrameworkObjects();
            BindBusinessObjects();
        }

        private static void BindFrameworkObjects()
        {
            Container.Settings.AllowNullInjection = true;

            Container.Bind<ACE.Loggers.IBusLogger>().To<ACE.Loggers.Log4NetBusLogger>().InSingletonScope();

            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            Container.Bind<IEventBus>().To<EventBus>()
                .InThreadScope()
                .WithConstructorArgument("eventDistributionOptions", ACE.Event.EventDistributionOptions.Queue);

            // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
            Container.Bind<IActionBus>().To<ActionBus>()
                .InThreadScope()
                .WithConstructorArgument("actionShouldDistributeToExternalQueue", true);

            IServiceMappingFactory serviceMappingFactory = new ServiceMappingFactory(() => {
                return new Dictionary<Type, ServiceMapping>() {
                     { typeof(ACE.Demo.Contracts.Services.GetInvestmentsRequest), new ServiceMapping("http://localhost:59857", "api/investment/list") },
                     { typeof(ACE.Demo.Contracts.Services.GetInvestmentRequest), new ServiceMapping("http://localhost:59857", "api/investment") }
                };
            });
            Container.Bind<IServiceMappingFactory>().ToConstant(serviceMappingFactory);
            Container.Bind<IServiceBus>().To<ServiceBus>().InSingletonScope();
        }

        private static void BindBusinessObjects()
        {
            Container.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope();
            Container.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            Container.Bind<IInvestmentRepository>().To<InvestmentRepository>().InSingletonScope();
            Container.Bind<IInvestmentService>().To<InvestmentService>().InSingletonScope();
            Container.Bind<IProjectRepository>().To<ProjectRepository>().InSingletonScope();
            Container.Bind<IProjectService>().To<ProjectService>().InSingletonScope();
            Container.Bind<IAccountRepository>().To<AccountRepository>().InSingletonScope();
            Container.Bind<IAccountService>().To<AccountService>().InSingletonScope();
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
