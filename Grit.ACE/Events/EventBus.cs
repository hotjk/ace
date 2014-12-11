using Grit.ACE.Events;
using Newtonsoft.Json;
using RabbitMQ.Client.Framing.v0_9_1;
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
        private IList<Tuple<Event, EventPublishOptions>> _events = new List<Tuple<Event, EventPublishOptions>>();

        public EventBus(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
        }

        public void Publish<T>(T @event, EventPublishOptions options = EventPublishOptions.BalckHole) where T : Event
        {
            _events.Add(new Tuple<Event, EventPublishOptions>(@event, options));
        }

        public void Flush()
        {
            foreach (var tuple in _events)
            {
                FlushAnEvent(tuple.Item1, tuple.Item2);
            }
            _events.Clear();
        }

        private void FlushAnEvent<T>(T @event, EventPublishOptions options) where T : Event
        {
            ServiceLocator.BusLogger.EventPublish(@event);
            if ((options & EventPublishOptions.CurrentThread) == EventPublishOptions.CurrentThread)
            {
                DistributeEventInCurrentThread(@event);
            }
            if ((options & EventPublishOptions.ThreadPool) == EventPublishOptions.ThreadPool)
            {
                DistributeEventInThreadPool(@event);
            }
            if (ServiceLocator.DistributeEventToQueue && 
                ((options & EventPublishOptions.Queue) == EventPublishOptions.Queue))
            {
                DistributeAnEventToQueue(@event);
            }
        }

        private void DistributeEventInCurrentThread<T>(T @event) where T : Event
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

        private void DistributeEventInThreadPool<T>(T @event) where T : Event
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

        private static void DistributeAnEventToQueue<T>(T @event) where T : Event
        {
            try
            {
                ServiceLocator.EasyNetQBus.Publish(@event, @event.RoutingKey);
            }
            catch (Exception ex)
            {
                ServiceLocator.BusLogger.Exception(@event, ex);
            }
        }

        public void Invoke<T>(T @event) where T : Event
        {
            ServiceLocator.BusLogger.EventHandle(@event);

            var handlers = _eventHandlerFactory.GetHandlers<T>();
            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
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
