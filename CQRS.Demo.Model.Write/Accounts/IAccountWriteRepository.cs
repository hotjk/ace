using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Accounts
{
    public interface IAccountWriteRepository
    {
        bool Create(Account account);
        bool ChangeAmount(int accountId, decimal Amount);
    }
}
