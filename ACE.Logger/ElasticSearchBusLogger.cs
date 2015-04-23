using Nest;
using System;

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
            public QDomainMessage Message { get; set; }
        }

        public void Sent(QDomainMessage message)
        {
            message.MarkAsSent();
            try
            {
                var repsonse = client.Index(message, i => i
                   .Id(message._id.ToString())
                   .Index(messageIndex));
            }
            catch
            {
            }
        }

        public void Received(QDomainMessage message)
        {
            message.MarkAsReceived();
            try
            { 
            var reponse = client.Update<QDomainMessage, object>(i => i
                .Id(message._id.ToString())
                .Index(messageIndex)
                .Upsert(message));
            }
            catch
            {
            }
        }

        public void Exception(QDomainMessage message, Exception ex)
        {
            try
            {
                var repsonse = client.Index(new DomainMessageException { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace, Message = message },
                    i => i.Id(message._id.ToString()).Index(exceptionIndex));
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
