using ACE.Loggers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

namespace ACE
{
    public class EventStation : IEventStation
    {
        private IEventHandlerFactory _eventHandlerFactory;
        private EasyNetQ.IBus _bus;
        private IBusLogger _busLogger;
        private IList<dynamic> _events = new List<dynamic>();

        public EventStation(IBusLogger busLogger,
            IEventHandlerFactory eventHandlerFactory = null,
            EasyNetQ.IBus bus = null)
        {
            _eventHandlerFactory = eventHandlerFactory;
            _bus = bus;
            _busLogger = busLogger;
        }

        public void Invoke<T>(T @event) where T : IEvent
        {
            var handlers = _eventHandlerFactory.GetHandlers<T>();
            if (handlers != null)
            {
                _busLogger.Received(@event);
                foreach (var handler in handlers)
                {
                    try
                    {
                        // handle event in current thread
                        handler.Handle(@event);
                    }
                    catch (ACE.Exceptions.BusinessException)
                    {
                    }
                    catch (Exception ex)
                    {
                        _busLogger.Exception(@event, ex);
                        throw;
                    }
                }
            }
        }

        private void Work(ACE.IEvent @event)
        {
            Invoke((dynamic)@event);
        }

        public void Subscribe(string subscriptionId, string[] topics, bool withAutoDelete = false, bool exclusive = false)
        {
            _bus.Subscribe<IEvent>(subscriptionId,
                @event => Work(@event),
                x =>
                {
                    if (withAutoDelete) { x.WithAutoDelete(); }
                    if (exclusive) { x.AsExclusive(); }
                    foreach (var topic in topics)
                    {
                        x.WithTopic(topic);
                    }
                });
        }

        public void SubscribeAsync(string subscriptionId, string[] topics, Func<IEvent, Task> onMessage, bool withAutoDelete = false, bool exclusive = false)
        {
            _bus.SubscribeAsync<IEvent>(
                subscriptionId,
                onMessage,
                x =>
                {
                    if (withAutoDelete) { x.WithAutoDelete(); }
                    if (exclusive) { x.AsExclusive(); }
                    foreach (var topic in topics)
                    {
                        x.WithTopic(topic);
                    }
                });
        }

        public void Subscribe(string subscriptionId, string[] topics, Action<IEvent> onMessage, bool withAutoDelete = false, bool exclusive = false)
        {
            _bus.Subscribe<IEvent>(subscriptionId,
                onMessage,
                x =>
                {
                    if (withAutoDelete) { x.WithAutoDelete(); }
                    if (exclusive) { x.AsExclusive(); }
                    foreach (var topic in topics)
                    {
                        x.WithTopic(topic);
                    }
                });
        }

        public void SubscribeInParallel(string subscriptionId, string[] topics, int capacity, bool withAutoDelete = false, bool exclusive = false)
        {
            var workers = new BlockingCollection<int>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                workers.Add(i);
            }

            _bus.SubscribeAsync<IEvent>(subscriptionId,
                @event => Task.Factory.StartNew(() =>
                {
                    var worker = workers.Take();
                    try
                    {
                        Work(@event);
                    }
                    finally
                    {
                        workers.Add(worker);
                    }
                }),
                x =>
                {
                    if (withAutoDelete) { x.WithAutoDelete(); }
                    if (exclusive) { x.AsExclusive(); }
                    foreach (var topic in topics)
                    {
                        x.WithTopic(topic);
                    }
                });
        }
    }
}
