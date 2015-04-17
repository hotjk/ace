using ACE.Demo.Contracts;
using ACE.Demo.Contracts.Actions;
using ACE.Demo.Contracts.Commands;
using ACE.Demo.Contracts.Events;
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
using ACE.WS;
using ACE.Demo.Contracts.Services;

namespace ACE.Demo.Web.Controllers
{
    public class InvestController : ControllerBase
    {
        public InvestController(IActionBus actionBus, IServiceBus serviceBus, 
            ISequenceService sequenceService)
            : base(actionBus, serviceBus)
        {
            _sequenceService = sequenceService;
        }

        private ISequenceService _sequenceService;

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

            var response = await ActionBus.SendAsyncWithRetry<InvestmentActionBase, InvestmentCreateRequest>(action, 3);
            TempData["ActionResponse"] = response;
            return RedirectToAction("Index", new { id = action.InvestmentId });
        }

        public async Task<ActionResult> Index(int id)
        {
            var investment = await ServiceBus.InvokeAsync<GetInvestmentRequest, Investment>(new GetInvestmentRequest { Id = id });
            return View(investment);
        }

        public async Task<ActionResult> Pay(int id)
        {
            var action = new InvestmentPayRequest
            {
                InvestmentId = id
            };

            ActionResponse response = await ActionBus.SendAsync<InvestmentActionBase, InvestmentPayRequest>(action);
            TempData["ActionResponse"] = response;
            return RedirectToAction("Index", new { id = action.InvestmentId });
        }
    }
}