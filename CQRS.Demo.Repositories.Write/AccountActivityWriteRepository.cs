using CQRS.Demo.Model.Write.AccountActivities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CQRS.Demo.Repositories.Write
{
    public class AccountActivityWriteRepository : BaseRepository, IAccountActivityWriteRepository
    {
        public bool Save(Model.AccountActivities.AccountActivity activity)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute("INSERT INTO cqrs_demo_account_activity (FromAccountId, ToAccountId, Amount) VALUES (@FromAccountId, @ToAccountId, @Amount);",
                    activity);
            }
        }
    }
}
