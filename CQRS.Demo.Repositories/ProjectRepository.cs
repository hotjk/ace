using CQRS.Demo.Model.Projects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CQRS.Demo.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public Project Get(int id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<Project>("SELECT ProjectId, Name, Amount, BorrowerId FROM cqrs_demo_project;",
                    new { id = id }).SingleOrDefault();
            }
        }
    }
}
