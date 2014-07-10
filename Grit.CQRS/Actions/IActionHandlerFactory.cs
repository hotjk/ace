using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public interface IActionHandlerFactory
    {
        IActionHandler<T> GetHandler<T>() where T : Action;
        Type GetType(string name);
    }
}
