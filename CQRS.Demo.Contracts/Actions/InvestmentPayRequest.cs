using Grit.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Contracts.Actions
{
    public class InvestmentPayRequest : Grit.CQRS.Action
    {
        public int InvestmentId { get; set; }
    }
}
