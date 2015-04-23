using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ACE
{
    /// <summary>
    /// ACE, Action/Command/Event
    /// </summary>
    public abstract class QDomainMessage : IDomainMessage
    {
        public enum MessageRouteState
        {
            Initial = 0,
            Sent = 1,
            Received = 2
        }

        public QDomainMessage()
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

        #region Route State

        public MessageRouteState _state { get; set; }
        public DateTime? _sendAt { get; set; }
        public DateTime? _receiveAt { get; set; }

        public void MarkAsSent()
        {
            this._state = QDomainMessage.MessageRouteState.Sent;
            this._sendAt = DateTime.Now;
        }

        public void MarkAsReceived()
        {
            this._state = QDomainMessage.MessageRouteState.Received;
            this._receiveAt = DateTime.Now;
        }

        #endregion
    }
}
