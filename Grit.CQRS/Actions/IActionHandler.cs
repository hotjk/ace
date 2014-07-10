using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public interface IActionHandler<T> where T : Action
    {
        void Invoke(T action);
    }
}
