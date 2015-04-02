using ACE.Demo.Contracts.Commands;
using ACE.Demo.Model.AccountActivities;
using ACE;
using ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Demo.Contracts;

namespace ACE.Demo.Model.Write.AccountActivities
{
    public class AccountActivityHandler : ICommandHandler<CreateAccountActivity>
    {
        static AccountActivityHandler()
        {
            ACEMapper.CreateMap<CreateAccountActivity, AccountActivity>();
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
                throw new BusinessException(BusinessStatusCode.BadRequest, "账户交易双方不能同时为空。");
            }
            _repository.Save(ACEMapper.Map<AccountActivity>(command));
        }
    }
}
