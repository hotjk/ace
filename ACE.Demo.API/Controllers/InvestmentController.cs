using ACE.Demo.Model.Investments;
using ACE.Demo.Model.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ACE.Demo.API.Controllers
{
    public class InvestmentController : ApiController
    {
        private IInvestmentService _investmentService;

        public InvestmentController(IInvestmentService investmentService)
        {
            _investmentService = investmentService;
        }

        [HttpGet]
        [HttpPost]
        public Investment Index(int id)
        {
            return _investmentService.Get(id);
        }

        [HttpGet]
        [HttpPost]
        public IEnumerable<Investment> List()
        {
            return _investmentService.GetAll();
        }
    }
}
