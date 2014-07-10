using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Accounts
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _repository;
        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public Account Get(int id)
        {
            return _repository.Get(id);
        }
    }
}
