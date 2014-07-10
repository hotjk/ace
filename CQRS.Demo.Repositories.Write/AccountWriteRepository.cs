using CQRS.Demo.Model.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace CQRS.Demo.Repositories.Write
{
    public class AccountWriteRepository : BaseRepository, IAccountWriteRepository
    {
        public bool Create(Account account)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute("INSERT INTO cqrs_demo_account (AccountId, Amount) VALUES (@AccountId, @Amount);",
                    account);
            }
        }

        public bool ChangeAmount(int accountId, decimal amount)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute("UPDATE cqrs_demo_account SET Amount = Amount + @Amount WHERE AccountId = @AccountId AND Amount + @Amount >= 0;",
                    new { AccountId = accountId, Amount = amount });
            }
        }
    }
}
