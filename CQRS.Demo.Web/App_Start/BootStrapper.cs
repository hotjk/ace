using CQRS.Demo.Model;
using CQRS.Demo.Model.Accounts;
using CQRS.Demo.Model.Investments;
using CQRS.Demo.Model.Projects;
using CQRS.Demo.Repositories;
using Grit.CQRS;
using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Web
{
    public static class BootStrapper
    {
        public static void BootStrap()
        {
            // Action a dummy method to ensoure Command/Event assembly been loaded
            CQRS.Demo.Contracts.EnsoureAssemblyLoaded.Pike();

            ServiceLocator.Init(Grit.Configuration.RabbitMQ.CQRSQueueConnectionString);
            AddIoCBindings();
        }

        private static void AddIoCBindings()
        {
            ServiceLocator.NinjectContainer.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            ServiceLocator.NinjectContainer.Bind<IInvestmentRepository>().To<InvestmentRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IInvestmentService>().To<InvestmentService>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IProjectRepository>().To<ProjectRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IProjectService>().To<ProjectService>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IAccountRepository>().To<AccountRepository>().InSingletonScope();
            ServiceLocator.NinjectContainer.Bind<IAccountService>().To<AccountService>().InSingletonScope();
        }
    }
}
