using Grit.CQRS;
using Grit.CQRS.Exceptions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var factory = new ConnectionFactory() { Uri = Grit.Configuration.RabbitMQ.CQRSDemo };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("account_event_queue", false, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        var props = ea.BasicProperties;
                        Type type = ServiceLocator.EventBus.GetType(props.Type);
                        dynamic @event = JsonConvert.DeserializeObject(message, type);
                        try
                        {
                            ServiceLocator.EventBus.Handle(@event);
                        }
                        catch (Exception ex)
                        {
                            log4net.LogManager.GetLogger("exception.logger").Error(ex);
                        }
                        finally
                        {
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                    }
                }
            }
        }
    }
}
