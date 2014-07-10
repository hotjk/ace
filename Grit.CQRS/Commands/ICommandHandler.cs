using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public interface ICommandHandler<T> where T : Command
    {
        void Execute(T command);
    }
}
