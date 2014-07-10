using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    /// <summary>
    /// Command is used to decoupe the aggragate.
    /// Single command MUST be handle by single method.
    /// Command SHOULD run in UnitOfWork(TransactionScope).
    /// Command never be sent to RabbitMQ.
    /// Command MAY send event, all events will real flush when UnitOfWork completed.
    /// Command can host big size entities.
    /// </summary>
    public interface ICommand
    {
    }
}
