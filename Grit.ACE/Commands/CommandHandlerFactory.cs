using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private IKernel _container;
        private IEnumerable<string> _commandAssmblies;
        private IEnumerable<string> _handlerAssmblies;
        private IDictionary<Type, Type> _handlers;
        private readonly object _lockThis = new object();

        public CommandHandlerFactory(IKernel container,
            IEnumerable<string> commandAssmblies,
            IEnumerable<string> handlerAssmblies)
        {
            lock (_lockThis)
            {
                _container = container;
                _commandAssmblies = commandAssmblies;
                _handlerAssmblies = handlerAssmblies;
                Utility.EnsoureAssemblyLoaded(_commandAssmblies);
                Utility.EnsoureAssemblyLoaded(_handlerAssmblies);
                HookHandlers();
                BindHandlers();
            }
        }

        private void HookHandlers()
        {
            _handlers = new Dictionary<Type, Type>();
            List<Type> commands = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies.Where(n => _commandAssmblies.Any(m => m == n.GetName().Name)))
            {
                commands.AddRange(assembly.GetExportedTypes().Where(x => x.IsSubclassOf(typeof(Command))));
            }

            foreach (var assembly in assemblies.Where(n => _handlerAssmblies.Any(m => m == n.GetName().Name)))
            {
                var allHandlers = assembly.GetExportedTypes().Where(x => x.GetInterfaces()
                        .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));
                foreach (var command in commands)
                {
                    var handlers = allHandlers
                        .Where(h => h.GetInterfaces()
                            .Any(i => i.GetGenericArguments()
                                .Any(e => e == command))).ToList();

                    if (handlers.Count > 1 ||
                        (handlers.Count == 1 && _handlers.ContainsKey(command)))
                    {
                        throw new MoreThanOneDomainCommandHandlerException("more than one handler for command: " + command.Name);
                    }
                    if (handlers.Count == 1)
                    {
                        _handlers[command] = handlers.First();
                    }
                }
            }
            foreach (var command in commands)
            {
                if (!_handlers.ContainsKey(command))
                {
                    throw new UnregisteredDomainCommandException("no handler registered for command: " + command.Name);
                }
            }
        }

        private void BindHandlers()
        {
            foreach (var kv in _handlers)
            {
                _container.Bind(kv.Key).To(kv.Value).InSingletonScope();
            }
        }

        private string Log()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("CommandBus:{0}", Environment.NewLine);
            foreach(var kv in this._handlers)
            {
                sb.AppendFormat("{0}{1}", kv.Key, Environment.NewLine);
                sb.AppendFormat("\t{0}{1}", kv.Value, Environment.NewLine);
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public ICommandHandler<T> GetHandler<T>() where T : Command
        {
            Type handler;
            if (!_handlers.TryGetValue(typeof(T), out handler))
            {
                throw new UnregisteredDomainCommandException("no handler registered for command: " + typeof(T));
            }

            return (ICommandHandler<T>)_container.GetService(handler);
        }
    }
}
