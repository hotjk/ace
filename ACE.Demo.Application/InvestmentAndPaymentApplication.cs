using ACE.Demo.Contracts.Actions;
using ACE.Demo.Contracts.Commands;
using ACE.Demo.Contracts.Events;
using ACE.Demo.Model.Accounts;
using ACE.Demo.Model.Investments;
using ACE.Demo.Model.Projects;
using ACE;
using ACE.Exceptions;
using Grit.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ACE.Demo.Contracts;

namespace ACE.Demo.Application
{
    public class InvestmentAndPaymentProcessManager :
        IActionHandler<InvestmentCreateRequest>,
        IActionHandler<InvestmentPayRequest>
    {
        public InvestmentAndPaymentProcessManager(
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

        static InvestmentAndPaymentProcessManager()
        {
            AutoMapper.Mapper.CreateMap<InvestmentCreateRequest, CreateInvestment>();
            AutoMapper.Mapper.CreateMap<InvestmentPayRequest, CompleteInvestment>();
        }

        public void Invoke(InvestmentCreateRequest action)
        {
            var account = _accountService.Get(action.AccountId);
            if (account.Amount < action.Amount)
            {
                throw new BusinessException(BusinessExceptionType.UserBalanceOverflow,"用户账户余额不足。");
            }

            var project = _projectService.Get(action.ProjectId);
            if (project.Amount < action.Amount)
            {
                throw new BusinessException(BusinessExceptionType.ProjectBalanceOverflow, "项目可投资金额不足。");
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
                throw new BusinessException(BusinessExceptionType.InvestmentNotExist, "投资不存在。");
            }
            if (investment.Status != Contracts.Enum.InvestmentStatus.Initial)
            {
                throw new BusinessException(BusinessExceptionType.InvestmentPaied, "投资已经支付。");
            }
            Project project = _projectService.Get(investment.ProjectId);

            using (UnitOfWork u = new UnitOfWork())
            {
                ServiceLocator.CommandBus
                    .Send(new CompleteInvestment
                    {
                        InvestmentId = action.InvestmentId
                    })
                    .Send(new ChangeProjectAmount
                    {
                        ProjectId = project.ProjectId,
                        Change = 0 - investment.Amount
                    })
                    .Send(new ChangeAccountAmount
                    {
                        AccountId = investment.AccountId,
                        Change = 0 - investment.Amount
                    })
                    .Send(new ChangeAccountAmount
                    {
                        AccountId = project.BorrowerId,
                        Change = investment.Amount
                    })
                    .Send(new CreateAccountActivity
                    {
                        FromAccountId = investment.AccountId,
                        ToAccountId = project.BorrowerId,
                        Amount = investment.Amount
                    });
                u.Complete();
            }
        }
    }
}

