using ACE.Demo.Contracts;
using ACE.Demo.Contracts.Actions;
using ACE.Demo.Contracts.Commands;
using ACE.Demo.Contracts.Events;
using ACE.Demo.Model.Investments;
using ACE.Demo.Light.Web.Models;
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

namespace ACE.Demo.Light.Web.Controllers
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
                InvestmentId = _sequenceService.Next(SequenceID.ACE_Investment, 1),
                AccountId = vm.AccountId,
                ProjectId = vm.ProjectId,
                Amount = vm.Amount
            };

            try
            {
                ServiceLocator.ActionBus.Invoke<InvestmentCreateRequest>(action);
            }
            catch (BusinessException ex)
            {
                TempData["ActionResponse"] = ex.Message;
            }
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

            try
            {
                ServiceLocator.ActionBus.Invoke<InvestmentPayRequest>(action);
            }
            catch (BusinessException ex)
            {
                TempData["ActionResponse"] = ex.Message;
            }
            return RedirectToAction("Index", new { id = action.InvestmentId });
        }
    }
}