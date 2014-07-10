using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Investments
{
    public class InvestmentService : IInvestmentService
    {
        private IInvestmentRepository _repository;
        public InvestmentService(IInvestmentRepository repository)
        {
            _repository = repository;
        }

        public Investment Get(int id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<Investment> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
