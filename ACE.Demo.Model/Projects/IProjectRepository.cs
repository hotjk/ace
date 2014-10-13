using System;

namespace ACE.Demo.Model.Projects
{
    public interface IProjectRepository
    {
        Project Get(int id);
    }
}
