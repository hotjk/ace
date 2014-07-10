using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Investments
{
    public interface IInvestmentService
    {
        Investment Get(int id);
        IEnumerable<Investment> GetAll();
    }
}
