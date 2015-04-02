using ACE.Demo.Contracts;
using ACE.Demo.Contracts.Commands;
using ACE.Demo.Contracts.Events;
using ACE.Demo.Model.Accounts;
using ACE.Demo.Model.Projects;
using ACE.Exceptions;

namespace ACE.Demo.Model.Investments
{
    public class InvestmentHandler : HandlerBase,
        ICommandHandler<CreateInvestment>,
        ICommandHandler<CompleteInvestment>
    {
        static InvestmentHandler()
        {
            ACEMapper.CreateMap<CreateInvestment, Investment>();
            ACEMapper.CreateMapIgnoreId<CreateInvestment, InvestmentStatusCreated>();
            ACEMapper.CreateMapIgnoreId<Investment, ChangeAccountAmount>();
            ACEMapper.CreateMapIgnoreId<Investment, ChangeProjectAmount>();
            ACEMapper.CreateMapIgnoreId<Investment, InvestmentStatusCompleted>();
        }

        private IInvestmentWriteRepository _repository;
        private IProjectService _projectService;
        private IAccountService _accountService;

        public InvestmentHandler(IEventBus eventBus,
            IInvestmentWriteRepository repository,
            IProjectService projectService,
            IAccountService accountService)
            : base(eventBus)
        {
            _repository = repository;
            _projectService = projectService;
            _accountService = accountService;
        }

        public void Execute(CreateInvestment command)
        {
            Investment investment = _repository.GetForUpdate(command.InvestmentId);
            if(investment != null)
            {
                throw new BusinessException(BusinessStatusCode.Conflict, "投资已经存在，不要重复提交。"); 
            }
            _repository.Add(ACEMapper.Map<Investment>(command));
            EventBus.Publish(ACEMapper.Map<InvestmentStatusCreated>(command)
                .DistributeInThreadPool()
                .DistributeToExternalQueue());
        }

        public void Execute(CompleteInvestment command)
        {
            Investment investment = _repository.GetForUpdate(command.InvestmentId);
            if (investment == null)
            {
                throw new BusinessException(BusinessStatusCode.NotFound, "投资不存在。");
            }
            _repository.Complete(command.InvestmentId);
            EventBus.Publish(ACEMapper.Map<InvestmentStatusCompleted>(investment)
                .DistributeInThreadPool()
                .DistributeToExternalQueue());
        }
    }
}
