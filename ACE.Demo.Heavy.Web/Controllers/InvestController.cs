using ACE.Demo.Contracts;
using ACE.Demo.Contracts.Actions;
using ACE.Demo.Contracts.Commands;
using ACE.Demo.Contracts.Events;
using ACE.Demo.Model.Investments;
using ACE.Demo.Web.Models;
using ACE;
using ACE.Actions;
using ACE.Exceptions;
using Grit.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ACE.Demo.Web.Controllers
{
    public class InvestController : Controller
    {
        public InvestController(ISequenceService sequenceService,
            IInvestmentService investmentService)
        {
            _sequenceService = sequenceService;
            _investmentService = investmentService;
        }

        private ISequenceService _sequenceService;
        private IInvestmentService _investmentService;

        [HttpGet]
        public ActionResult Create()
        {
            return View(new InvestViewModel
            {
                ProjectId = 1,
                AccountId = 2,
                Amount = 1,
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(InvestViewModel vm)
        {
            var action = new InvestmentCreateRequest
            {
                InvestmentId = _sequenceService.Next(SequenceID.ACE_Investment, 1),
                AccountId = vm.AccountId,
                ProjectId = vm.ProjectId,
                Amount = vm.Amount
            };

            var response = await ServiceLocator.ActionBus.SendAsync<InvestmentActionBase, InvestmentCreateRequest>(action);
            //var response = ServiceLocator.ActionBus.Send(action);
            TempData["ActionResponse"] = response;
            return RedirectToAction("Index", new { id = action.InvestmentId });
        }

        public ActionResult Index(int id)
        {
            var investment = _investmentService.Get(id);
            return View(investment);
        }

        public async Task<ActionResult> Pay(int id)
        {
            var action = new InvestmentPayRequest
            {
                InvestmentId = id
            };

            ActionResponse response = await ServiceLocator.ActionBus.SendAsync<InvestmentActionBase, InvestmentPayRequest>(action);
            TempData["ActionResponse"] = response;
            return RedirectToAction("Index", new { id = action.InvestmentId });
        }
    }
}