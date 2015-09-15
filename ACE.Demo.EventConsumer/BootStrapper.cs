using ACE.Demo.Model;
using ACE.Demo.Model.Accounts;
using ACE.Demo.Model.Investments;
using ACE.Demo.Model.Projects;
using ACE.Demo.Model.Write.AccountActivities;
using ACE.Demo.Model.Write.Messages;
using ACE.Demo.Repositories;
using ACE.Demo.Repositories.Write;
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
using Autofac;

namespace ACE.Demo.EventConsumer
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
                x => x.Register<IEasyNetQLogger, NullLogger>());

            _builder = new ContainerBuilder();
            BindFrameworkObjects();
            BindBusinessObjects();
            _builder.Update(Container);

            EventStation = Container.Resolve<IEventStation>();
        }

        private static void BindFrameworkObjects()
        {
            //Container.Settings.AllowNullInjection = true;
            _builder.RegisterType<ACE.Loggers.Log4NetBusLogger>().As<ACE.Loggers.IBusLogger>().SingleInstance();
            _builder.RegisterType<CommandHandlerFactory>().As<ICommandHandlerFactory>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Autofac.IContainer), Container))
                .WithParameter(Constants.ParamCommandAssmblies, new string[] { "ACE.Demo.ContractsFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "ACE.Demo.Model.Write" });
            _builder.RegisterType<CommandBus>().As<ICommandBus>().SingleInstance();

            _builder.RegisterType<EventHandlerFactory>().As<IEventHandlerFactory>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Autofac.IContainer), Container))
                .WithParameter(Constants.ParamEventAssmblies, new string[] { "ACE.Demo.ContractsFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "ACE.Demo.Model.Write" });
            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            _builder.RegisterType<EventStation>().As<IEventStation>().SingleInstance();
        }

        private static void BindBusinessObjects()
        {
            _builder.RegisterType<SequenceRepository>().As<ISequenceRepository>().SingleInstance();
            _builder.RegisterType<SequenceService>().As<ISequenceService>().SingleInstance();

            _builder.RegisterType<InvestmentRepository>().As<IInvestmentRepository>().SingleInstance();
            _builder.RegisterType<InvestmentWriteRepository>().As<IInvestmentWriteRepository>().SingleInstance();
            _builder.RegisterType<InvestmentService>().As<IInvestmentService>().SingleInstance();
            _builder.RegisterType<ProjectRepository>().As<IProjectRepository>().SingleInstance();
            _builder.RegisterType<ProjectWriteRepository>().As<IProjectWriteRepository>().SingleInstance();
            _builder.RegisterType<ProjectService>().As<IProjectService>().SingleInstance();
            _builder.RegisterType<AccountRepository>().As<IAccountRepository>().SingleInstance();
            _builder.RegisterType<AccountWriteRepository>().As<IAccountWriteRepository>().SingleInstance();
            _builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            _builder.RegisterType<MessageWriteRepository>().As<IMessageWriteRepository>().SingleInstance();
            _builder.RegisterType<AccountActivityWriteRepository>().As<IAccountActivityWriteRepository>().SingleInstance();
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
