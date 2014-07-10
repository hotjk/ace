using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grit.Utility.Sql;
using Dapper;
using System.Transactions;
using MySql.Data.MySqlClient;

namespace Grit.Sequence.Repository.MySql
{
    public class SequenceRepository : BaseRepository, ISequenceRepository
    {
        public int Next(int id, int step)
        {
            int? next = null;

            using (IDbConnection conn = OpenConnection())
            {
                const string query = "SELECT Value FROM Sequence WHERE Id=@Id FOR UPDATE; UPDATE Sequence SET Value=Value+@Step WHERE Id=@Id;";
                IDbTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                next = conn.Query<int?>(query, new { Id = id, Step = step }).SingleOrDefault();
                transaction.Commit();
            }

            if (next == null)
            {
                throw new ApplicationException(string.Format("Sequence does not found in database, id: {0}.", id));
            }
            return next.Value;
        }

        public int NextWithTransactionScope(int id, int step)
        {
            int? next = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead }))
            {
                using (IDbConnection conn = OpenConnection())
                {
                    const string query = "SELECT Value FROM Sequence WHERE Id=@Id FOR UPDATE; UPDATE Sequence SET Value=Value+@Step WHERE Id=@Id;";
                    next = conn.Query<int?>(query, new { Id = id, Step = step }).SingleOrDefault();
                    if (next == null)
                    {
                        throw new ApplicationException(string.Format("Sequence does not found in database, id: {0}.", id));
                    }
                }
                scope.Complete();
            }

            if (next == null)
            {
                throw new ApplicationException(string.Format("Sequence does not found in database, id: {0}.", id));
            }
            return next.Value;
        }
    }
}
