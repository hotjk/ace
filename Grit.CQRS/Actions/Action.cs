using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public class Action : DomainMessage, IAction
    {
        public Action()
        {
            ActionId = Guid.NewGuid();
        }
        public override Guid Id
        {
            get
            {
                return ActionId;
            }
        }
        public Guid ActionId { get; set; }
    }
}
