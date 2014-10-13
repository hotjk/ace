using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.ACE.Actions
{
    public class ActionResponse : DomainMessage
    {
        public enum ActionResponseResult
        {
            OK = 0,
            NG = 1,
            Exception = 2,
        }
        public override Guid Id
        {
            get { return ResponseId; }
        }
        public Guid ResponseId { get; set; }
        public ActionResponseResult Result { get; set; }
        public string Message { get; set; }
    }
}
