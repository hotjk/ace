using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public class CommandBus : ICommandBus
    {
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public CommandBus(ICommandHandlerFactory commandHandlerFactory)
        {
            _commandHandlerFactory = commandHandlerFactory;
        }

        public ICommandBus Send<T>(T command) where T : Command
        {
            ServiceLocator.BusLogger.CommandSend(command);

            var handler = _commandHandlerFactory.GetHandler<T>();
            handler.Execute(command);
            return this;
        }
    }
}
