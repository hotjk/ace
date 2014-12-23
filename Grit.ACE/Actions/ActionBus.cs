using ACE.Actions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE
{
    public class ActionBus : IActionBus
    {
        private IActionHandlerFactory _actionHandlerFactory;

        public ActionBus(IActionHandlerFactory ActionHandlerFactory)
        {
            _actionHandlerFactory = ActionHandlerFactory;
        }

        public void Invoke<T>(T action) where T : Action
        {
            var handler = _actionHandlerFactory.GetHandler<T>();
            if (handler != null)
            {
                action.MarkedAsReceived();
                ServiceLocator.BusLogger.Received(action);
                try
                {
                    handler.Invoke(action);
                }
                catch (Exception ex)
                {
                    if (!(ex is ACE.Exceptions.BusinessException))
                    {
                        ServiceLocator.BusLogger.Exception(action, ex);
                    }
                    throw;
                }
            }
        }

        public ActionResponse Send<B, T>(T action)
            where B : Action
            where T : B
        {
            if (!ServiceLocator.ActionShouldDistributeToExternalQueue)
            {
                throw new Exception("Action is not allow to distribute to queue, maybe you can direct invoke action in thread.");
            }
            ServiceLocator.BusLogger.Sent(action);
            return ServiceLocator.EasyNetQBus.Request<B, ActionResponse>(action);
        }

        public async Task<ActionResponse> SendAsync<B, T>(T action)
            where B : Action
            where T : B
        {
            if (!ServiceLocator.ActionShouldDistributeToExternalQueue)
            {
                throw new Exception("Action is not allow to distribute to queue, maybe you can direct invoke action in thread.");
            }
            ServiceLocator.BusLogger.Sent(action);
            return await ServiceLocator.EasyNetQBus.RequestAsync<B, ActionResponse>(action);
        }

        public void Subscribe<T>() where T : ACE.Action
        {
            ActionWorker worker = new ActionWorker();
            ServiceLocator.EasyNetQBus.Respond<T, ActionResponse>(action => worker.Execute(action));
        }

        public void SubscribeInParallel<T>(int capacity) where T : ACE.Action
        {
            var workers = new BlockingCollection<ActionWorker>();
            for (int i = 0; i < capacity; i++)
            {
                workers.Add(new ActionWorker());
            }

            ServiceLocator.EasyNetQBus.RespondAsync<T, ActionResponse>(action =>
                Task.Factory.StartNew(() =>
                {
                    var worker = workers.Take();
                    try
                    {
                        return worker.Execute(action);
                    }
                    finally
                    {
                        workers.Add(worker);
                    }
                }));
        }
    }
}
