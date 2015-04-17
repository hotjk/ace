using ACE.Demo.Contracts.Services;
using ACE.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ACE.Demo.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IServiceBus serviceBus)
            : base(null, serviceBus)
        {
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<Investment> investments = 
                await ServiceBus.InvokeAsync<GetInvestmentsRequest, IEnumerable<Investment>>(new GetInvestmentsRequest());
            return View(investments);
        }
    }
}