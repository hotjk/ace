using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Messages
{
    public class Message
    {
        public int MessageId { get; set; }
        public int AccountId { get; set; }
        public string Content { get; set; }
    }
}
