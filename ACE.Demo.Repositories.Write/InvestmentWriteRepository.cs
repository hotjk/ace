using ACE.Demo.Model.Investments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using ACE.Demo.Contracts.Enum;


namespace ACE.Demo.Repositories.Write
{
    public class InvestmentWriteRepository : BaseRepository, IInvestmentWriteRepository
    {
        public bool Add(Investment investment)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    "INSERT INTO ace_demo_investment (InvestmentId, ProjectId, AccountId, Amount, Status) VALUES (@InvestmentId, @ProjectId, @AccountId, @Amount, @Status);",
                    investment);
            }
        }

        public Investment GetForUpdate(int investmentId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<Investment>(
@"SELECT InvestmentId, ProjectId, AccountId, Amount, Status 
FROM ace_demo_investment 
WHERE InvestmentId = @InvestmentId 
FOR UPDATE;",
                    new { InvestmentId = investmentId }).SingleOrDefault();
            }
        }

        public bool Complete(int investmentId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    "UPDATE ace_demo_investment SET Status = @Status WHERE InvestmentId = @InvestmentId;",
                    new { investmentId = investmentId, Status = InvestmentStatus.Paied });
            }
        }
    }
}
