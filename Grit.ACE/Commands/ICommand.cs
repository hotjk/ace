using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE
{
    /// <summary>
    /// Command is used to decoupe the aggragate service.
    /// One command MUST be handle by one method.
    /// Command SHOULD run in UnitOfWork(TransactionScope).
    /// Command never be sent to RabbitMQ.
    /// Command MAY send event, all events will real flush when UnitOfWork completed.
    /// Command can host big size entities.
    /// </summary>
    public interface ICommand
    {
    }
}
