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

namespace CQRS.Demo.EventConsumer
{
    public static class BootStrapper
    {
        public static void BootStrap()
        {
            // Pike a dummy method to ensoure Command/Event assembly been loaded
            CQRS.Demo.Contracts.EnsoureAssemblyLoaded.Pike();
            ServiceLocator.Init(Grit.Configuration.RabbitMQ.CQRSQueueConnectionString);

            AddIocBindings();
            InitHandlerFactory();
        }

        private static void AddIocBindings()
        {
            ServiceLocator.IoCKernel.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            ServiceLocator.IoCKernel.Bind<IInvestmentRepository>().To<InvestmentRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IInvestmentWriteRepository>().To<InvestmentWriteRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IInvestmentService>().To<InvestmentService>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IProjectRepository>().To<ProjectRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IProjectWriteRepository>().To<ProjectWriteRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IProjectService>().To<ProjectService>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IAccountRepository>().To<AccountRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IAccountWriteRepository>().To<AccountWriteRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IAccountService>().To<AccountService>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IMessageWriteRepository>().To<MessageWriteRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IAccountActivityWriteRepository>().To<AccountActivityWriteRepository>().InSingletonScope();
        }

        private static void InitHandlerFactory()
        {
            CommandHandlerFactory.Init(new string[] { "CQRS.Demo.Contracts" },
                new string[] { "CQRS.Demo.Model.Write" });
            EventHandlerFactory.Init(new string[] { "CQRS.Demo.Contracts" },
                new string[] { "CQRS.Demo.Model.Write" });
        }
    }
}
