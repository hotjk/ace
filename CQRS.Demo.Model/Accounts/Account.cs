using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Accounts
{
    public class Account
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
