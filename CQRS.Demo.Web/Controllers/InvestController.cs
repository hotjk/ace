using CQRS.Demo.Contracts;
using CQRS.Demo.Contracts.Actions;
using CQRS.Demo.Contracts.Commands;
using CQRS.Demo.Contracts.Events;
using CQRS.Demo.Model.Investments;
using CQRS.Demo.Web.Models;
using Grit.CQRS;
using Grit.CQRS.Actions;
using Grit.CQRS.Exceptions;
using Grit.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CQRS.Demo.Web.Controllers
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
        public ActionResult Create(InvestViewModel vm)
        {
            var action = new InvestmentCreateRequest
            {
                InvestmentId = _sequenceService.Next(SequenceID.CQRS_Investment, 1),
                AccountId = vm.AccountId,
                ProjectId = vm.ProjectId,
                Amount = vm.Amount
            };

            ActionResponse response = ServiceLocator.ActionBus.Send(action);
            TempData["ActionResponse"] = response;
            return RedirectToAction("Index", new { id = action.InvestmentId });
        }

        public ActionResult Index(int id)
        {
            var investment = _investmentService.Get(id);
            return View(investment);
        }

        public ActionResult Pay(int id)
        {
            var action = new InvestmentPayRequest
            {
                InvestmentId = id
            };

            ActionResponse response = ServiceLocator.ActionBus.Send(action);
            TempData["ActionResponse"] = response;
            return RedirectToAction("Index", new { id = action.InvestmentId });
        }
    }
}