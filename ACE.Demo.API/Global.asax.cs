using ACE.Demo.Model.Investments;
using ACE.Demo.Model.Projects;
using ACE.Demo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ACE.Demo.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            NinjectDependencyResolver ninject = new NinjectDependencyResolver { Kernel = new Ninject.StandardKernel() };
            DependencyResolver.SetResolver(ninject);
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver4API(ninject.Kernel);

            ninject.Kernel.Bind<IProjectRepository>().To<ProjectRepository>().InSingletonScope();
            ninject.Kernel.Bind<IProjectService>().To<ProjectService>().InSingletonScope();
            ninject.Kernel.Bind<IInvestmentRepository>().To<InvestmentRepository>().InSingletonScope();
            ninject.Kernel.Bind<IInvestmentService>().To<InvestmentService>().InSingletonScope();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
