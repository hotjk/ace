using Grit.ACE.Loggers;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.ACE.ESLogger
{
    public class ElasticSearchLogger : IBusLogger
    {
        private const string actionIndex = "action";
        private const string commandIndex = "command";
        private const string eventIndex = "event";
        private const string exceptionIndex = "exception";

        private ElasticClient client;
        
        public ElasticSearchLogger(string connectionString = "http://localhost:9200")
        {
            var node = new Uri(connectionString);
            var settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
        }

        public void ActionSend(Action action)
        {
            var repsonse = client.Index(action, i=>i
                .Id(action.Id.ToString())
                .Index(actionIndex));
        }

        public void ActionInvoke(Action action)
        {
            var reponse = client.Update<Action, object>(i => i
                .Id(action.Id.ToString())
                .Index(actionIndex)
                .Doc(new { RouteState = action.RouteState })
                .DocAsUpsert());
        }

        public void CommandSend(Command command)
        {
            var repsonse = client.Index(command, i => i
                .Id(command.Id.ToString())
                .Index(commandIndex));
        }

        public void EventPublish(Event @event)
        {
            var repsonse = client.Index(@event, i => i
                .Id(@event.Id.ToString())
                .Index(eventIndex));
        }

        public void EventHandle(Event @event)
        {
            var reponse = client.Update<Action, object>(i => i
                .Id(@event.Id.ToString())
                .Index(eventIndex)
                .Doc(new { RouteState = @event.RouteState })
                .DocAsUpsert());
        }

        public class DomainMessageException
        {
            public string ExceptionMessage { get; set; }
            public string StackTrace { get; set; }
            public DomainMessage Message { get; set; }
        }

        public void Exception(DomainMessage message, Exception ex)
        {
            var repsonse = client.Index(new DomainMessageException { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace, Message = message},
                i => i.Id(message.Id.ToString()).Index(exceptionIndex));
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }
    }
}
