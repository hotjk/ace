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
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace ACE.Demo.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterType<IProjectRepository>().As<ProjectRepository>().SingleInstance();
            builder.RegisterType<IProjectService>().As<ProjectService>().SingleInstance();
            builder.RegisterType<IInvestmentRepository>().As<InvestmentRepository>().SingleInstance();
            builder.RegisterType<IInvestmentService>().As<InvestmentService>().SingleInstance();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
