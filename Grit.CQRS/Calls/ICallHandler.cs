using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public interface ICallHandler<T> where T : Call
    {
        void Invoke(T call);
    }
}
