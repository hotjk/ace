using ACE.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Contracts.Services
{
    public class GetInvestmentRequest : IService
    {
        public int Id { get; set; }
    }
}
