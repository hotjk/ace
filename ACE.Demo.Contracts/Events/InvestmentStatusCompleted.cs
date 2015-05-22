using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Contracts.Events
{
    public class InvestmentStatusCompleted : IEvent
    {
        public int InvestmentId { get; set; }
    }
}
