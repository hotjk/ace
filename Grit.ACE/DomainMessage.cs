using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grit.ACE
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
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public MessageRouteState RouteState { get; private set; }

        public string Type
        {
            get
            {
                return this.GetType().FullName;
            }
        }

        public string RoutingKey()
        {
            return ToDotString(this.GetType().Name);
        }

        public void Sent()
        {
            this.RouteState = DomainMessage.MessageRouteState.Sent;
        }

        public void Recevied()
        {
            this.RouteState = DomainMessage.MessageRouteState.Received;
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
    }
}
