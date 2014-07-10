using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public interface ICommandBus
    {
        ICommandBus Send<T>(T command) where T : Command;
    }
}
