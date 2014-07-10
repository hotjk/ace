using Grit.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Contracts.Commands
{
    public class CompleteInvestment : Command
    {
        public int InvestmentId { get; set; }
    }
}
