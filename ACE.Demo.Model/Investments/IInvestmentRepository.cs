using System;
using System.Collections.Generic;

namespace ACE.Demo.Model.Investments
{
    public interface IInvestmentRepository
    {
        Investment Get(int id);
        IEnumerable<Investment> GetAll();
    }
}
