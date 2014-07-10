using System;

namespace CQRS.Demo.Model.Projects
{
    public interface IProjectService
    {
        Project Get(int id);
    }
}
