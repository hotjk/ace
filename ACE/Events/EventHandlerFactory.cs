using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace ACE
{
    public class EventHandlerFactory : IEventHandlerFactory
    {
        private Autofac.IContainer _container;
        private IEnumerable<string> _eventAssmblies;
        private IEnumerable<string> _handlerAssmblies;
        private IDictionary<Type, List<Type>> _handlers;
        private readonly object _lockThis = new object();
        private IDictionary<string, Type> _eventTypes;

        public EventHandlerFactory(
            Autofac.IContainer container,
            IEnumerable<string> eventAssmblies,
            IEnumerable<string> handlerAssmblies)
        {
            lock (_lockThis)
            {
                _container = container;
                _eventAssmblies = eventAssmblies;
                _handlerAssmblies = handlerAssmblies;
                Utility.EnsoureAssemblyLoaded(_eventAssmblies);
                Utility.EnsoureAssemblyLoaded(_handlerAssmblies);
                HookHandlers();
                BindHandlers();
            }
        }

        public Type GetType(string eventName)
        {
            return _eventTypes[eventName];
        }

        private void HookHandlers()
        {
            _handlers = new Dictionary<Type, List<Type>>();

            List<Type> events = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies.Where(n => _eventAssmblies.Any(m => m == n.GetName().Name)))
            {
                events.AddRange(assembly.GetExportedTypes().Where(x => typeof(IEvent).IsAssignableFrom(x)).Where(x => !x.IsAbstract));
            }

            foreach (var assembly in assemblies.Where(n => _handlerAssmblies.Any(m => m == n.GetName().Name)))
            {
                var allHandlers = assembly.GetExportedTypes().Where(x => x.GetInterfaces()
                        .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IEventHandler<>)));
                foreach (var @event in events)
                {
                    var handlers = allHandlers
                        .Where(h => h.GetInterfaces()
                            .Any(i => i.GetGenericArguments()
                                .Any(e => e == @event))).ToList();
                    if (handlers.Count() == 0)
                    {
                        continue;
                    }
                    List<Type> value;
                    if (_handlers.TryGetValue(@event, out value))
                    {
                        _handlers[@event].AddRange(handlers);
                    }
                    else
                    {
                        _handlers[@event] = handlers;
                    }
                }
            }
            _eventTypes = new Dictionary<string, Type>();
            foreach (Type type in events)
            {
                _eventTypes[type.Name] = type;
            }
        }

        private void BindHandlers()
        {
            var builder = new ContainerBuilder();
            foreach (var kv in _handlers)
            {
                foreach (var type in kv.Value)
                {
                    builder.RegisterType(kv.Key).As(type).SingleInstance();
                }
            }
            builder.Update(_container);
        }

        private string Log()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("EventBus:{0}", Environment.NewLine);
            foreach (var kv in this._handlers)
            {
                sb.AppendFormat("{0}{1}", kv.Key, Environment.NewLine);
                if(kv.Value != null)
                {
                    foreach(var handler in kv.Value)
                    {
                        sb.AppendFormat("\t{0}{1}", handler, Environment.NewLine);
                    }
                }
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public IEnumerable<IEventHandler<T>> GetHandlers<T>() where T : IEvent
        {
            List<Type> handlers;
            if (_handlers.TryGetValue(typeof(T), out handlers))
            {
                return handlers.Select(handler => (IEventHandler<T>)_container.Resolve(handler)).ToList();
            }
            return null;
        }
    }
}
