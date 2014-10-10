using Grit.CQRS.Actions;
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

namespace Grit.CQRS
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
            log4net.LogManager.GetLogger("action.logger").Info(
                string.Format("Action BeginInvoke {0}", action.Id));

            var handler = _actionHandlerFactory.GetHandler<T>();
            if (handler != null)
            {
                handler.Invoke(action);
            }
            log4net.LogManager.GetLogger("action.logger").Info(
                string.Format("Action EndInvoke {0}", action.Id));
        }

        public ActionResponse Send<T>(T action) where T : Action
        {
            var task = SendAsync(action);
            task.Wait();
            return task.Result;
        }

        public async Task<ActionResponse> SendAsync<T>(T action) where T : Action
        {
            string json = JsonConvert.SerializeObject(action);
            log4net.LogManager.GetLogger("action.logger").Info(
                string.Format("Action Send {0} {1}", action, json));

            return await ServiceLocator.EasyNetQBus.RequestAsync<Action, ActionResponse>(action);
        }

        public void Handle()
        {
            ActionWorker worker = new ActionWorker();
            ServiceLocator.EasyNetQBus.Respond<Grit.CQRS.Action, ActionResponse>(action => worker.Execute(action));
        }

        public void HandleInParallel(int capacity)
        {
            var workers = new BlockingCollection<ActionWorker>();
            for (int i = 0; i < capacity; i++)
            {
                workers.Add(new ActionWorker());
            }

            ServiceLocator.EasyNetQBus.RespondAsync<Grit.CQRS.Action, ActionResponse>(action =>
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
