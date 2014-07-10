using CQRS.Demo.Contracts.Events;
using CQRS.Demo.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grit.CQRS;
using CQRS.Demo.Model.Investments;
using CQRS.Demo.Model.Projects;
using Grit.CQRS.Exceptions;
using CQRS.Demo.Model.Accounts;

namespace CQRS.Demo.Model.Investments
{
    public class InvestmentHandler :
        ICommandHandler<CreateInvestment>,
        ICommandHandler<CompleteInvestment>
    {
        static InvestmentHandler()
        {
            AutoMapper.Mapper.CreateMap<CreateInvestment, Investment>();
            AutoMapper.Mapper.CreateMap<CreateInvestment, InvestmentStatusCreated>().ForMember(dest => dest.Id, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<Investment, ChangeAccountAmount>().ForMember(dest => dest.Id, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<Investment, ChangeProjectAmount>().ForMember(dest => dest.Id, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<Investment, InvestmentStatusCompleted>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }

        private IInvestmentWriteRepository _repository;
        private IProjectService _projectService;
        private IAccountService _accountService;

        public InvestmentHandler(IInvestmentWriteRepository repository,
            IProjectService projectService,
            IAccountService accountService)
        {
            _repository = repository;
            _projectService = projectService;
            _accountService = accountService;
        }

        public void Execute(CreateInvestment command)
        {
            _repository.Add(AutoMapper.Mapper.Map<Investment>(command));
            ServiceLocator.EventBus.Publish(AutoMapper.Mapper.Map<InvestmentStatusCreated>(command));
        }

        public void Execute(CompleteInvestment command)
        {
            Investment investment = _repository.GetForUpdate(command.InvestmentId);
            _repository.Complete(command.InvestmentId);
            ServiceLocator.EventBus.Publish(AutoMapper.Mapper.Map<InvestmentStatusCompleted>(investment));
        }
    }
}
