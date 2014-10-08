using Grit.CQRS;
using Grit.CQRS.Exceptions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Demo.EventConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            // Pike a dummy method to ensoure Command/Event assembly been loaded
            CQRS.Demo.Contracts.EnsoureAssemblyLoaded.Pike();
            BootStrapper.BootStrap();

            var workers = new BlockingCollection<Worker>();
            for (int i = 0; i < 20; i++)
            {
                workers.Add(new Worker());
            }

            ServiceLocator.EasyNetQBus.SubscribeAsync<Event>("Account", 
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
                x=>x.WithTopic("account.*.*"));
            Thread.Sleep(Timeout.Infinite);
        }

        public class Worker
        {
            public void Execute(Grit.CQRS.Event @event)
            {
                try
                {
                    ServiceLocator.EventBus.Handle(@event);
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("exception.logger").Error(ex);
                }
            }
        }
    }
}
