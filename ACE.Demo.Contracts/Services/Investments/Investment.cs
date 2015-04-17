using ACE.Demo.Contracts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Contracts.Services
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
