using CQRS.Demo.Model.Investments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQRS.Demo.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(
            IInvestmentService investmentService)
        {
            _investmentService = investmentService;
        }

        private IInvestmentService _investmentService;
        public ActionResult Index()
        {
            var investments = _investmentService.GetAll();
            return View(investments);
        }
    }
}