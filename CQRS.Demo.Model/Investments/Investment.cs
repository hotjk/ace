using CQRS.Demo.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Investments
{
    public class Investment
    {
        public int InvestmentId { get; set; }
        public int ProjectId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public InvestmentStatus Status { get; set; }
    }
}
