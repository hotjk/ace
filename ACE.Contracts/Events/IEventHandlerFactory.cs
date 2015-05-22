using System;
using System.Collections.Generic;

namespace ACE
{
    public interface IEventHandlerFactory
    {
        IEnumerable<IEventHandler<T>> GetHandlers<T>() where T : IEvent;
        Type GetType(string name);
    }
}
