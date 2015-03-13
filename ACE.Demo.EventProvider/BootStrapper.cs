using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.EventConsumer
{
    public static class BootStrapper
    {
        public static void BootStrap()
        {
            ServiceLocator.Init(new ACE.Loggers.NullBusLogger(),
                Grit.Configuration.RabbitMQ.ACEQueueConnectionString, false, Event.EventDistributionOptions.Queue);
            AddIocBindings();
            InitHandlerFactory();
        }

        private static void AddIocBindings()
        {
        }

        private static void InitHandlerFactory()
        {
            
        }
    }
}
