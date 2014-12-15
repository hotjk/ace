using ACE.Demo.Model;
using ACE.Demo.Model.Accounts;
using ACE.Demo.Model.Investments;
using ACE.Demo.Model.Projects;
using ACE.Demo.Model.Write.AccountActivities;
using ACE.Demo.Model.Write.Messages;
using ACE.Demo.Repositories;
using ACE.Demo.Repositories.Write;
using Grit.ACE;
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
        public static void BootStrap()
        {
            ServiceLocator.Init(new Grit.ACE.Loggers.NullBusLogger(), null, false, true, false);
            AddIoCBindings();
            InitHandlerFactory();
        }

        private static void AddIoCBindings()
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
            CommandHandlerFactory.Init(new string[] { "ACE.Demo.Contracts" },
                new string[] { "ACE.Demo.Model.Write" });
            EventHandlerFactory.Init(new string[] { "ACE.Demo.Contracts" },
                new string[] { "ACE.Demo.Model.Write" });
            ActionHandlerFactory.Init(new string[] { "ACE.Demo.Contracts" },
                new string[] { "ACE.Demo.Application" });
        }
    }
}
