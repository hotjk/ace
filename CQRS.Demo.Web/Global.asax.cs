using CQRS.Demo.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CQRS.Demo.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            // Action a dummy method to ensoure Command/Event assembly been loaded
            EnsoureAssemblyLoaded.Pike();
            BootStrapper.BootStrap();
            DependencyResolver.SetResolver(new NinjectDependencyResolver { Kernel = BootStrapper.Kernel });

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Stop()
        {
            BootStrapper.Dispose();
        }
    }
}
