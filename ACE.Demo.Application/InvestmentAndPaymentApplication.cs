using ACE.Demo.Contracts;
using ACE.Demo.Contracts.Actions;
using ACE.Demo.Contracts.Commands;
using ACE.Demo.Model.Accounts;
using ACE.Demo.Model.Investments;
using ACE.Demo.Model.Projects;
using ACE.Exceptions;

namespace ACE.Demo.Application
{
    public class InvestmentAndPaymentProcessManager : ProcessManagerBase,
        IActionHandler<InvestmentCreateRequest>,
        IActionHandler<InvestmentPayRequest>
    {
        public InvestmentAndPaymentProcessManager(ICommandBus commandBus, IEventBus eventBus,
            IAccountService accountService,
            IProjectService projectService,
            IInvestmentService investmentService)
            : base(commandBus, eventBus)
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
                throw new BusinessException(BusinessStatusCode.Forbidden, "用户账户余额不足。");
            }

            var project = _projectService.Get(action.ProjectId);
            if (project.Amount < action.Amount)
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "项目可投资金额不足。");
            }

            using (UnitOfWork u = new UnitOfWork(EventBus))
            {
                CommandBus.Send(AutoMapper.Mapper.Map<CreateInvestment>(action));
                u.Complete();
            }
        }

        public void Invoke(InvestmentPayRequest action)
        {
            var investment = _investmentService.Get(action.InvestmentId);
            if (investment == null)
            {
                throw new BusinessException(BusinessStatusCode.NotFound, "投资不存在。");
            }
            if (investment.Status != Contracts.Enum.InvestmentStatus.Initial)
            {
                throw new BusinessException(BusinessStatusCode.Conflict, "投资已经支付。");
            }
            Project project = _projectService.Get(investment.ProjectId);

            using (UnitOfWork u = new UnitOfWork(EventBus))
            {
                CommandBus
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

