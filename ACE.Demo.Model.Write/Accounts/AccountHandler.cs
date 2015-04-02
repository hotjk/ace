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

namespace ACE.Demo.Model.Accounts
{
    public class AccountHandler : HandlerBase,
        ICommandHandler<CreateAccount>,
        ICommandHandler<ChangeAccountAmount>
    {
        static AccountHandler()
        {
            ACEMapper.CreateMapIgnoreId<ChangeAccountAmount, AccountAmountChanged>();
            ACEMapper.CreateMap<CreateAccount, Account>();
            ACEMapper.CreateMapIgnoreId<CreateAccount, AccountStatusCreated>();
        }
        private IAccountWriteRepository _repository;
        public AccountHandler(IEventBus eventBus,
            IAccountWriteRepository repository)
            : base(eventBus)
        {
            _repository = repository;
        }
        public void Execute(ChangeAccountAmount command)
        {
            if (!_repository.ChangeAmount(command.AccountId, command.Change))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "账户余额不足。");
            }
            EventBus.Publish(ACEMapper.Map<AccountAmountChanged>(command).DistributeInThreadPool().DistributeToExternalQueue());
        }

        public void Execute(CreateAccount command)
        {
            if (!_repository.Create(ACEMapper.Map<Account>(command)))
            {
                throw new BusinessException(BusinessStatusCode.Conflict, "账户已存在。");
            }
            EventBus.Publish(ACEMapper.Map<AccountStatusCreated>(command).DistributeInThreadPool().DistributeToExternalQueue());
        }
    }
}
