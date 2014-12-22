﻿using Grit.ACE.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grit.ACE
{
    public class EventBus : IEventBus
    {
        private IEventHandlerFactory _eventHandlerFactory;
        private IList<dynamic> _events = new List<dynamic>(); // todo: a child class push into list will pop a base class instance.

        public EventBus(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
        }

        public void Publish<T>(T @event) where T : Event
        {
            _events.Add(@event);
        }

        public void Flush()
        {
            foreach (var @event in _events)
            {
                @event.Sent();
                FlushAnEvent(@event);
            }
            _events.Clear();
        }

        private void FlushAnEvent<T>(T @event) where T : Event
        {
            ServiceLocator.BusLogger.EventPublish(@event);
            if (@event.ShouldDistributeInCurrentThread())
            {
                DistributeInCurrentThread(@event);
            }
            if (@event.ShouldDistributeInThreadPool())
            {
                DistributeInThreadPool(@event);
            }
            if (ServiceLocator.EventShouldDistributeToExternalQueue && @event.ShouldDistributeToExternalQueue())
            {
                DistributeToExternalQueue(@event);
            }
        }

        private void DistributeInCurrentThread<T>(T @event) where T : Event
        {
            var handlers = _eventHandlerFactory.GetHandlers<T>();
            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    try
                    {
                        handler.Handle(@event);
                    }
                    catch (Exception ex)
                    {
                        ServiceLocator.BusLogger.Exception(@event, ex);
                    }
                }
            }
        }

        private void DistributeInThreadPool<T>(T @event) where T : Event
        {
            var handlers = _eventHandlerFactory.GetHandlers<T>();
            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    // handle event in thread pool
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        try
                        {
                            handler.Handle(@event);
                        }
                        catch (Exception ex)
                        {
                            ServiceLocator.BusLogger.Exception(@event, ex);
                        }
                    });
                }
            }
        }

        private static void DistributeToExternalQueue<T>(T @event) where T : Event
        {
            try
            {
                ServiceLocator.EasyNetQBus.Publish<Grit.ACE.Event>(@event, @event.RoutingKey());
            }
            catch (Exception ex)
            {
                ServiceLocator.BusLogger.Exception(@event, ex);
            }
        }

        public void Invoke<T>(T @event) where T : Event
        {
            var handlers = _eventHandlerFactory.GetHandlers<T>();
            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    @event.Recevied();
                    ServiceLocator.BusLogger.EventHandle(@event);
                    try
                    {
                        // handle event in current thread
                        handler.Handle(@event);
                    }
                    catch (Exception ex)
                    {
                        ServiceLocator.BusLogger.Exception(@event, ex);
                    }
                }
            }
        }

        public void Purge()
        {
            _events.Clear();
        }

        public void Subscribe(string subscriptionId, string topic)
        {
            var worker = new EventWorker();

            ServiceLocator.EasyNetQBus.Subscribe<Event>(subscriptionId,
                @event => worker.Execute(@event),
                x => x.WithTopic(topic));
        }

        public void SubscribeInParallel(string subscriptionId, string topic, int capacity)
        {
            var workers = new BlockingCollection<EventWorker>();
            for (int i = 0; i < capacity; i++)
            {
                workers.Add(new EventWorker());
            }

            ServiceLocator.EasyNetQBus.SubscribeAsync<Event>(subscriptionId,
                @event => Task.Factory.StartNew(() =>
                {
                    var worker = workers.Take();
                    try
                    {
                        worker.Execute(@event);
                    }
                    finally
                    {
                        workers.Add(worker);
                    }
                }),
                x => x.WithTopic(topic));
        }
    }
}
