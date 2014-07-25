using CQRS.Demo.Contracts;
using CQRS.Demo.Contracts.Events;
using Grit.CQRS;
using Grit.CQRS.Actions;
using Grit.CQRS.Exceptions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
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

            while (true)
            {
                try
                {
                    Run();
                }
                catch (Exception ex) // Ping rabbitmq exception
                {
                    log4net.LogManager.GetLogger("exception.logger").Error(ex);
                }
                Thread.Sleep(5000);
            }
        }

        private static void Run()
        {
            var factory = new ConnectionFactory() { Uri = Grit.Configuration.RabbitMQ.CQRSDemo };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(ServiceLocator.ActionBusQueue, false, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;
                        ActionResponse response = new ActionResponse { Result = ActionResponse.ActionResponseResult.OK };

                        try
                        {
                            Type type = ServiceLocator.ActionBus.GetType(props.Type);
                            dynamic action = JsonConvert.DeserializeObject(message, type);
                            try
                            {
                                ServiceLocator.ActionBus.Invoke(action);
                            }
                            catch (BusinessException ex)
                            {
                                response.Result = ActionResponse.ActionResponseResult.NG;
                                response.Message = ex.Message;
                            }
                        }
                        catch(RabbitMQ.Client.Exceptions.AlreadyClosedException ex)
                        {
                            response.Result = ActionResponse.ActionResponseResult.Exception;
                            response.Message = ex.Message;
                            log4net.LogManager.GetLogger("exception.logger").Error(ex);
                            return;
                        }
                        catch (Exception ex) // Deserialze action exception
                        {
                            response.Result = ActionResponse.ActionResponseResult.Exception;
                            response.Message = ex.Message;
                            log4net.LogManager.GetLogger("exception.logger").Error(ex);
                        }
                        finally
                        {

                            try
                            {
                                var responseBytes =
                                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                                channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
                                channel.BasicAck(ea.DeliveryTag, false);
                            }
                            catch (Exception ex) // Reply Ack exception
                            {
                                log4net.LogManager.GetLogger("exception.logger").Error(ex);
                            }
                        }
                    }
                }
            }
        }
    }
}
