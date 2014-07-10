using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grit.CQRS;

namespace CQRS.Demo.Contracts.Commands
{
    public class CreateInvestment : Command
    {
        public int InvestmentId { get; set; }
        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
    }
}
