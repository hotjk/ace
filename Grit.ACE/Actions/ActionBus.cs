using Grit.ACE.Actions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.v0_9_1;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grit.ACE
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
                ServiceLocator.BusLogger.ActionBeginInvoke(action);
                try
                {
                    handler.Invoke(action);
                }
                finally
                {
                    ServiceLocator.BusLogger.ActionEndInvoke(action);
                }
            }
        }

        public ActionResponse Send<T>(T action) where T : Action
        {
            ServiceLocator.BusLogger.ActionSend(action);
            
            if (!ServiceLocator.DistributeActionToQueue)
            {
                throw new Exception("Action is not allow to distribute to queue, maybe you can direct invoke action in thread.");
            }
            
            return ServiceLocator.EasyNetQBus.Request<Action, ActionResponse>(action);
        }

        public async Task<ActionResponse> SendAsync<T>(T action) where T : Action
        {
            ServiceLocator.BusLogger.ActionSend(action);

            if (!ServiceLocator.DistributeActionToQueue)
            {
                throw new Exception("Action is not allow to distribute to queue, maybe you can direct invoke action in thread.");
            }
            
            return await ServiceLocator.EasyNetQBus.RequestAsync<Action, ActionResponse>(action);
        }

        public void Subscribe()
        {
            ActionWorker worker = new ActionWorker();
            ServiceLocator.EasyNetQBus.Respond<Grit.ACE.Action, ActionResponse>(action => worker.Execute(action));
        }

        public void SubscribeInParallel(int capacity)
        {
            var workers = new BlockingCollection<ActionWorker>();
            for (int i = 0; i < capacity; i++)
            {
                workers.Add(new ActionWorker());
            }

            ServiceLocator.EasyNetQBus.RespondAsync<Grit.ACE.Action, ActionResponse>(action =>
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
