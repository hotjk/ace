using ACE.Demo.Model.Write.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ACE.Demo.Repositories.Write
{
    public class MessageWriteRepository : BaseRepository, IMessageWriteRepository
    {

        public bool Add(Model.Messages.Message message)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute("INSERT INTO ace_demo_message (AccountId, Content) VALUES (@AccountId, @Content);",
                    message);
            }
        }
    }
}
