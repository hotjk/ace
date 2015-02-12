using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Application
{
    public class ProcessManagerBase
    {
        protected IServiceLocator ServiceLocator { get; private set; }

        public ProcessManagerBase(IServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;
        }
    }
}
