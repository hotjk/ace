using ACE.Demo.Contracts.Events;
using ACE.Demo.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE;
using ACE.Exceptions;
using ACE.Demo.Contracts;

namespace ACE.Demo.Model.Projects
{
    public class ProjectHandler : HandlerBase,
        ICommandHandler<ChangeProjectAmount>
    {
        static ProjectHandler()
        {
            ACEMapper.CreateMapIgnoreId<ChangeProjectAmount, ProjectAmountChanged>();
        }

        private IProjectWriteRepository _repository;
        public ProjectHandler(IEventBus eventBus,
            IProjectWriteRepository repository)
            : base(eventBus)
        {
            _repository = repository;
        }

        public void Execute(ChangeProjectAmount command)
        {
            if (!_repository.ChangeAmount(command.ProjectId, command.Change))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "项目可投资金额不足。");
            }
            EventBus.Publish(ACEMapper.Map<ProjectAmountChanged>(command).ToExternalQueue());
        }
    }
}

