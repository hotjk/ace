using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ACE
{
    /// <summary>
    /// ACE, Action/Command/Event
    /// </summary>
    public abstract class DomainMessage
    {
        public enum MessageRouteState
        {
            Sent = 0,
            Received = 1
        }

        public DomainMessage()
        {
            this._id = Guid.NewGuid();
        }

        public Guid _id { get; set; }

        public string _type
        {
            get
            {
                return this.GetType().FullName;
            }
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

        #region Route State

        public MessageRouteState _state { get; private set; }
        public DateTime? _sendAt { get; private set; }
        public DateTime? _receiveAt { get; private set; }

        public void MarkAsSent()
        {
            this._state = DomainMessage.MessageRouteState.Sent;
            this._sendAt = DateTime.Now;
        }

        public void MarkAsReceived()
        {
            this._state = DomainMessage.MessageRouteState.Received;
            this._receiveAt = DateTime.Now;
        }

        #endregion

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
    }
}
