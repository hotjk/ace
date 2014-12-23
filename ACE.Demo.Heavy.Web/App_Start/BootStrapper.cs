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

namespace ACE.Demo.Heavy.Web
{
    public static class BootStrapper
    {
        public static void BootStrap()
        {
            ServiceLocator.Init(new ACE.Loggers.Log4NetBusLogger(),
                Grit.Configuration.RabbitMQ.ACEQueueConnectionString, true,
                ACE.Event.EventDistributionOptions.Queue);
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
