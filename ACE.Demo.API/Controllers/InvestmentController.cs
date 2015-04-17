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
        static InvestmentController()
        {
            AutoMapper.Mapper.CreateMap<Investment, ACE.Demo.Contracts.Services.Investment>();
        }
        private IInvestmentService _investmentService;

        public InvestmentController(IInvestmentService investmentService)
        {
            _investmentService = investmentService;
        }

        [HttpGet]
        [HttpPost]
        public ACE.Demo.Contracts.Services.Investment Index(int id)
        {
            return AutoMapper.Mapper.Map<ACE.Demo.Contracts.Services.Investment>(_investmentService.Get(id));
        }

        [HttpGet]
        [HttpPost]
        public IEnumerable<ACE.Demo.Contracts.Services.Investment> List()
        {
            return AutoMapper.Mapper.Map<IEnumerable<ACE.Demo.Contracts.Services.Investment>>(_investmentService.GetAll());
        }
    }
}
