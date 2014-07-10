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
            string json = JsonConvert.SerializeObject(command);
            log4net.LogManager.GetLogger("command.logger").Info(
                string.Format("Command Send {0} {1}", command, json));

            var handler = _commandHandlerFactory.GetHandler<T>();
            handler.Execute(command);
            return this;
        }
    }
}
