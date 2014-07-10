using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    /// <summary>
    /// Action is a message direct send to SAGA via RabbitMQ.
    /// SAGA will process this message and send an ActionResponse to orignal sender.
    /// Action SHOULD NOT host big size entities since it will be serilized(json) and saved in event log.
    /// </summary>
    public interface IAction
    {
    }
}
