using ACE.Loggers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

namespace ACE
{
    public class EventBus : IEventBus
    {
        private IEventHandlerFactory _eventHandlerFactory;
        private EasyNetQ.IBus _bus;
        private IBusLogger _busLogger;
        private IList<dynamic> _events = new List<dynamic>();

        public EventBus(IBusLogger busLogger, IEventHandlerFactory eventHandlerFactory = null,
            EasyNetQ.IBus bus = null)
        {
            _eventHandlerFactory = eventHandlerFactory;
            _bus = bus;
            _busLogger = busLogger;
        }

        public void Publish<T>(T @event) where T : IEvent
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

        public void FlushAnEvent<T>(T @event) where T : IEvent
        {
            _busLogger.Sent(@event);
            DistributeToExternalQueue(@event);
            // Since TransactionScope has completed, can not write database in current thread scope.
            //Invoke((dynamic)@event);
            DistributeInThreadPool((dynamic)@event);
        }

        private void DistributeInThreadPool<T>(T @event) where T : IEvent   
        {
            if (_eventHandlerFactory == null) return;
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

        private void DistributeToExternalQueue<T>(T @event) where T : IEvent
        {
            try
            {
                _bus.Publish<ACE.IEvent>(@event, RoutingKey(@event));
            }
            catch (Exception ex)
            {
                _busLogger.Exception(@event, ex);
            }
        }

        public void Purge()
        {
            _events.Clear();
        }

        /// <summary>
        /// Routing key is the RabbitMQ exchange routing topic.
        /// ProjectAmountChanged -> project.amount.changed 
        /// </summary>
        /// <returns></returns>
        public static string RoutingKey(IEvent @event)
        {
            return ToDotString(@event.GetType().Name);
        }

        #region Converter between dot to camel

        private static Regex _regexCamel = new Regex("[a-z][A-Z]");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">HelleWorld</param>
        /// <returns>hello.world</returns>
        public static string ToDotString(string str)
        {
            return _regexCamel.Replace(str, m => m.Value[0] + "." + m.Value[1]).ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">hello.world</param>
        /// <returns>HelleWorld</returns>
        public static string ToCamelString(string str)
        {
            return string.Join("", str.Split(new char[] { '.' }).Select(n => char.ToUpper(n[0]) + n.Substring(1)));
        }

        #endregion
    }
}
