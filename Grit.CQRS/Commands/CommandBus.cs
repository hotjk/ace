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
            if(ServiceLocator.CommandLogger.IsInfoEnabled)
            {
                ServiceLocator.CommandLogger.Info(
                    string.Format("Command Send {0} {1}", 
                    command, 
                    JsonConvert.SerializeObject(command)));;
            }
            var handler = _commandHandlerFactory.GetHandler<T>();
            handler.Execute(command);
            return this;
        }
    }
}
