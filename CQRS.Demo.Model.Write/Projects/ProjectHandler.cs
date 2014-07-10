using CQRS.Demo.Contracts.Events;
using CQRS.Demo.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grit.CQRS;
using Grit.CQRS.Exceptions;

namespace CQRS.Demo.Model.Projects
{
    public class ProjectHandler : 
        ICommandHandler<ChangeProjectAmount>
    {
        static ProjectHandler()
        {
            AutoMapper.Mapper.CreateMap<ChangeProjectAmount, ProjectAmountChanged>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }

        private IProjectWriteRepository _repository;
        public ProjectHandler(IProjectWriteRepository repository)
        {
            _repository = repository;
        }

        public void Execute(ChangeProjectAmount command)
        {
            if (!_repository.ChangeAmount(command.ProjectId, command.Change))
            {
                throw new BusinessException("项目可投资金额不足。");
            }
            ServiceLocator.EventBus.Publish(AutoMapper.Mapper.Map<ProjectAmountChanged>(command));
        }
    }
}

