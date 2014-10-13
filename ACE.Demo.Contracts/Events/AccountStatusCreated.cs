using Grit.ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Contracts.Events
{
    public class AccountStatusCreated : Event
    {
        public int AccountId { get; set; }
    }
}
