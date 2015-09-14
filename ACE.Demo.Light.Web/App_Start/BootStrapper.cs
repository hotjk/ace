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
using Autofac;

namespace ACE.Demo.Light.Web
{
    public static class BootStrapper
    {
        public static Autofac.IContainer Container { get; private set; }

        private static ContainerBuilder _builder;

        public static void BootStrap()
        {
            _builder = new ContainerBuilder();
            Container = _builder.Build();
            BindFrameworkObjects();
            BindBusinessObjects();
        }

        private static void BindFrameworkObjects()
        {
            //Container.Settings.AllowNullInjection = true;
            _builder.RegisterType<ACE.Loggers.IBusLogger>().As<ACE.Loggers.Log4NetBusLogger>().SingleInstance();
            _builder.RegisterType<ICommandHandlerFactory>().As<CommandHandlerFactory>()
                .SingleInstance()
                .WithParameter(Constants.ParamCommandAssmblies, new string[] { "ACE.Demo.ContractsFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "ACE.Demo.Model.Write" });
            _builder.RegisterType<ICommandBus>().As<CommandBus>().SingleInstance();

            _builder.RegisterType<IEventHandlerFactory>().As<EventHandlerFactory>()
                .SingleInstance()
                .WithParameter(Constants.ParamEventAssmblies, new string[] { "ACE.Demo.ContractsFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "ACE.Demo.Model.Write" });
            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            _builder.RegisterType<IEventBus>().As<EventBus>().InstancePerRequest();

            _builder.RegisterType<IActionHandlerFactory>().As<ActionHandlerFactory>()
                .SingleInstance()
                .WithParameter(Constants.ParamActionAssmblies, new string[] { "ACE.Demo.ContractsFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "ACE.Demo.Application" });
            // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
            _builder.RegisterType<IActionBus>().As<ActionBus>().SingleInstance();
        }

        private static void BindBusinessObjects()
        {
            _builder.RegisterType<ISequenceRepository>().As<SequenceRepository>().SingleInstance();
            _builder.RegisterType<ISequenceService>().As<SequenceService>().SingleInstance();

            _builder.RegisterType<IInvestmentRepository>().As<InvestmentRepository>().SingleInstance();
            _builder.RegisterType<IInvestmentWriteRepository>().As<InvestmentWriteRepository>().SingleInstance();
            _builder.RegisterType<IInvestmentService>().As<InvestmentService>().SingleInstance();
            _builder.RegisterType<IProjectRepository>().As<ProjectRepository>().SingleInstance();
            _builder.RegisterType<IProjectWriteRepository>().As<ProjectWriteRepository>().SingleInstance();
            _builder.RegisterType<IProjectService>().As<ProjectService>().SingleInstance();
            _builder.RegisterType<IAccountRepository>().As<AccountRepository>().SingleInstance();
            _builder.RegisterType<IAccountWriteRepository>().As<AccountWriteRepository>().SingleInstance();
            _builder.RegisterType<IAccountService>().As<AccountService>().SingleInstance();
            _builder.RegisterType<IMessageWriteRepository>().As<MessageWriteRepository>().SingleInstance();
            _builder.RegisterType<IAccountActivityWriteRepository>().As<AccountActivityWriteRepository>().SingleInstance();
        }

        public static void Dispose()
        {
            if (Container != null)
            {
                Container.Dispose();
            }
        }
    }
}
