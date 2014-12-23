using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Loggers
{
    public class ElasticSearchBusLogger : IBusLogger
    {
        private const string messageIndex = "message";
        private const string exceptionIndex = "exception";
        private ElasticClient client;
        
        public ElasticSearchBusLogger(string connectionString = "http://localhost:9200")
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
            message.MarkedAsSent();
            try
            {
                var repsonse = client.Index(message, i => i
                   .Id(message.Id.ToString())
                   .Index(messageIndex));
            }
            catch
            {
            }
        }

        public void Received(DomainMessage message)
        {
            message.MarkedAsReceived();
            try
            { 
            var reponse = client.Update<DomainMessage, object>(i => i
                .Id(message.Id.ToString())
                .Index(messageIndex)
                .Upsert(message));
            }
            catch
            {
            }
        }

        public void Exception(DomainMessage message, Exception ex)
        {
            try
            {
                var repsonse = client.Index(new DomainMessageException { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace, Message = message },
                    i => i.Id(message.Id.ToString()).Index(exceptionIndex));
            }
            catch
            {
            }
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }
    }
}
