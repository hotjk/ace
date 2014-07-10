using Grit.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Contracts.Actions
{
    public class InvestmentCreateRequest : Grit.CQRS.Action
    {
        public int InvestmentId { get; set; }
        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
    }
}
