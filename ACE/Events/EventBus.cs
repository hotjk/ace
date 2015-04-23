using ACE.Loggers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACE
{
    public class EventBus : IEventBus
    {
        private IEventHandlerFactory _eventHandlerFactory;
        private EasyNetQ.IBus _bus;
        private IBusLogger _busLogger;
        private IList<dynamic> _events = new List<dynamic>();
        public Event.EventDistributionOptions _eventDistributionOptions;
        public bool EventShouldDistributeInCurrentThread
        {
            get
            {
                return (_eventDistributionOptions & Event.EventDistributionOptions.CurrentThread) == Event.EventDistributionOptions.CurrentThread;
            }
        }
        public bool EventShouldDistributeInThreadPool
        {
            get
            {
                return (_eventDistributionOptions & Event.EventDistributionOptions.ThreadPool) == Event.EventDistributionOptions.ThreadPool;
            }
        }
        public bool EventShouldDistributeToExternalQueue
        {
            get
            {
                return (_eventDistributionOptions & Event.EventDistributionOptions.Queue) == Event.EventDistributionOptions.Queue;
            }
        }

        public EventBus(IBusLogger busLogger, IEventHandlerFactory eventHandlerFactory = null,
            Event.EventDistributionOptions eventDistributionOptions = Event.EventDistributionOptions.BalckHole,
            EasyNetQ.IBus bus = null)
        {
            _eventDistributionOptions = eventDistributionOptions;
            _eventHandlerFactory = eventHandlerFactory;
            _bus = bus;
            _busLogger = busLogger;
            
            if (EventShouldDistributeToExternalQueue && _bus == null)
            {
                throw new Exception("IBus is required when distribute event to queue.");
            }
        }

        public void Publish<T>(T @event) where T : Event
        {
            _events.Add(@event);
        }

        public void Flush()
        {
            foreach (var @event in _events)
            {
                FlushAnEvent(@event);
            }
            _events.Clear();
        }

        private void FlushAnEvent<T>(T @event) where T : Event
        {
            _busLogger.Sent(@event);
            // Handle event in current thread or in thread pool will change event, so dispatch orignal event to queue first.
            if (EventShouldDistributeToExternalQueue && @event.ShouldDistributeToExternalQueue())
            {
                DistributeToExternalQueue(@event);
            }
            if (@event.ShouldDistributeInCurrentThread())
            {
                Invoke((dynamic)@event);
            }
            if (@event.ShouldDistributeInThreadPool())
            {
                DistributeInThreadPool((dynamic)@event);
            }
        }

        private void DistributeInThreadPool<T>(T @event) where T : Event
        {
            var handlers = _eventHandlerFactory.GetHandlers<T>();
            if (handlers != null)
            {
                _busLogger.Received(@event);
                foreach (var handler in handlers)
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            handler.Handle(@event);
                        }
                        catch (Exception ex)
                        {
                            _busLogger.Exception(@event, ex);
                        }
                    });
                }
            }
        }

        private void DistributeToExternalQueue<T>(T @event) where T : Event
        {
            try
            {
                _bus.Publish<ACE.Event>(@event, @event.RoutingKey());
            }
            catch (Exception ex)
            {
                _busLogger.Exception(@event, ex);
            }
        }

        public void Invoke<T>(T @event) where T : Event
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
                    catch(ACE.Exceptions.BusinessException)
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

        public void Purge()
        {
            _events.Clear();
        }

        private void Work(ACE.Event @event)
        {
            Invoke((dynamic)@event);
        }

        public void Subscribe(string subscriptionId, string[] topics)
        {
            foreach (var topic in topics)
            {
                _bus.Subscribe<Event>(subscriptionId,
                    @event => Work(@event),
                    x => x.WithTopic(topic));
            }
        }

        public void SubscribeInParallel(string subscriptionId, string[] topics, int capacity)
        {
            var workers = new BlockingCollection<int>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                workers.Add(i);
            }

            foreach (var topic in topics)
            {
                _bus.SubscribeAsync<Event>(subscriptionId,
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
                    x => x.WithTopic(topic));
            };
        }
    }
}
