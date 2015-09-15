using ACE.Loggers;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Autofac;

namespace ACE
{
    public class ActionStation : ACE.IActionStation
    {
        private IActionHandlerFactory _actionHandlerFactory;
        private EasyNetQ.IBus _bus;
        private IBusLogger _busLogger;
        private IContainer _container;

        public ActionStation(IBusLogger busLogger, 
            IContainer container = null,
            IActionHandlerFactory ActionHandlerFactory = null,
            EasyNetQ.IBus bus = null)
        {
            _container = container;
            _actionHandlerFactory = ActionHandlerFactory;
            _bus = bus;
            _busLogger = busLogger;
        }

        public void Invoke<T>(T action) where T : IAction
        {
            var handler = _actionHandlerFactory.GetHandler<T>();
            if (handler != null)
            {
                _busLogger.Received(action);
                try
                {
                    handler.Invoke(action);
                }
                catch (Exception ex)
                {
                    if (!(ex is ACE.Exceptions.BusinessException))
                    {
                        _busLogger.Exception(action, ex);
                    }
                    throw;
                }
            }
        }

        private ActionResponse Work(ACE.IAction action)
        {
            ActionResponse response = new ActionResponse();
            try
            {
                using (var scope = _container.BeginLifetimeScope())
                {
                    Invoke((dynamic)action);
                }
            }
            catch (ACE.Exceptions.BusinessException ex)
            {
                response.Result = ActionResponse.ActionResponseResult.NG;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Result = ActionResponse.ActionResponseResult.Exception;
                response.Message = ex.Message;
            }
            _busLogger.Sent(response);
            return response;
        }

        public void Subscribe<T>() where T : class, ACE.IAction
        {
            _bus.Respond<T, ActionResponse>(action => Work(action));
        }

        public void SubscribeInParallel<T>(int capacity) where T : class, ACE.IAction
        {
            var workers = new BlockingCollection<int>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                workers.Add(i);
            }

            _bus.RespondAsync<T, ActionResponse>(action =>
                Task.Factory.StartNew(() =>
                {
                    var worker = workers.Take();
                    try
                    {
                        return Work(action);
                    }
                    finally
                    {
                        workers.Add(worker);
                    }
                }));
        }
    }
}
