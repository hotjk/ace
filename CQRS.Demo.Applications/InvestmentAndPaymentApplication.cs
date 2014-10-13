using CQRS.Demo.Contracts.Actions;
using CQRS.Demo.Contracts.Commands;
using CQRS.Demo.Contracts.Events;
using CQRS.Demo.Model.Accounts;
using CQRS.Demo.Model.Investments;
using CQRS.Demo.Model.Projects;
using Grit.CQRS;
using Grit.CQRS.Exceptions;
using Grit.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CQRS.Demo.Applications
{
    public class InvestmentAndPaymentApplication :
        IActionHandler<InvestmentCreateRequest>,
        IActionHandler<InvestmentPayRequest>
    {
        public InvestmentAndPaymentApplication(
            IAccountService accountService,
            IProjectService projectService,
            IInvestmentService investmentService)
        {
            _accountService = accountService;
            _projectService = projectService;
            _investmentService = investmentService;
        }

        private IAccountService _accountService;
        private IProjectService _projectService;
        private IInvestmentService _investmentService;

        static InvestmentAndPaymentApplication()
        {
            AutoMapper.Mapper.CreateMap<InvestmentCreateRequest, CreateInvestment>();
            AutoMapper.Mapper.CreateMap<InvestmentPayRequest, CompleteInvestment>();
        }

        public void Invoke(InvestmentCreateRequest action)
        {
            var account = _accountService.Get(action.AccountId);
            if (account.Amount < action.Amount)
            {
                throw new BusinessException("用户账户余额不足。");
            }

            var project = _projectService.Get(action.ProjectId);
            if (project.Amount < action.Amount)
            {
                throw new BusinessException("项目可投资金额不足。");
            }

            using (UnitOfWork u = new UnitOfWork())
            {
                ServiceLocator.CommandBus.Send(AutoMapper.Mapper.Map<CreateInvestment>(action));
                u.Complete();
            }
        }

        public void Invoke(InvestmentPayRequest action)
        {
            var investment = _investmentService.Get(action.InvestmentId);
            if (investment == null)
            {
                throw new BusinessException("投资不存在。");
            }
            if (investment.Status != Contracts.Enum.InvestmentStatus.Initial)
            {
                throw new BusinessException("投资已经支付。");
            }
            Project project = _projectService.Get(investment.ProjectId);

            using (UnitOfWork u = new UnitOfWork())
            {
                ServiceLocator.CommandBus
                    .Send(new CompleteInvestment
                    {
                        ActionId = action.ActionId,
                        InvestmentId = action.InvestmentId
                    })
                    .Send(new ChangeProjectAmount
                    {
                        ActionId = action.ActionId,
                        ProjectId = project.ProjectId,
                        Change = 0 - investment.Amount
                    })
                    .Send(new ChangeAccountAmount
                    {
                        ActionId = action.ActionId,
                        AccountId = investment.AccountId,
                        Change = 0 - investment.Amount
                    })
                    .Send(new ChangeAccountAmount
                    {
                        ActionId = action.ActionId,
                        AccountId = project.BorrowerId,
                        Change = investment.Amount
                    })
                    .Send(new CreateAccountActivity
                    {
                        ActionId = action.ActionId,
                        FromAccountId = investment.AccountId,
                        ToAccountId = project.BorrowerId,
                        Amount = investment.Amount
                    });
                u.Complete();
            }
        }
    }
}

