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

        private ActionResponse Work(ACE.Action action)
        {
            ActionResponse response = new ActionResponse { Result = ActionResponse.ActionResponseResult.OK };
            try
            {
                Invoke((dynamic)action);
            }
            catch (ACE.Exceptions.BusinessException ex)
            {
                response.Result = ActionResponse.ActionResponseResult.NG;
                response.Message = ex.Message;
            }
            return response;
        }

        public void Subscribe<T>() where T : ACE.Action
        {
            ServiceLocator.EasyNetQBus.Respond<T, ActionResponse>(action => Work(action));
        }

        public void SubscribeInParallel<T>(int capacity) where T : ACE.Action
        {
            var workers = new BlockingCollection<int>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                workers.Add(i);
            }

            ServiceLocator.EasyNetQBus.RespondAsync<T, ActionResponse>(action =>
                Task.Factory.StartNew(() =>
                {
                    var worker = workers.Take();
                    try
                    {
                        Console.WriteLine(worker);
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
