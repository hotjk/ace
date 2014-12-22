using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ACE.Demo.Model.Investments;

namespace ACE.Demo.Repositories
{
    public class InvestmentRepository : BaseRepository, IInvestmentRepository
    {
        public Investment Get(int id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<Investment>(
                    "SELECT InvestmentId, ProjectId, AccountId, Amount, Status FROM ace_demo_investment WHERE InvestmentId = @InvestmentId;",
                    new { InvestmentId = id }).SingleOrDefault();
            }
        }

        public IEnumerable<Investment> GetAll()
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<Investment>(
                    "SELECT InvestmentId, ProjectId, AccountId, Amount, Status FROM ace_demo_investment ORDER BY InvestmentId DESC LIMIT 10;"
                    );
            }
        }
    }
}
