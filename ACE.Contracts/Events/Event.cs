using System.Linq;
using System.Text.RegularExpressions;
namespace ACE
{
    public class Event : QDomainMessage, IEvent
    {
        public enum EventDistributionOptions
        {
            BalckHole = 0,
            CurrentThread = 1,
            ThreadPool = 2,
            Queue = 4,
        }

        public Event()
        {
            _options = EventDistributionOptions.BalckHole;
        }

        /// <summary>
        /// Routing key is the RabbitMQ exchange routing topic.
        /// ProjectAmountChanged -> project.amount.changed 
        /// </summary>
        /// <returns></returns>
        public string RoutingKey()
        {
            return ToDotString(this.GetType().Name);
        }

        #region Converter between dot to camel

        private static Regex _regexCamel = new Regex("[a-z][A-Z]");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">HelleWorld</param>
        /// <returns>hello.world</returns>
        public static string ToDotString(string str)
        {
            return _regexCamel.Replace(str, m => m.Value[0] + "." + m.Value[1]).ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">hello.world</param>
        /// <returns>HelleWorld</returns>
        public static string ToCamelString(string str)
        {
            return string.Join("", str.Split(new char[] { '.' }).Select(n => char.ToUpper(n[0]) + n.Substring(1)));
        }

        #endregion

        #region Distribution Options

        public EventDistributionOptions _options { get; private set; }

        public bool ShouldDistributeInCurrentThread()
        {
            return (this._options & EventDistributionOptions.CurrentThread) == EventDistributionOptions.CurrentThread;
        }
        public bool ShouldDistributeInThreadPool()
        {
            return (this._options & EventDistributionOptions.ThreadPool) == EventDistributionOptions.ThreadPool;
        }
        public bool ShouldDistributeToExternalQueue()
        {
            return (this._options & EventDistributionOptions.Queue) == EventDistributionOptions.Queue;
        }

        public Event InCurrentThread()
        {
            this._options = this._options | EventDistributionOptions.CurrentThread;
            return this;
        }
        public Event InThreadPool()
        {
            this._options = this._options | EventDistributionOptions.ThreadPool;
            return this;
        }
        public Event ToExternalQueue()
        {
            this._options = this._options | EventDistributionOptions.Queue;
            return this;
        }

        #endregion
    }
}
