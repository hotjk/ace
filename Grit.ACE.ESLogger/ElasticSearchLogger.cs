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
        private const string messageIndex = "message";
        private const string exceptionIndex = "exception";
        private ElasticClient client;
        
        public ElasticSearchLogger(string connectionString = "http://localhost:9200")
        {
            var node = new Uri(connectionString);
            var settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
        }

        public class DomainMessageException
        {
            public string ExceptionMessage { get; set; }
            public string StackTrace { get; set; }
            public DomainMessage Message { get; set; }
        }

        public void Sent(DomainMessage message)
        {
            message.Sent();
            var repsonse = client.Index(message, i => i
               .Id(message.Id.ToString())
               .Index(messageIndex));
        }

        public void Received(DomainMessage message)
        {
            message.Recevied();
            var reponse = client.Update<DomainMessage, object>(i => i
                .Id(message.Id.ToString())
                .Index(messageIndex)
                .Doc(new { RouteState = message.RouteState })
                .DocAsUpsert());
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
