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
            ServiceLocator.IoCKernel.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            ServiceLocator.IoCKernel.Bind<IInvestmentRepository>().To<InvestmentRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IInvestmentService>().To<InvestmentService>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IProjectRepository>().To<ProjectRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IProjectService>().To<ProjectService>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IAccountRepository>().To<AccountRepository>().InSingletonScope();
            ServiceLocator.IoCKernel.Bind<IAccountService>().To<AccountService>().InSingletonScope();
        }
    }
}
