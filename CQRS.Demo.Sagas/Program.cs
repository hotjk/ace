using CQRS.Demo.Contracts;
using CQRS.Demo.Contracts.Events;
using Grit.CQRS;
using Grit.CQRS.Actions;
using Grit.CQRS.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Demo.Sagas
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            // Pike a dummy method to ensoure Command/Event assembly been loaded
            CQRS.Demo.Contracts.EnsoureAssemblyLoaded.Pike();
            CQRS.Demo.Applications.EnsoureAssemblyLoaded.Pike();
            BootStrapper.BootStrap();

            var workers = new BlockingCollection<Worker>();
            for (int i = 0; i < 20; i++)
            {
                workers.Add(new Worker());
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
            Thread.Sleep(Timeout.Infinite);
        }

        public class Worker
        {
            public ActionResponse Execute(Grit.CQRS.Action action)
            {
                ActionResponse response = new ActionResponse { Result = ActionResponse.ActionResponseResult.OK };
                try
                {
                    ServiceLocator.ActionBus.Invoke((dynamic)action);
                }
                catch (BusinessException ex)
                {
                    response.Result = ActionResponse.ActionResponseResult.NG;
                    response.Message = ex.Message;
                }
                return response;
            }
        }
    }
}
