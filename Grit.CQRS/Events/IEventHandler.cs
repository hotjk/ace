using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public interface IEventHandler<T> where T : Event
    {
        void Handle(T handle);
    }
}
