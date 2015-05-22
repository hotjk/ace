using System;

namespace ACE
{
    public interface IActionHandlerFactory
    {
        IActionHandler<T> GetHandler<T>() where T : IAction;
        Type GetType(string name);
    }
}
