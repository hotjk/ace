using CQRS.Demo.Model.Write.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CQRS.Demo.Repositories.Write
{
    public class MessageWriteRepository : BaseRepository, IMessageWriteRepository
    {

        public bool Add(Model.Messages.Message message)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute("INSERT INTO cqrs_demo_message (AccountId, Content) VALUES (@AccountId, @Content);",
                    message);
            }
        }
    }
}
