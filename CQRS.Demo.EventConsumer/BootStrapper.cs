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
            ServiceLocator.Init(new Grit.CQRS.Loggers.Log4NetBusLogger(),
                Grit.Configuration.RabbitMQ.CQRSQueueConnectionString,
                false, false, false);

            AddIocBindings();
            InitHandlerFactory();
        }

        private static void AddIocBindings()
        {
            ServiceLocator.NinjectContainer.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            ServiceLocator.NinjectContainer.Bind<IInvestmentRepository>().To<InvestmentRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IInvestmentWriteRepository>().To<InvestmentWriteRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IInvestmentService>().To<InvestmentService>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IProjectRepository>().To<ProjectRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IProjectWriteRepository>().To<ProjectWriteRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IProjectService>().To<ProjectService>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IAccountRepository>().To<AccountRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IAccountWriteRepository>().To<AccountWriteRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IAccountService>().To<AccountService>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IMessageWriteRepository>().To<MessageWriteRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IAccountActivityWriteRepository>().To<AccountActivityWriteRepository>().InSingletonScope();
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
