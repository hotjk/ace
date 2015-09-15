using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace ACE
{
    public class ActionHandlerFactory : IActionHandlerFactory
    {
        private IContainer _container;
        private IEnumerable<string> _actionAssmblies;
        private IEnumerable<string> _handlerAssmblies;
        private IDictionary<Type, Type> _handlers;
        private readonly object _lockThis = new object();
        private IDictionary<string, Type> _actionTypes;

        public ActionHandlerFactory(IContainer container,
            IEnumerable<string> actionAssmblies,
            IEnumerable<string> handlerAssmblies)
        {
            lock (_lockThis)
            {
                _container = container;
                _actionAssmblies = actionAssmblies;
                _handlerAssmblies = handlerAssmblies;
                Utility.EnsoureAssemblyLoaded(_actionAssmblies);
                Utility.EnsoureAssemblyLoaded(_handlerAssmblies);
                HookHandlers();
                BindHandlers();
            }
        }

        public Type GetType(string actionName)
        {
            return _actionTypes[actionName];
        }

        private void HookHandlers()
        {
            _handlers = new Dictionary<Type, Type>();
            List<Type> actions = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies.Where(n => _actionAssmblies.Any(m => m == n.GetName().Name)))
            {
                actions.AddRange(assembly.GetExportedTypes().Where(x => typeof(IAction).IsAssignableFrom(x)).Where(x => !x.IsAbstract));
            }

            foreach (var assembly in assemblies.Where(n => _handlerAssmblies.Any(m => m == n.GetName().Name)))
            {
                var allHandlers = assembly.GetExportedTypes().Where(x => x.GetInterfaces()
                        .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IActionHandler<>)));
                foreach (var action in actions)
                {
                    var handlers = allHandlers
                        .Where(h => h.GetInterfaces()
                            .Any(i => i.GetGenericArguments()
                                .Any(e => e == action))).ToList();

                    if (handlers.Count > 1 ||
                        (handlers.Count == 1 && _handlers.ContainsKey(action)))
                    {
                        throw new MoreThanOneDomainCommandHandlerException("more than one handler for action: " + action.Name);
                    }
                    if (handlers.Count == 1)
                    {
                        _handlers[action] = handlers.First();
                    }
                }
            }

            _actionTypes = new Dictionary<string, Type>();
            foreach (Type type in actions)
            {
                _actionTypes[type.Name] = type;
            }
        }

        private void BindHandlers()
        {
            var builder = new ContainerBuilder();
            foreach (var v in _handlers.Values.Distinct())
            {
                builder.RegisterType(v).SingleInstance();
            }
            builder.Update(_container);
        }

        private string Log()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("ActionBus:{0}", Environment.NewLine);
            foreach (var kv in this._handlers)
            {
                sb.AppendFormat("{0}{1}", kv.Key, Environment.NewLine);
                sb.AppendFormat("\t{0}{1}", kv.Value, Environment.NewLine);
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public IActionHandler<T> GetHandler<T>() where T : IAction
        {
            Type handler;
            if (!_handlers.TryGetValue(typeof(T), out handler))
            {
                throw new UnregisteredDomainCommandException("no handler registered for action: " + typeof(T));
            }

            return (IActionHandler<T>)_container.Resolve(handler);
        }
    }
}
