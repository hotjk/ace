using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Model
{
    public class HandlerBase
    {
        protected IEventBus EventBus { get; private set; }
        public HandlerBase(IEventBus eventBus)
        {
            EventBus = eventBus;
        }
    }
}
