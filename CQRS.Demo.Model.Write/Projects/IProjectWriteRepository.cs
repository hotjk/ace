using System;

namespace CQRS.Demo.Model.Projects
{
    public interface IProjectWriteRepository
    {
        bool Create(Project project);
        bool ChangeAmount(int projectId, decimal Amount);
    }
}
