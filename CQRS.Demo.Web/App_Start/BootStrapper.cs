using CQRS.Demo.Model;
using CQRS.Demo.Model.Accounts;
using CQRS.Demo.Model.Investments;
using CQRS.Demo.Model.Projects;
using CQRS.Demo.Repositories;
using Grit.CQRS;
using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using Ninject;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Web
{
    public static class BootStrapper
    {
        public static IKernel IoCKernel { get; private set; }

        public static void BootStrap()
        {
            AddIoCBindings();
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

        private static void AddIoCBindings()
        {
            IoCKernel = new StandardKernel();

            IoCKernel.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope();
            IoCKernel.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            IoCKernel.Bind<IInvestmentRepository>().To<InvestmentRepository>().InSingletonScope();
            IoCKernel.Bind<IInvestmentService>().To<InvestmentService>().InSingletonScope();
            IoCKernel.Bind<IProjectRepository>().To<ProjectRepository>().InSingletonScope();
            IoCKernel.Bind<IProjectService>().To<ProjectService>().InSingletonScope();
            IoCKernel.Bind<IAccountRepository>().To<AccountRepository>().InSingletonScope();
            IoCKernel.Bind<IAccountService>().To<AccountService>().InSingletonScope();
        }

        private static void InitServiceLocator()
        {
            ServiceLocator.Init(
                IoCKernel,
                Grit.Configuration.RabbitMQ.CQRSQueueConnectionString);
        }
    }
}
