using ACE.Demo.Contracts.Events;
using ACE.Demo.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE;
using ACE.Demo.Model.Investments;
using ACE.Demo.Model.Projects;
using ACE.Exceptions;
using ACE.Demo.Model.Accounts;

namespace ACE.Demo.Model.Investments
{
    public class InvestmentHandler : HandlerBase,
        ICommandHandler<CreateInvestment>,
        ICommandHandler<CompleteInvestment>
    {
        static InvestmentHandler()
        {
            AutoMapper.Mapper.CreateMap<CreateInvestment, Investment>();
            AutoMapper.Mapper.CreateMap<CreateInvestment, InvestmentStatusCreated>().ForMember(dest => dest._id, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<Investment, ChangeAccountAmount>().ForMember(dest => dest._id, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<Investment, ChangeProjectAmount>().ForMember(dest => dest._id, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<Investment, InvestmentStatusCompleted>().ForMember(dest => dest._id, opt => opt.Ignore());
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
            _repository.Add(AutoMapper.Mapper.Map<Investment>(command));
            EventBus.Publish(AutoMapper.Mapper.Map<InvestmentStatusCreated>(command)
                .DistributeInThreadPool()
                .DistributeToExternalQueue());
        }

        public void Execute(CompleteInvestment command)
        {
            Investment investment = _repository.GetForUpdate(command.InvestmentId);
            _repository.Complete(command.InvestmentId);
            EventBus.Publish(AutoMapper.Mapper.Map<InvestmentStatusCompleted>(investment)
                .DistributeInThreadPool()
                .DistributeToExternalQueue());
        }
    }
}
