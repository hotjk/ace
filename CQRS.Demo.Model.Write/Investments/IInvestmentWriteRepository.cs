using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Investments
{
    public interface IInvestmentWriteRepository
    {
        bool Add(Investment investment);
        Investment GetForUpdate(int investmentId);
        bool Complete(int investmentId);
    }
}
