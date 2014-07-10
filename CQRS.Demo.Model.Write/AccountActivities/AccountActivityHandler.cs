using CQRS.Demo.Contracts.Commands;
using CQRS.Demo.Model.AccountActivities;
using Grit.CQRS;
using Grit.CQRS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Write.AccountActivities
{
    public class AccountActivityHandler : ICommandHandler<CreateAccountActivity>
    {
        static AccountActivityHandler()
        {
            AutoMapper.Mapper.CreateMap<CreateAccountActivity, AccountActivity>();
        }
        public AccountActivityHandler(IAccountActivityWriteRepository repository)
        {
            _repository = repository;
        }
        private IAccountActivityWriteRepository _repository;
        public void Execute(CreateAccountActivity command)
        {
            if(command.FromAccountId == null && command.ToAccountId == null)
            {
                throw new BusinessException("账户交易双方不能同时为空。");
            }
            _repository.Save(AutoMapper.Mapper.Map<AccountActivity>(command));
        }
    }
}
