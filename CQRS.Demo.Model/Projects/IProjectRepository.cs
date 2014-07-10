using System;

namespace CQRS.Demo.Model.Projects
{
    public interface IProjectRepository
    {
        Project Get(int id);
    }
}
