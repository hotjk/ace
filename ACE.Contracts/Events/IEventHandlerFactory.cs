using System;
using System.Collections.Generic;

namespace ACE
{
    public interface IEventHandlerFactory
    {
        IEnumerable<IEventHandler<T>> GetHandlers<T>() where T : Event;
        Type GetType(string name);
    }
}
