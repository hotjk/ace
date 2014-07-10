using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Demo.Web.Models
{
    public class InvestViewModel
    {
        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
    }
}