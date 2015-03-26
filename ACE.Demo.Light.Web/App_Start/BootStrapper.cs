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
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Light.Web
{
    public static class BootStrapper
    {
        public static Ninject.IKernel Container { get; private set; }
        public static void BootStrap()
        {
            Container = new StandardKernel();
            BindFrameworkObjects();
            BindBusinessObjects();
        }

        private static void BindFrameworkObjects()
        {
            Container.Bind<ACE.Loggers.IBusLogger>().To<ACE.Loggers.Log4NetBusLogger>();
            Container.Bind<ICommandHandlerFactory>().To<CommandHandlerFactory>()
                .InSingletonScope()
                .WithConstructorArgument("commandAssmblies", new string[] { "ACE.Demo.Contracts" })
                .WithConstructorArgument("handlerAssmblies", new string[] { "ACE.Demo.Model.Write" });
            Container.Bind<ICommandBus>().To<CommandBus>().InSingletonScope();

            Container.Bind<IEventHandlerFactory>().To<EventHandlerFactory>()
                .InSingletonScope()
                .WithConstructorArgument("eventAssmblies", new string[] { "ACE.Demo.Contracts" })
                .WithConstructorArgument("handlerAssmblies", new string[] { "ACE.Demo.Model.Write" });
            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            Container.Bind<IEventBus>().To<EventBus>().InThreadScope();

            Container.Bind<IActionHandlerFactory>().To<ActionHandlerFactory>()
                .InSingletonScope()
                .WithConstructorArgument("actionAssmblies", new string[] { "ACE.Demo.Contracts" })
                .WithConstructorArgument("handlerAssmblies", new string[] { "ACE.Demo.Application" });
            // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
            Container.Bind<IActionBus>().To<ActionBus>().InThreadScope();
        }

        private static void BindBusinessObjects()
        {
            Container.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope();
            Container.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            Container.Bind<IInvestmentRepository>().To<InvestmentRepository>().InSingletonScope();
            Container.Bind<IInvestmentWriteRepository>().To<InvestmentWriteRepository>().InSingletonScope();
            Container.Bind<IInvestmentService>().To<InvestmentService>().InSingletonScope();
            Container.Bind<IProjectRepository>().To<ProjectRepository>().InSingletonScope();
            Container.Bind<IProjectWriteRepository>().To<ProjectWriteRepository>().InSingletonScope();
            Container.Bind<IProjectService>().To<ProjectService>().InSingletonScope();
            Container.Bind<IAccountRepository>().To<AccountRepository>().InSingletonScope();
            Container.Bind<IAccountWriteRepository>().To<AccountWriteRepository>().InSingletonScope();
            Container.Bind<IAccountService>().To<AccountService>().InSingletonScope();
            Container.Bind<IMessageWriteRepository>().To<MessageWriteRepository>().InSingletonScope();
            Container.Bind<IAccountActivityWriteRepository>().To<AccountActivityWriteRepository>().InSingletonScope();
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
