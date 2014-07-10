using CQRS.Demo.Model.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace CQRS.Demo.Repositories.Write
{
    public class ProjectWriteRepository : BaseRepository, IProjectWriteRepository
    {
        public bool Create(Project project)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute("INSERT INTO cqrs_demo_project (ProjectId, Name, Amount, BorrowerId) VALUES (@ProjectId, @Name, @Amount, @BorrowerId);", 
                    project);
            }
        }

        public bool ChangeAmount(int projectId, decimal amount)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute("UPDATE cqrs_demo_project SET Amount = Amount + @Amount WHERE ProjectId = @ProjectId AND Amount + @Amount >= 0;",
                    new { ProjectId = projectId, Amount = amount });
            }
        }
    }
}
