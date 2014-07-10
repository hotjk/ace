using CQRS.Demo.Contracts.Events;
using CQRS.Demo.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grit.CQRS;
using Grit.CQRS.Exceptions;

namespace CQRS.Demo.Model.Accounts
{
    public class AccountHandler :
        ICommandHandler<CreateAccount>,
        ICommandHandler<ChangeAccountAmount>
    {
        static AccountHandler()
        {
            AutoMapper.Mapper.CreateMap<ChangeAccountAmount, AccountAmountChanged>().ForMember(dest => dest.Id, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<CreateAccount, Account>();
            AutoMapper.Mapper.CreateMap<CreateAccount, AccountStatusCreated>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }
        private IAccountWriteRepository _repository;
        public AccountHandler(IAccountWriteRepository repository)
        {
            _repository = repository;
        }
        public void Execute(ChangeAccountAmount command)
        {
            if (!_repository.ChangeAmount(command.AccountId, command.Change))
            {
                throw new BusinessException("账户余额不足。");
            }
            ServiceLocator.EventBus.Publish(AutoMapper.Mapper.Map<AccountAmountChanged>(command));
        }

        public void Execute(CreateAccount command)
        {
            if (!_repository.Create(AutoMapper.Mapper.Map<Account>(command)))
            {
                throw new BusinessException("账户已存在。");
            }
            ServiceLocator.EventBus.Publish(AutoMapper.Mapper.Map<AccountStatusCreated>(command));
        }
    }
}
