using System;

namespace ACE
{
    public interface IActionHandlerFactory
    {
        IActionHandler<T> GetHandler<T>() where T : Action;
        Type GetType(string name);
    }
}
