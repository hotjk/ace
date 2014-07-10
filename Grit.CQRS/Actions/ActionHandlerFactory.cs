using Ninject;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public class ActionHandlerFactory : IActionHandlerFactory
    {
        private static IEnumerable<string> _actionAssmblies;
        private static IEnumerable<string> _handlerAssmblies;
        private static IDictionary<Type, Type> _handlers;
        private static bool _isInitialized;
        private static readonly object _lockThis = new object();

        private static IDictionary<string, Type> _actionTypes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionAssmblies">The assmbly name list that keep the domain command/action.</param>
        /// <param name="handlerAssmblies">The assmbly name list that keep the domain command/action handlers</param>
        /// <param name="channel">RabbitMQ queue channel</param>
        /// <param name="queue">RabbitMQ exchange name</param>
        public static void Init(IEnumerable<string> actionAssmblies,
            IEnumerable<string> handlerAssmblies)
        {
            if (!_isInitialized)
            {
                lock (_lockThis)
                {
                    _actionAssmblies = actionAssmblies;
                    _handlerAssmblies = handlerAssmblies;
                    HookHandlers();
                    _isInitialized = true;
                }
            }
        }

        public Type GetType(string actionName)
        {
            return _actionTypes[actionName];
        }

        private static void HookHandlers()
        {
            _handlers = new Dictionary<Type, Type>();
            List<Type> actions = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies.Where(n => _actionAssmblies.Any(m => m == n.GetName().Name)))
            {
                actions.AddRange(assembly.GetExportedTypes().Where(x => x.IsSubclassOf(typeof(Action))));
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
            //foreach (var action in actions)
            //{
            //    if (!_handlers.ContainsKey(action))
            //    {
            //        throw new UnregisteredDomainCommandException("no handler registered for action: " + action.Name);
            //    }
            //}
            _actionTypes = new Dictionary<string, Type>();
            foreach (Type type in actions)
            {
                _actionTypes[type.Name] = type;
            }
            Log(actions);
        }

        private static void Log(List<Type> actions)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("ActionBus:{0}", Environment.NewLine);
            foreach (var action in actions)
            {
                sb.AppendFormat("{0}{1}", action, Environment.NewLine);
                Type value;
                if (_handlers.TryGetValue(action, out value))
                {
                    sb.AppendFormat("\t{0}{1}", value, Environment.NewLine);
                }
            }
            sb.AppendLine();
            log4net.LogManager.GetLogger("action.logger").Debug(sb);
        }

        public IActionHandler<T> GetHandler<T>() where T : Action
        {
            Type handler;
            if (!_handlers.TryGetValue(typeof(T), out handler))
            {
                throw new UnregisteredDomainCommandException("no handler registered for action: " + typeof(T));
            }

            return (IActionHandler<T>)ServiceLocator.Kernel.GetService(handler);
        }
    }
}
