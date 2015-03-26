using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Application
{
    public class ProcessManagerBase
    {
        protected ICommandBus CommandBus { get; private set; }

        public ProcessManagerBase(ICommandBus commandBus)
        {
            CommandBus = commandBus;
        }
    }
}
