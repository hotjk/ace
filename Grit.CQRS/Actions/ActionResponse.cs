using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS.Actions
{
    public class ActionResponse : DomainMessage
    {
        public enum ActionResponseResult
        {
            OK = 0,
            NG = 1,
            Exception = 2,
        }
        public ActionResponseResult Result { get; set; }
        public string Message { get; set; }
    }
}
