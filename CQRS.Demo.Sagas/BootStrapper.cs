using CQRS.Demo.Model;
using CQRS.Demo.Model.Accounts;
using CQRS.Demo.Model.Investments;
using CQRS.Demo.Model.Projects;
using CQRS.Demo.Model.Write.AccountActivities;
using CQRS.Demo.Model.Write.Messages;
using CQRS.Demo.Repositories;
using CQRS.Demo.Repositories.Write;
using Grit.CQRS;
using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Sagas
{
    public static class BootStrapper
    {
        public static IKernel IoCKernel { get; private set; }

        public static void BootStrap()
        {
            AddIocBindings();
            InitHandlerFactory();
            InitServiceLocator();
        }

        public static void Dispose()
        {
            ServiceLocator.Dispose();
            if (IoCKernel != null)
            {
                IoCKernel.Dispose();
            }
        }

        private static void AddIocBindings()
        {
            IoCKernel = new StandardKernel();

            IoCKernel.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope();
            IoCKernel.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            IoCKernel.Bind<IInvestmentRepository>().To<InvestmentRepository>().InSingletonScope();
            IoCKernel.Bind<IInvestmentWriteRepository>().To<InvestmentWriteRepository>().InSingletonScope();
            IoCKernel.Bind<IInvestmentService>().To<InvestmentService>().InSingletonScope();
            IoCKernel.Bind<IProjectRepository>().To<ProjectRepository>().InSingletonScope();
            IoCKernel.Bind<IProjectWriteRepository>().To<ProjectWriteRepository>().InSingletonScope();
            IoCKernel.Bind<IProjectService>().To<ProjectService>().InSingletonScope();
            IoCKernel.Bind<IAccountRepository>().To<AccountRepository>().InSingletonScope();
            IoCKernel.Bind<IAccountWriteRepository>().To<AccountWriteRepository>().InSingletonScope();
            IoCKernel.Bind<IAccountService>().To<AccountService>().InSingletonScope();
            IoCKernel.Bind<IMessageWriteRepository>().To<MessageWriteRepository>().InSingletonScope();
            IoCKernel.Bind<IAccountActivityWriteRepository>().To<AccountActivityWriteRepository>().InSingletonScope();
        }

        private static void InitHandlerFactory()
        {
            CommandHandlerFactory.Init(new string[] { "CQRS.Demo.Contracts" },
                new string[] { "CQRS.Demo.Model.Write" });
            EventHandlerFactory.Init(new string[] { "CQRS.Demo.Contracts" },
                new string[] { "CQRS.Demo.Model.Write" } );
            ActionHandlerFactory.Init(new string[] { "CQRS.Demo.Contracts" },
                new string[] { "CQRS.Demo.Applications" });
        }

        private static void InitServiceLocator()
        {
            ServiceLocator.Init(
                IoCKernel,
                Grit.Configuration.RabbitMQ.CQRSQueueConnectionString);
        }
    }
}
